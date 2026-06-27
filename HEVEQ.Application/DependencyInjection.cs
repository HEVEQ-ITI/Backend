using FluentValidation;
using HEVEQ.Application.Common.AI;
using HEVEQ.Application.Common.Behaviours;
using HEVEQ.Application.Features.Bookings.Services.Implementation;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HEVEQ.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        services.AddAutoMapper(cfg => { }, assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddScoped<IBookingAddressResolver, BookingAddressResolver>();
        services.AddScoped<IBookingCreationService, BookingCreationService>();
        services.AddScoped<ICancellationPolicyService, CancellationPolicyService>();

        services.Configure<AiSettings>(configuration.GetSection("AiSettings"));

        return services;
    }
}