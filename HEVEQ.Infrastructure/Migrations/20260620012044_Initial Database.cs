using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace HEVEQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FcmToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainEventQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FailureReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEventQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AiInteractionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgentType = table.Column<int>(type: "int", nullable: false),
                    InvocationContext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AiRecommendation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AiRiskScore = table.Column<int>(type: "int", nullable: true),
                    AdminOverride = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminOverrideById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdminOverrideAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatencyMs = table.Column<int>(type: "int", nullable: true),
                    InputTokens = table.Column<int>(type: "int", nullable: true),
                    OutputTokens = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "DATEADD(day, 90, GETUTCDATE())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiInteractionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiInteractionLogs_AspNetUsers_AdminOverrideById",
                        column: x => x.AdminOverrideById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrustScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 100m),
                    CancellationRate = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    DisputeFrequencyScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PaymentFailureCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReviewAuthenticityScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RequiresAdditionalVerification = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalBookings = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TrustScoreLastComputedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssignedGovernorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAvailableForDispatch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalVerificationsCompleted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalTicketsHandled = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformSettings",
                columns: table => new
                {
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettings", x => x.SettingKey);
                    table.ForeignKey(
                        name: "FK_PlatformSettings_AspNetUsers_UpdatedByAdminId",
                        column: x => x.UpdatedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BusinessDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BaseLatitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    BaseLongitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    ServiceRadiusKm = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ServiceZoneCenter = table.Column<Point>(type: "geography", nullable: true),
                    ServiceZonePoly = table.Column<Geometry>(type: "geography", nullable: true),
                    OnboardingTier = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AverageRating = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    TotalReviewsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompletedBookingsCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ResponseRate = table.Column<decimal>(type: "decimal(5,4)", nullable: false, defaultValue: 0m),
                    SearchRankingModifier = table.Column<decimal>(type: "decimal(5,4)", nullable: false, defaultValue: 0m),
                    TrustScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    TrustLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TrustScoreLastComputedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchQueryLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RawQuery = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExtractedIntentJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextDomain = table.Column<int>(type: "int", nullable: false),
                    SearchMode = table.Column<int>(type: "int", nullable: false),
                    ResultCount = table.Column<int>(type: "int", nullable: false),
                    HasLowConfidence = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HasZeroResults = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AlternativeSuggested = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AlternativeAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingMs = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchQueryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchQueryLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPricingAggregates",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LocationGovernorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PriceType = table.Column<int>(type: "int", nullable: false),
                    MedianPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Percentile25 = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Percentile75 = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MinPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SampleCount = table.Column<int>(type: "int", nullable: false),
                    ComputedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPricingAggregates", x => new { x.CategoryId, x.LocationGovernorate, x.PriceType });
                    table.ForeignKey(
                        name: "FK_CategoryPricingAggregates_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    YearOfManufacture = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsNegotiable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TransactionMethod = table.Column<int>(type: "int", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AiRiskScore = table.Column<int>(type: "int", nullable: true),
                    AiRiskLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AiRiskFlags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QdrantPointId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastEmbeddedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmbeddingStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AdminRejectionNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketplaceListings_AspNetUsers_RejectedByAdminId",
                        column: x => x.RejectedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketplaceListings_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketplaceListings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTrustScoreHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrustScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TriggerEvent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTrustScoreHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTrustScoreHistory_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LicenseType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProfilePhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operators_ProviderProfiles_ProviderProfileId",
                        column: x => x.ProviderProfileId,
                        principalTable: "ProviderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderTrustScoreHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrustScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TrustLevel = table.Column<int>(type: "int", nullable: false),
                    ComponentRating = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ComponentCompletion = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ComponentResponse = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ComponentDocs = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ComponentIncident = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TriggerEvent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderTrustScoreHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderTrustScoreHistory_ProviderProfiles_ProviderProfileId",
                        column: x => x.ProviderProfileId,
                        principalTable: "ProviderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EquipmentModel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EquipmentCapacity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EquipmentCondition = table.Column<int>(type: "int", nullable: true),
                    YearOfManufacture = table.Column<int>(type: "int", nullable: true),
                    EquipmentRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DailyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MinimumBookingHours = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AiRiskScore = table.Column<int>(type: "int", nullable: true),
                    AiRiskLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AiRiskFlags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiRecommendation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QualityScore = table.Column<int>(type: "int", nullable: true),
                    QdrantPointId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastEmbeddedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmbeddingStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AdminRejectionNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmissionCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    QsDescription = table.Column<int>(type: "int", nullable: true),
                    QsPhotos = table.Column<int>(type: "int", nullable: true),
                    QsSpecs = table.Column<int>(type: "int", nullable: true),
                    QsPricing = table.Column<int>(type: "int", nullable: true),
                    QsOperator = table.Column<int>(type: "int", nullable: true),
                    QsDocs = table.Column<int>(type: "int", nullable: true),
                    QsZone = table.Column<int>(type: "int", nullable: true),
                    QualityScoreComputedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceListings_AspNetUsers_RejectedByAdminId",
                        column: x => x.RejectedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceListings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceListings_ProviderProfiles_ProviderProfileId",
                        column: x => x.ProviderProfileId,
                        principalTable: "ProviderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceListingPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceListingPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketplaceListingPhotos_MarketplaceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeliveryPreference = table.Column<int>(type: "int", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SellerConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DispatchedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedByBuyerAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationInitiatedByRole = table.Column<int>(type: "int", nullable: true),
                    ReturnShippingCost = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReturnShippingAcceptedByBuyerAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketplaceOrders_AspNetUsers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketplaceOrders_MarketplaceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlackoutDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackoutDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackoutDates_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlackoutDates_ServiceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedOperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    ServiceLocationGeo = table.Column<Point>(type: "geography", nullable: true),
                    SiteContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SiteContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccessRequirements = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SafetyNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RequestedStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedStartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EstimatedDurationHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ActualDurationHours = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HourlyRateSnapshot = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EstimatedTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SurchargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsOutOfZoneBooking = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OutOfZoneDistanceKm = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    OutOfZoneSurchargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    OutOfZoneSurchargeAcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrioritySurchargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    FuelSurchargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProviderRejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationInitiatedByRole = table.Column<int>(type: "int", nullable: true),
                    CancellationRefundPct = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ProviderCancellationPenaltyApplied = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReassignedToBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OriginalBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReassignedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentCapturedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedMarkedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisputeOpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FieldVerificationDispatchedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Bookings_OriginalBookingId",
                        column: x => x.OriginalBookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Bookings_ReassignedToBookingId",
                        column: x => x.ReassignedToBookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Operators_AssignedOperatorId",
                        column: x => x.AssignedOperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_ServiceListings_ServiceListingId",
                        column: x => x.ServiceListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExtractedText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    KeyFieldsPresent = table.Column<bool>(type: "bit", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryStatus = table.Column<int>(type: "int", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_MarketplaceListings_MarketplaceListingId",
                        column: x => x.MarketplaceListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_ServiceListings_ServiceListingId",
                        column: x => x.ServiceListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceListingAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListingAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceListingAvailability_ServiceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceListingOperators",
                columns: table => new
                {
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListingOperators", x => new { x.ListingId, x.OperatorId });
                    table.ForeignKey(
                        name: "FK_ServiceListingOperators_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceListingOperators_ServiceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceListingPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListingPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceListingPhotos_ServiceListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingTimeAdjustmentRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedAdditionalHrs = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    AdditionalCostAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProviderNote = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CustomerAcknowledgedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTimeAdjustmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingTimeAdjustmentRequests_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InitiatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LockedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_InitiatedById",
                        column: x => x.InitiatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_AspNetUsers_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_MarketplaceListings_MarketplaceListingId",
                        column: x => x.MarketplaceListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Conversations_ServiceListings_ServiceListingId",
                        column: x => x.ServiceListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCompletionEvidenceForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmittedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminReviewNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCompletionEvidenceForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCompletionEvidenceForms_AspNetUsers_ReviewedByAdminId",
                        column: x => x.ReviewedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCompletionEvidenceForms_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCompletionEvidenceForms_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OperatorAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatorAssignments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperatorAssignments_Operators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProviderIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IncidentType = table.Column<int>(type: "int", nullable: false),
                    PenaltyApplied = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AdminNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderIncidents_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderIncidents_ProviderProfiles_ProviderProfileId",
                        column: x => x.ProviderProfileId,
                        principalTable: "ProviderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ReviewedUserId",
                        column: x => x.ReviewedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_MarketplaceListings_MarketplaceListingId",
                        column: x => x.MarketplaceListingId,
                        principalTable: "MarketplaceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_MarketplaceOrders_MarketplaceOrderId",
                        column: x => x.MarketplaceOrderId,
                        principalTable: "MarketplaceOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_ServiceListings_ServiceListingId",
                        column: x => x.ServiceListingId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SubmittedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResolvedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AiSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiIdentifiedIssue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AiClaimedImpact = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AiEscalationPriority = table.Column<int>(type: "int", nullable: true),
                    ResolutionType = table.Column<int>(type: "int", nullable: true),
                    AdminResolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminResponseDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EscalatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EscalatedToAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstResponseAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReopenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_EscalatedToAdminId",
                        column: x => x.EscalatedToAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_ResolvedByUserId",
                        column: x => x.ResolvedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_SubmittedById",
                        column: x => x.SubmittedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_MarketplaceOrders_MarketplaceOrderId",
                        column: x => x.MarketplaceOrderId,
                        principalTable: "MarketplaceOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EscrowRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MarketplaceOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdjustmentRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GrossAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PlatformCommission = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CommissionRateSnapshot = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    ProviderPayout = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PartialSettleCustomerAmt = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PartialSettleProviderAmt = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentGatewayReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdditionalHoldDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EarliestReleaseAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CapturedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeldAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleasedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FrozenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FreezeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscrowRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscrowRecords_BookingTimeAdjustmentRequests_AdjustmentRequestId",
                        column: x => x.AdjustmentRequestId,
                        principalTable: "BookingTimeAdjustmentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EscrowRecords_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EscrowRecords_MarketplaceOrders_MarketplaceOrderId",
                        column: x => x.MarketplaceOrderId,
                        principalTable: "MarketplaceOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldVerificationForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchedEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DecidedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinkedEvidenceFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DispatchInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VisitStatus = table.Column<int>(type: "int", nullable: false),
                    EmployeeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldVerificationOutcome = table.Column<int>(type: "int", nullable: true),
                    AiSimilarityScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AiSimilarityNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdminDecision = table.Column<int>(type: "int", nullable: false),
                    AdminDecisionNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DispatchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormSubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldVerificationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldVerificationForms_AspNetUsers_DecidedByAdminId",
                        column: x => x.DecidedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldVerificationForms_AspNetUsers_DispatchedByAdminId",
                        column: x => x.DispatchedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldVerificationForms_AspNetUsers_DispatchedEmployeeId",
                        column: x => x.DispatchedEmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldVerificationForms_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldVerificationForms_JobCompletionEvidenceForms_LinkedEvidenceFormId",
                        column: x => x.LinkedEvidenceFormId,
                        principalTable: "JobCompletionEvidenceForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCompletionEvidencePhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCompletionEvidencePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCompletionEvidencePhotos_JobCompletionEvidenceForms_FormId",
                        column: x => x.FormId,
                        principalTable: "JobCompletionEvidenceForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationReadReceipts",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastReadMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastReadAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationReadReceipts", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ConversationReadReceipts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationReadReceipts_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationReadReceipts_Messages_LastReadMessageId",
                        column: x => x.LastReadMessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FieldVerificationPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldVerificationFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldVerificationPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldVerificationPhotos_FieldVerificationForms_FieldVerificationFormId",
                        column: x => x.FieldVerificationFormId,
                        principalTable: "FieldVerificationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_TicketMessages_TicketMessageId",
                        column: x => x.TicketMessageId,
                        principalTable: "TicketMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AiInteractionLogs_AdminOverrideById",
                table: "AiInteractionLogs",
                column: "AdminOverrideById");

            migrationBuilder.CreateIndex(
                name: "IX_AiInteractionLogs_AgentType_CreatedAt",
                table: "AiInteractionLogs",
                columns: new[] { "AgentType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AiInteractionLogs_EntityType_EntityId",
                table: "AiInteractionLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_ListingId_Date",
                table: "BlackoutDates",
                columns: new[] { "ListingId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_BlackoutDates_OperatorId",
                table: "BlackoutDates",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AssignedOperatorId",
                table: "Bookings",
                column: "AssignedOperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_OriginalBookingId",
                table: "Bookings",
                column: "OriginalBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ReassignedToBookingId",
                table: "Bookings",
                column: "ReassignedToBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceListingId_Status",
                table: "Bookings",
                columns: new[] { "ServiceListingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingTimeAdjustmentRequests_BookingId_Status",
                table: "BookingTimeAdjustmentRequests",
                columns: new[] { "BookingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationReadReceipts_LastReadMessageId",
                table: "ConversationReadReceipts",
                column: "LastReadMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationReadReceipts_UserId",
                table: "ConversationReadReceipts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_BookingId",
                table: "Conversations",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_InitiatedById",
                table: "Conversations",
                column: "InitiatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_MarketplaceListingId",
                table: "Conversations",
                column: "MarketplaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ParticipantId",
                table: "Conversations",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ServiceListingId",
                table: "Conversations",
                column: "ServiceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfiles_UserId",
                table: "CustomerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTrustScoreHistory_CustomerProfileId_RecordedAt",
                table: "CustomerTrustScoreHistory",
                columns: new[] { "CustomerProfileId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MarketplaceListingId",
                table: "Documents",
                column: "MarketplaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_OperatorId",
                table: "Documents",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ServiceListingId",
                table: "Documents",
                column: "ServiceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEventQueue_Status_CreatedAt",
                table: "DomainEventQueue",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_EmployeeCode",
                table: "EmployeeProfiles",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_IsAvailableForDispatch_AssignedGovernorate",
                table: "EmployeeProfiles",
                columns: new[] { "IsAvailableForDispatch", "AssignedGovernorate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfiles_UserId",
                table: "EmployeeProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EscrowRecords_AdjustmentRequestId",
                table: "EscrowRecords",
                column: "AdjustmentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowRecords_BookingId",
                table: "EscrowRecords",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EscrowRecords_MarketplaceOrderId",
                table: "EscrowRecords",
                column: "MarketplaceOrderId",
                unique: true,
                filter: "[MarketplaceOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationForms_BookingId",
                table: "FieldVerificationForms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationForms_DecidedByAdminId",
                table: "FieldVerificationForms",
                column: "DecidedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationForms_DispatchedByAdminId",
                table: "FieldVerificationForms",
                column: "DispatchedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationForms_DispatchedEmployeeId",
                table: "FieldVerificationForms",
                column: "DispatchedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationForms_LinkedEvidenceFormId",
                table: "FieldVerificationForms",
                column: "LinkedEvidenceFormId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldVerificationPhotos_FieldVerificationFormId_DisplayOrder",
                table: "FieldVerificationPhotos",
                columns: new[] { "FieldVerificationFormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_JobCompletionEvidenceForms_BookingId",
                table: "JobCompletionEvidenceForms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCompletionEvidenceForms_ReviewedByAdminId",
                table: "JobCompletionEvidenceForms",
                column: "ReviewedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCompletionEvidenceForms_SubmittedByUserId",
                table: "JobCompletionEvidenceForms",
                column: "SubmittedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCompletionEvidencePhotos_FormId_DisplayOrder",
                table: "JobCompletionEvidencePhotos",
                columns: new[] { "FormId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListingPhotos_ListingId_DisplayOrder",
                table: "MarketplaceListingPhotos",
                columns: new[] { "ListingId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListings_CategoryId_Status",
                table: "MarketplaceListings",
                columns: new[] { "CategoryId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListings_RejectedByAdminId",
                table: "MarketplaceListings",
                column: "RejectedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceListings_SellerId_Status",
                table: "MarketplaceListings",
                columns: new[] { "SellerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceOrders_BuyerId_Status",
                table: "MarketplaceOrders",
                columns: new[] { "BuyerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceOrders_ListingId_Status",
                table: "MarketplaceOrders",
                columns: new[] { "ListingId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId_SentAt",
                table: "Messages",
                columns: new[] { "ConversationId", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_SentAt",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OperatorAssignments_BookingId",
                table: "OperatorAssignments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorAssignments_OperatorId_ScheduledStart_ScheduledEnd",
                table: "OperatorAssignments",
                columns: new[] { "OperatorId", "ScheduledStart", "ScheduledEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_Operators_ProviderProfileId_IsActive",
                table: "Operators",
                columns: new[] { "ProviderProfileId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSettings_UpdatedByAdminId",
                table: "PlatformSettings",
                column: "UpdatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderIncidents_BookingId",
                table: "ProviderIncidents",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderIncidents_ProviderProfileId_OccurredAt",
                table: "ProviderIncidents",
                columns: new[] { "ProviderProfileId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_ResponseRate_AverageRating_OnboardingTier",
                table: "ProviderProfiles",
                columns: new[] { "ResponseRate", "AverageRating", "OnboardingTier" });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderProfiles_UserId",
                table: "ProviderProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderTrustScoreHistory_ProviderProfileId_RecordedAt",
                table: "ProviderTrustScoreHistory",
                columns: new[] { "ProviderProfileId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingId",
                table: "Reviews",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MarketplaceListingId",
                table: "Reviews",
                column: "MarketplaceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MarketplaceOrderId",
                table: "Reviews",
                column: "MarketplaceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewedUserId",
                table: "Reviews",
                column: "ReviewedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ServiceListingId",
                table: "Reviews",
                column: "ServiceListingId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchQueryLogs_ContextDomain_SearchMode_CreatedAt",
                table: "SearchQueryLogs",
                columns: new[] { "ContextDomain", "SearchMode", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SearchQueryLogs_UserId",
                table: "SearchQueryLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingAvailability_ListingId_DayOfWeek",
                table: "ServiceListingAvailability",
                columns: new[] { "ListingId", "DayOfWeek" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingOperators_ListingId",
                table: "ServiceListingOperators",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingOperators_OperatorId",
                table: "ServiceListingOperators",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingPhotos_ListingId_DisplayOrder",
                table: "ServiceListingPhotos",
                columns: new[] { "ListingId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListings_CategoryId_Status",
                table: "ServiceListings",
                columns: new[] { "CategoryId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListings_ProviderProfileId_Status",
                table: "ServiceListings",
                columns: new[] { "ProviderProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListings_RejectedByAdminId",
                table: "ServiceListings",
                column: "RejectedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketMessageId",
                table: "TicketAttachments",
                column: "TicketMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_UploadedByUserId",
                table: "TicketAttachments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SenderId",
                table: "TicketMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TicketId_CreatedAt",
                table: "TicketMessages",
                columns: new[] { "TicketId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToUserId",
                table: "Tickets",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EscalatedToAdminId",
                table: "Tickets",
                column: "EscalatedToAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_MarketplaceOrderId",
                table: "Tickets",
                column: "MarketplaceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ResolvedByUserId",
                table: "Tickets",
                column: "ResolvedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status_Priority",
                table: "Tickets",
                columns: new[] { "Status", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubmittedById",
                table: "Tickets",
                column: "SubmittedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketNumber",
                table: "Tickets",
                column: "TicketNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AiInteractionLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlackoutDates");

            migrationBuilder.DropTable(
                name: "CategoryPricingAggregates");

            migrationBuilder.DropTable(
                name: "ConversationReadReceipts");

            migrationBuilder.DropTable(
                name: "CustomerTrustScoreHistory");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "DomainEventQueue");

            migrationBuilder.DropTable(
                name: "EmployeeProfiles");

            migrationBuilder.DropTable(
                name: "EscrowRecords");

            migrationBuilder.DropTable(
                name: "FieldVerificationPhotos");

            migrationBuilder.DropTable(
                name: "JobCompletionEvidencePhotos");

            migrationBuilder.DropTable(
                name: "MarketplaceListingPhotos");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OperatorAssignments");

            migrationBuilder.DropTable(
                name: "PlatformSettings");

            migrationBuilder.DropTable(
                name: "ProviderIncidents");

            migrationBuilder.DropTable(
                name: "ProviderTrustScoreHistory");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SearchQueryLogs");

            migrationBuilder.DropTable(
                name: "ServiceListingAvailability");

            migrationBuilder.DropTable(
                name: "ServiceListingOperators");

            migrationBuilder.DropTable(
                name: "ServiceListingPhotos");

            migrationBuilder.DropTable(
                name: "TicketAttachments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "CustomerProfiles");

            migrationBuilder.DropTable(
                name: "BookingTimeAdjustmentRequests");

            migrationBuilder.DropTable(
                name: "FieldVerificationForms");

            migrationBuilder.DropTable(
                name: "TicketMessages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "JobCompletionEvidenceForms");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "MarketplaceOrders");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "ServiceListings");

            migrationBuilder.DropTable(
                name: "MarketplaceListings");

            migrationBuilder.DropTable(
                name: "ProviderProfiles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
