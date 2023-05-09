namespace ETX.Workflow.Customer.Application.Features.Validators;

public interface IWorkflowResponseValidator
{
    bool IsValidResponseToSend(
        CreateFirstDepositCommandResponse createFirstDepositCommandResponse);

    bool IsValidResponseToSend(
       CreateRegistrationStatusCommandResponse createRegistrationStatusCommandResponse);

    bool IsValidResponseToSend(
       CreateFirstTradeCommandResponse createFirstTradeCommandResponse);
}