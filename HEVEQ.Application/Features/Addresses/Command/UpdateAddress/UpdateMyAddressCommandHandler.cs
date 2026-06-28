using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.Addresses.Command.UpdateAddress
{
    public class UpdateMyAddressCommandHandler(IApplicationDbContext context)
        : IRequestHandler<UpdateMyAddressCommand, UpdateAddressDTO>
    {
        public async Task<UpdateAddressDTO> Handle(UpdateMyAddressCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق رياضياً من الإحداثيات (Business Rule)
            var address = await context.Addresses
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (address == null)
            {
                return new UpdateAddressDTO { IsSuccess = false, StatusCode = 404, Message = "Address not found." };
            }

            if (address.UserId != request.UserId)
            {
                return new UpdateAddressDTO { IsSuccess = false, StatusCode = 403, Message = "You are not authorized to update this address." };
            }

            if (request.IsDefault && !address.IsDefault)
            {
                var existingDefaultAddresses = await context.Addresses
                    .Where(a => a.UserId == request.UserId && a.IsDefault && a.Id != request.Id)
                    .ToListAsync(cancellationToken);

                foreach (var defAddress in existingDefaultAddresses)
                {
                    defAddress.IsDefault = false;
                }
            }

            address.Label = request.Label ?? address.Label;
            address.Governorate = request.Governorate ?? address.Governorate;
            address.District = request.District ?? address.District;
            address.Street = request.Street ?? address.Street;
            address.IsDefault = request.IsDefault;

            if (request.Latitude.HasValue) address.Latitude = request.Latitude.Value;
            if (request.Longitude.HasValue) address.Longitude = request.Longitude.Value;

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateAddressDTO { IsSuccess = true, StatusCode = 204 };
        }
    }
}
