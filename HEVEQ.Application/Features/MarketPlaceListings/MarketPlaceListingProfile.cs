using AutoMapper;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings
{
    public class MarketPlaceListingProfile:Profile
    {
        public MarketPlaceListingProfile()
        {
        
            CreateMap<MarketplaceListingPhoto, MarketplaceListingPhotoDto>();

            CreateMap<MarketplaceListing, ProviderMarketPlaceListingDTO>()
                 .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                 .ForMember(d => d.StatusAr, opt => opt.MapFrom(s => s.Status.ToArabic()))
                 .ForMember(d => d.PhotosCount, opt => opt.MapFrom(s => s.Photos.Count));


            CreateMap<MarketplaceListing, MarketPlaceListingDTO>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src =>
                            src.Seller.ProviderProfile != null &&
                            !string.IsNullOrEmpty(src.Seller.ProviderProfile.CompanyName)
                                ? src.Seller.ProviderProfile.CompanyName
                                : !string.IsNullOrEmpty(src.Seller.FirstName)
                                    ? $"{src.Seller.FirstName} {src.Seller.LastName}"
                                    : src.Seller.UserName))
                    .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                            src.Seller.ProviderProfile != null
                                ? src.Seller.ProviderProfile.AverageRating
                                : (decimal?)null))
                     .ForMember(dest => dest.TotalReviewsCount, opt => opt.MapFrom(src =>
                                src.Seller.ProviderProfile != null
                                    ? src.Seller.ProviderProfile.TotalReviewsCount
                                    : (int?)null))
                    .ForMember(dest => dest.Location, opt  => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.District) ? src.Governorate : $"{src.District}, {src.Governorate}"))
                    .ForMember(dest => dest.CoverPhotoUrl, opt => opt.MapFrom(src =>
                        src.Photos.OrderBy(p => p.DisplayOrder).Select(p => p.PhotoUrl).FirstOrDefault()))
                    .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString()))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.TransactionMethod, opt => opt.MapFrom(src => src.TransactionMethod.ToString()))
                    .ForMember(dest => dest.StatusAr, opt => opt.MapFrom(src => src.Status.ToArabic()))
                    .ForMember(dest => dest.ConditionAr, opt => opt.MapFrom(src => src.Condition.ToArabic()));


            CreateMap<ApplicationUser, ListingSellerDto>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(d => d.DisplayName, opt => opt.MapFrom(s =>
                     s.ProviderProfile != null && !string.IsNullOrEmpty(s.ProviderProfile.CompanyName)
                         ? s.ProviderProfile.CompanyName
                         : !string.IsNullOrEmpty(s.FirstName) ? $"{s.FirstName} {s.LastName}" : s.UserName))
                 .ForMember(d => d.AverageRating, opt => opt.MapFrom(s =>
                     s.ProviderProfile != null ? s.ProviderProfile.AverageRating : (decimal?)null))
                 .ForMember(d => d.TotalReviewsCount, opt => opt.MapFrom(s =>
                     s.ProviderProfile != null ? s.ProviderProfile.TotalReviewsCount : (int?)null));

            CreateMap<MarketplaceListing, ListingManagementInfoDto>();

            CreateMap<MarketplaceListing, MarketplaceListingDetailsDto>()
            .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.ToString()))
            .ForMember(dest => dest.ConditionAr, opt => opt.MapFrom(src => src.Condition.ToArabic()))
            .ForMember(dest => dest.CanBuyNow, opt => opt.Ignore())
            .ForMember(dest => dest.ManagementInfo, opt => opt.Ignore());
        }

    }
}
