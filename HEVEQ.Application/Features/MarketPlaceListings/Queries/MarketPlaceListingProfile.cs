using AutoMapper;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries
{
    public class MarketPlaceListingProfile:Profile
    {
        public MarketPlaceListingProfile()
        {
        CreateMap<MarketplaceListingPhoto, MarketplaceListingPhotoDto>();

            CreateMap<MarketplaceListing, MarketPlaceListingDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}" ?? src.Seller.UserName))
                .ForMember(dest => dest.CoverPhotoUrl, opt => opt.MapFrom(src =>
                      src.Photos.OrderBy(p => p.DisplayOrder).Select(p => p.PhotoUrl).FirstOrDefault()));

            CreateMap<MarketplaceListing, MarketplaceListingDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Seller.FirstName)
                    ? $"{src.Seller.FirstName} {src.Seller.LastName}"
                    : src.Seller.UserName));
        }

    }
}
