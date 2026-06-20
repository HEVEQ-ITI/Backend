namespace HEVEQ.Domain.Enums;

public enum TicketResolutionType
{
    FullRefundToCustomer = 0,
    FullReleaseToProvider = 1,
    PartialSettlement = 2,
    ServiceRedo = 3,
    NoActionRequired = 4,
    EscalatedToFieldVerification = 5
}
