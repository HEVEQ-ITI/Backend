using Hangfire;
using HEVEQ.Api.Middleware;
using HEVEQ.Application;
using HEVEQ.Application.Common.Jobs;
using HEVEQ.Application.Common.Mappings;
using HEVEQ.Infrastructure;
using HEVEQ.Infrastructure.Identity;
using HEVEQ.Infrastructure.Services.BackgroundJobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

namespace HEVEQ.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var envPath = Path.Combine(builder.Environment.ContentRootPath, ".env");

            if (File.Exists(envPath))
            {
                Env.Load(envPath);
                builder.Configuration.AddEnvironmentVariables();
            }

            // Application Layer Dependencies
            builder.Services.AddApplication(builder.Configuration);

            // Infrastructure Layer Dependencies
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularClient", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddHangfire(config => {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHangfireServer();

            var app = builder.Build();
            app.UseHangfireDashboard("/hangfire");
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                var backgroundJobOptions = scope.ServiceProvider
                    .GetRequiredService<IOptions<BackgroundJobOptions>>()
                    .Value;

                recurringJobs.AddOrUpdate<ProviderResponseSlaJob>(
                    "booking-provider-response-sla",
                    job => job.RunAsync(),
                    backgroundJobOptions.ProviderResponseSlaCron
                );

                recurringJobs.AddOrUpdate<CustomerCompletionAutoConfirmJob>(
                    "booking-customer-completion-auto-confirm",
                    job => job.RunAsync(),
                    backgroundJobOptions.CustomerCompletionAutoConfirmCron
                );

                recurringJobs.AddOrUpdate<EscrowReleaseAfterCompletionJob>(
                    "booking-escrow-release-after-completion",
                    job => job.RunAsync(),
                    backgroundJobOptions.EscrowReleaseAfterCompletionCron
                );

                recurringJobs.AddOrUpdate<MarketplaceAutoConfirmJob>(
                    "marketplace-auto-confirm",
                    job => job.RunAsync(),
                    backgroundJobOptions.MarketplaceAutoConfirmCron
                );

                recurringJobs.AddOrUpdate<MarketplaceEscrowReleaseJob>(
                    "marketplace-escrow-release",
                    job => job.RunAsync(),
                    backgroundJobOptions.MarketplaceEscrowReleaseCron
                );
            }

            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
            }

            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AngularClient");
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
