using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.DeleteAddress
{
    public class DeleteAddressCommandHandler(IApplicationDbContext context)
        : IRequestHandler<DeleteAddressCommand, DeleteAddressDTO>
    {
        public async Task<DeleteAddressDTO> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await context.Addresses
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (address == null)
            {
                return new DeleteAddressDTO { IsSuccess = false, StatusCode = 404, Message = "Address not found." };
            }

            if (address.UserId != request.UserId)
            {
                return new DeleteAddressDTO { IsSuccess = false, StatusCode = 403, Message = "You are not authorized to delete this address." };
            }

            string notice = null;
            bool wasDefault = address.IsDefault;

            context.Addresses.Remove(address);

            if (wasDefault)
            {
                var nextAddress = await context.Addresses
                    .Where(a => a.UserId == request.UserId && a.Id != request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (nextAddress != null)
                {
                    nextAddress.IsDefault = true;
                    notice = $"Default address was deleted. '{nextAddress.Label}' is now set as default.";
                }
            }

            await context.SaveChangesAsync(cancellationToken);

            return new DeleteAddressDTO
            {
                IsSuccess = true,
                StatusCode = 200, 
                Notice = notice
            };
        }
    }

}
