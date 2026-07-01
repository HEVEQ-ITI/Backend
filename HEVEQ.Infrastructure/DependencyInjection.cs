using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Payments;
using HEVEQ.Domain.Identity;
using HEVEQ.Infrastructure.Payments.Stripe;
using HEVEQ.Infrastructure.Persistence;
using HEVEQ.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HEVEQ.Application.Common.Jobs;
using HEVEQ.Infrastructure.Services.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection.Extensions;



namespace HEVEQ.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.UseNetTopologySuite());
        });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.Configure<JwtHelper>(configuration.GetSection("JWT"));
        services.AddScoped<IJwtService, JwtService>();

        services.Configure<StripePaymentOptions>(configuration.GetSection("StripePayment"));
        services.Configure<PaymentPlatformOptions>(configuration.GetSection("PaymentPlatform"));
        services.AddScoped<IPaymentCheckoutService, StripePaymentCheckoutService>();

        services.Configure<BackgroundJobOptions>(configuration.GetSection("BackgroundJobs"));
        services.AddScoped<ProviderResponseSlaJob>();
        services.AddScoped<CustomerCompletionAutoConfirmJob>();
        services.AddScoped<EscrowReleaseAfterCompletionJob>();

        services.AddScoped<MarketplaceAutoConfirmJob>();
        services.AddScoped<MarketplaceEscrowReleaseJob>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();


        return services;
    }
}