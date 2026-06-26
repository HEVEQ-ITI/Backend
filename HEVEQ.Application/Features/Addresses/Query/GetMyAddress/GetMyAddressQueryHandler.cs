using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Addresses.DTOs;
using HEVEQ.Application.Features.CustomerProfiles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Query.GetMyAddress
{
    public class GetMyAddressQueryHandler(IApplicationDbContext context) :
        IRequestHandler<GetMyAddressQuery, List<AddressDTO>>
    {
        public async Task<List<AddressDTO>> Handle(GetMyAddressQuery request, CancellationToken cancellationToken)
        {
            var addresses = await context.Addresses
                .AsNoTracking() // للقراءة فقط، تسرع الأداء جداً
                .Where(a => a.UserId == request.UserId)
                .OrderByDescending(a => a.IsDefault) // العنوان الافتراضي يظهر أولاً
                .Select(a => new AddressDTO
                {
                    Id = a.Id,
                    Label = a.Label,
                    Governorate = a.Governorate,
                    District = a.District,
                    Street = a.Street,
                    Latitude = a.Latitude, 
                    Longitude = a.Longitude,
                    IsDefault = a.IsDefault
                })
                .ToListAsync(cancellationToken);
            return addresses;
        }
    }
}
