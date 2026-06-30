using System;
using HEVEQ.Application.Features.ProviderEarnings.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.ProviderEarnings.Queries.GetProviderEarningsServiceSummary;

public record GetProviderEarningsServiceSummaryQuery(DateOnly From, DateOnly To)
    : IRequest<ProviderEarningsServiceSummaryDto>;