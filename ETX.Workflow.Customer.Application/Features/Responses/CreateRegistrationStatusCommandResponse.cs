using ETX.Workflow.Customer.Domain;

namespace ETX.Workflow.Customer.Application.Features.Responses;

[ExcludeFromCodeCoverage]
public class CreateRegistrationStatusCommandResponse
    : ResponseBase
{
    public string EventName
    {
        get
        {
            return Enumerations.GetEnumDescription((WorkflowEventType)EventValue.StatusId);
        }
    }

    public RegistrationStatusEvent EventValue { get; set; }
}

public class RegistrationStatusEvent
{
    public int StatusId { get; set; }
    public string StatusDescription { get; set; }
}