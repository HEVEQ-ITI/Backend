namespace HEVEQ.Api.Requests.Bookings
{
    public sealed record CreateBookingRequest(
    Guid ServiceListingId,
    string JobTitle,
    string? JobDescription,
    Guid? AddressId,
    string? Governorate,
    string? District,
    string? Street,
    decimal? Latitude,
    decimal? Longitude,
    DateOnly RequestedStartDate,
    TimeOnly RequestedStartTime,
    decimal EstimatedDurationHours,
    string? SiteContactName,
    string? SiteContactPhone,
    string? AccessRequirements,
    string? SafetyNotes,
    bool AcceptOutOfZoneSurcharge
    );
}
