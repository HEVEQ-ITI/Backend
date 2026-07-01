using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Notifications.DTOs;

namespace HEVEQ.Application.Features.Notifications.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandler
    : IRequestHandler<GetMyNotificationsQuery, NotificationListDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetMyNotificationsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<NotificationListDto> Handle(
        GetMyNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // Ownership enforced: user can only see their own notifications
        // UnreadCount is always total unread — does NOT respect the isRead filter
        var unreadCount = await _context.Notifications
            .AsNoTracking()
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        // Apply optional isRead filter on top of ownership filter
        var filteredQuery = _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId);

        if (request.IsRead.HasValue)
            filteredQuery = filteredQuery.Where(n => n.IsRead == request.IsRead.Value);

        var totalCount = await _context.Notifications
     .AsNoTracking()
     .CountAsync(n => n.UserId == userId, cancellationToken);

        var items = await filteredQuery
            .OrderByDescending(n => n.SentAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationItemDto
            {
                Id = n.Id,
                Title = n.Title,
                Body = n.Body,
                EventType = n.EventType,
                ReferenceId = n.ReferenceId,
                ReferenceType = n.ReferenceType,
                IsRead = n.IsRead,
                SentAt = n.SentAt
            })
            .ToListAsync(cancellationToken);

        return new NotificationListDto
        {
            Items = items,
            UnreadCount = unreadCount,
            TotalCount = totalCount
        };
    }
}