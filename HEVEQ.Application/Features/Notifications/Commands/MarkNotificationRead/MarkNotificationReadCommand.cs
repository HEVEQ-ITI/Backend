using HEVEQ.Application.Features.Notifications.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Notifications.Commands.MarkNotificationRead;

public record MarkNotificationReadCommand(Guid NotificationId)
    : IRequest<MarkNotificationReadResult>;