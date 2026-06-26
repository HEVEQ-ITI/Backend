using System;

namespace HEVEQ.Application.Common.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException()
        : base("A bad request occurred.")
    {
    }

    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}