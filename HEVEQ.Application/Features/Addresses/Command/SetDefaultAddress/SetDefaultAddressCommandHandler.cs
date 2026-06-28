using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Addresses.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.SetDefaultAddress
{
    public class SetDefaultAddressCommandHandler(IApplicationDbContext context)
        : IRequestHandler<SetDefaultAddressCommand, SetDefaultAddressDTO>
    {
        public async Task<SetDefaultAddressDTO> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            var targetAddress = await context.Addresses
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (targetAddress == null)
            {
                return new SetDefaultAddressDTO { IsSuccess = false, StatusCode = 404, Message = "Address not found." };
            }

            if (targetAddress.UserId != request.UserId)
            {
                return new SetDefaultAddressDTO { IsSuccess = false, StatusCode = 403, Message = "Unauthorized access to this address." };
            }

            if (targetAddress.IsDefault)
            {
                return new SetDefaultAddressDTO { IsSuccess = true, StatusCode = 200, Message = "Default address updated successfully" };
            }

            var previousDefaultAddresses = await context.Addresses
                .Where(a => a.UserId == request.UserId && a.IsDefault && a.Id != request.Id)
                .ToListAsync(cancellationToken);

            foreach (var oldDefault in previousDefaultAddresses)
            {
                oldDefault.IsDefault = false;
            }

            targetAddress.IsDefault = true;

            await context.SaveChangesAsync(cancellationToken);

            return new SetDefaultAddressDTO
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Default address updated successfully"
            };
        }
    }
}
