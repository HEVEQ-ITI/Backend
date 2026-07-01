using System;
using HEVEQ.Application.Features.ProviderCalendar.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.ProviderCalendar.Queries.GetProviderCalendar;

public record GetProviderCalendarQuery(DateOnly From, DateOnly To) : IRequest<ProviderCalendarResultDto>;