using MediatR;

namespace HEVEQ.Application.Features.Operators.Commands.DeleteOperator;

public record DeleteOperatorCommand(Guid Id) : IRequest;