using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Addresses.DTOs;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.AddAddress
{
    public class CreateAddressesCommandHandler(IApplicationDbContext context)
        : IRequestHandler<CreateAddressesCommand, CreateAddressDTO>
    {
        public async Task<CreateAddressDTO> Handle(CreateAddressesCommand request, CancellationToken cancellationToken)
        {
            var hasAnyAddress = await context.Addresses
                .AnyAsync(a => a.UserId == request.UserId, cancellationToken);

            if (!hasAnyAddress)
            {
                request.IsDefault = true;
            }
            else if (request.IsDefault)
            {
                var existingDefaultAddresses = await context.Addresses
                    .Where(a => a.UserId == request.UserId && a.IsDefault)
                    .ToListAsync(cancellationToken);

                foreach (var address in existingDefaultAddresses)
                {
                    address.IsDefault = false;
                }
            }

            var newAddress = new Address
            {
                UserId = request.UserId,
                Label = request.Label,
                Governorate = request.Governorate,
                District = request.District,
                Street = request.Street,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsDefault = request.IsDefault
            };

            context.Addresses.Add(newAddress);
            await context.SaveChangesAsync(cancellationToken);

            // 4. إرجاع النتيجة
            return new CreateAddressDTO
            {
                Id = newAddress.Id,
                Label = newAddress.Label,
                IsDefault = newAddress.IsDefault
            };
        }
    }
}
