using HEVEQ.Application.Features.CustomerDashboard.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.CustomerDashboard.Queries.GetCustomerDashboardSummary;

public record GetCustomerDashboardSummaryQuery : IRequest<CustomerDashboardSummaryDto>;