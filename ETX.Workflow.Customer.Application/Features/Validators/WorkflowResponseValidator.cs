namespace ETX.Workflow.Customer.Application.Features.Validators;

public class WorkflowResponseValidator : IWorkflowResponseValidator
{
    private readonly IMetricsLoggerService _metricslogger = default;

    public WorkflowResponseValidator(IMetricsLoggerService metricslogger)
    {
        _metricslogger = metricslogger;
    }

    public bool IsValidResponseToSend(
        CreateFirstDepositCommandResponse createFirstDepositCommandResponse)
    {
        bool validResponse = true;
        if (!ValidateBasic(createFirstDepositCommandResponse) ||
            (string.IsNullOrWhiteSpace(createFirstDepositCommandResponse.EventValue.Currency)) ||
            (createFirstDepositCommandResponse.EventValue.Amount == 0))
        {
            _metricslogger.LogWarnMessage($"Event=\"Publishing First Deposit Failed, mandatory data missing\" " +
                                          $"TrackingId={createFirstDepositCommandResponse?.CustomerWorkflowId} " +
                                          $"CustomerId={createFirstDepositCommandResponse?.CustomerId} " +
                                          $"Currency=\"{createFirstDepositCommandResponse?.EventValue.Currency}\" " +
                                          $"Amount={createFirstDepositCommandResponse?.EventValue.Amount}");

            validResponse = false;
        }
        return validResponse;
    }

    public bool IsValidResponseToSend(
        CreateRegistrationStatusCommandResponse createRegistrationStatusCommandResponse)
    {
        bool validResponse = true;
        if (!ValidateBasic(createRegistrationStatusCommandResponse) ||
            (createRegistrationStatusCommandResponse.EventValue.StatusId == 0) ||
            (string.IsNullOrWhiteSpace(createRegistrationStatusCommandResponse.EventValue.StatusDescription)))
        {
            _metricslogger.LogWarnMessage($"Event=\"Publishing First Deposit Failed, mandatory data missing\" " +
                                          $"TrackingId={createRegistrationStatusCommandResponse?.CustomerWorkflowId} " +
                                          $"CustomerId={createRegistrationStatusCommandResponse?.CustomerId} " +
                                          $"Currency=\"{createRegistrationStatusCommandResponse?.EventValue.StatusId}\" " +
                                          $"Amount={createRegistrationStatusCommandResponse?.EventValue.StatusDescription}");

            validResponse = false;
        }
        return validResponse;
    }

    public bool IsValidResponseToSend(
        CreateFirstTradeCommandResponse createFirstTradeCommandResponse)
    {
        bool validResponse = true;
        if (!ValidateBasic(createFirstTradeCommandResponse) ||
            (string.IsNullOrWhiteSpace(createFirstTradeCommandResponse.EventValue.Currency)))
        {
            _metricslogger.LogWarnMessage($"Event=\"Publishing First Deposit Failed, mandatory data missing\" " +
                                          $"TrackingId={createFirstTradeCommandResponse?.CustomerWorkflowId} " +
                                          $"CustomerId={createFirstTradeCommandResponse?.CustomerId} " +
                                          $"Currency=\"{createFirstTradeCommandResponse?.EventValue.ClientId}\" " +
                                          $"Amount={createFirstTradeCommandResponse?.EventValue.Amount}" +
                                          $"Currency={createFirstTradeCommandResponse?.EventValue.Currency}" +
                                          $"TradeDate={createFirstTradeCommandResponse?.EventValue.TradeDate}" +
                                          $"MarketName={createFirstTradeCommandResponse?.EventValue.MarketName}");

            validResponse = false;
        }
        return validResponse;
    }

    private bool ValidateBasic<RType>(RType response)
        where RType : ResponseBase
    {
        var validResponse = true;
        if ((response == null) ||
           (string.IsNullOrWhiteSpace(response?.CustomerId)) ||
           (string.IsNullOrWhiteSpace(response?.CustomerWorkflowId))
           )
        {
            validResponse = false;
        }
        return validResponse;
    }
}