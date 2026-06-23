using HEVEQ.Application.Features.Bookings.Commands.CreateBooking;
using HEVEQ.Application.Features.Bookings.Services.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Services.Implementation
{
    public class BookingCreationService : IBookingCreationService
    {
        private const decimal OutOfZoneSurchargePerKm = 25m;
        public Booking Create(CreateBookingCommand request, ServiceListing listing, BookingAddressSnapshot addressSnapshot)
        {
            ValidateListing(listing, request);
            ValidateAvailability(listing, request);

            var zoneResult = CalculateZone(
                listing.ProviderProfile,
                addressSnapshot.Latitude,
                addressSnapshot.Longitude,
                request.AcceptOutOfZoneSurcharge);

            var hourlyRate = listing.HourlyRate!.Value;
            var surchargeAmount = zoneResult.OutOfZoneSurchargeAmount ?? 0m;
            var estimatedTotal = (hourlyRate * request.EstimatedDurationHours) + surchargeAmount;

            return new Booking
            {
                CustomerId = request.CustomerId,
                ServiceListingId = request.ServiceListingId,
                JobTitle = request.JobTitle,
                JobDescription = request.JobDescription,
                Governorate = addressSnapshot.Governorate,
                District = addressSnapshot.District,
                Street = addressSnapshot.Street,
                Latitude = addressSnapshot.Latitude,
                Longitude = addressSnapshot.Longitude,
                ServiceLocationGeo = zoneResult.ServicePoint,
                RequestedStartDate = request.RequestedStartDate,
                RequestedStartTime = request.RequestedStartTime,
                EstimatedDurationHours = request.EstimatedDurationHours,
                HourlyRateSnapshot = hourlyRate,
                EstimatedTotal = estimatedTotal,
                SurchargeAmount = surchargeAmount,
                IsOutOfZoneBooking = zoneResult.IsOutOfZone,
                OutOfZoneDistanceKm = zoneResult.OutOfZoneDistanceKm,
                OutOfZoneSurchargeAmount = zoneResult.OutOfZoneSurchargeAmount,
                OutOfZoneSurchargeAcceptedAt = zoneResult.IsOutOfZone ? DateTime.UtcNow : null,
                ProviderCancellationPenaltyApplied = false,
                SiteContactName = request.SiteContactName,
                SiteContactPhone = request.SiteContactPhone,
                AccessRequirements = request.AccessRequirements,
                SafetyNotes = request.SafetyNotes,
                Status = BookingStatus.PendingProviderResponse,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static void ValidateListing(ServiceListing listing, CreateBookingCommand request)
        {
            if (listing.Status != ServiceListingStatus.Active)
                throw new InvalidOperationException("Booking is allowed only for active service listings.");

            if (listing.HourlyRate is null || listing.HourlyRate <= 0)
                throw new InvalidOperationException("Service listing hourly rate is not configured.");

            if (request.EstimatedDurationHours < listing.MinimumBookingHours)
                throw new InvalidOperationException($"Minimum booking duration is {listing.MinimumBookingHours} hour(s).");

            if (listing.ProviderProfile is null)
                throw new InvalidOperationException("Provider profile is not configured.");
        }

        private static void ValidateAvailability(ServiceListing listing, CreateBookingCommand request)
        {
            var isBlackoutDate = listing.BlackoutDates.Any(x =>
                x.Date == request.RequestedStartDate &&
                x.OperatorId == null);

            if (isBlackoutDate)
                throw new InvalidOperationException("Booking is not allowed on a blackout date.");

            var dayOfWeek = (int)request.RequestedStartDate.DayOfWeek;

            var availability = listing.Availability.FirstOrDefault(x =>
                x.DayOfWeek == dayOfWeek);

            if (availability is null)
                throw new InvalidOperationException("No availability exists for the selected day.");

            if (!IsInsideAvailability(
                    request.RequestedStartTime,
                    request.EstimatedDurationHours,
                    availability.OpenTime,
                    availability.CloseTime))
            {
                throw new InvalidOperationException("Booking time is outside service listing availability.");
            }
        }

        private static ZoneResult CalculateZone(
            ProviderProfile providerProfile,
            decimal latitude,
            decimal longitude,
            bool acceptOutOfZoneSurcharge)
        {
            if (providerProfile.BaseLatitude is null ||
                providerProfile.BaseLongitude is null ||
                providerProfile.ServiceRadiusKm <= 0)
            {
                throw new InvalidOperationException("Provider service zone is not configured.");
            }

            var servicePoint = new Point((double)longitude, (double)latitude)
            {
                SRID = 4326
            };

            var distanceFromBaseKm = CalculateDistanceKm(
                (double)providerProfile.BaseLatitude.Value,
                (double)providerProfile.BaseLongitude.Value,
                (double)latitude,
                (double)longitude);

            var insideRadius = distanceFromBaseKm <= providerProfile.ServiceRadiusKm;

            var insidePolygon = providerProfile.ServiceZonePoly is null ||
                                providerProfile.ServiceZonePoly.Covers(servicePoint);

            var isOutOfZone = !insideRadius || !insidePolygon;

            var outOfZoneDistanceKm = isOutOfZone
                ? Math.Round((decimal)Math.Max(0, distanceFromBaseKm - providerProfile.ServiceRadiusKm), 2)
                : (decimal?)null;

            var outOfZoneSurchargeAmount = isOutOfZone
                ? Math.Round((outOfZoneDistanceKm ?? 0m) * OutOfZoneSurchargePerKm, 2)
                : (decimal?)null;

            if (isOutOfZone && !acceptOutOfZoneSurcharge)
            {
                throw new InvalidOperationException("Booking location is outside provider service zone. Customer must accept out-of-zone surcharge.");
            }

            return new ZoneResult
            {
                ServicePoint = servicePoint,
                IsOutOfZone = isOutOfZone,
                OutOfZoneDistanceKm = outOfZoneDistanceKm,
                OutOfZoneSurchargeAmount = outOfZoneSurchargeAmount
            };
        }

        private static bool IsInsideAvailability(TimeOnly start, decimal durationHours, TimeOnly open, TimeOnly close)
        {
            var startMinutes = start.Hour * 60 + start.Minute;
            var endMinutes = startMinutes + (int)Math.Ceiling(durationHours * 60m);
            var openMinutes = open.Hour * 60 + open.Minute;
            var closeMinutes = close.Hour * 60 + close.Minute;

            return startMinutes >= openMinutes && endMinutes <= closeMinutes;
        }

        private static double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadiusKm = 6371.0088;

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var radLat1 = ToRadians(lat1);
            var radLat2 = ToRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(radLat1) * Math.Cos(radLat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadiusKm * c;
        }

        private static double ToRadians(double value)
        {
            return value * Math.PI / 180;
        }

        private class ZoneResult
        {
            public Point ServicePoint { get; set; } = default!;

            public bool IsOutOfZone { get; set; }

            public decimal? OutOfZoneDistanceKm { get; set; }

            public decimal? OutOfZoneSurchargeAmount { get; set; }
        }
    }
}
