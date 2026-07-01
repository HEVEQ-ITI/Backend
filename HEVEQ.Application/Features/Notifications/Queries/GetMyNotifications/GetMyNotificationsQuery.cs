using HEVEQ.Application.Features.Notifications.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Notifications.Queries.GetMyNotifications;

public record GetMyNotificationsQuery(
    bool? IsRead,      
    int Page,
    int PageSize
) : IRequest<NotificationListDto>;