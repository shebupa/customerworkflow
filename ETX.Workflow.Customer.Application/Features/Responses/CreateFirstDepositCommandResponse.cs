using ETX.Workflow.Customer.Domain;

namespace ETX.Workflow.Customer.Application.Features.Responses;

[ExcludeFromCodeCoverage]
public class CreateFirstDepositCommandResponse :
    ResponseBase
{
    public string EventName { get; set; } = Enumerations.GetEnumDescription(WorkflowEventType.FirstDeposit);
    public FirstDepositEvent EventValue { get; set; }
}

public class FirstDepositEvent
{
    public string Currency { get; set; }
    public float Amount { get; set; }
}