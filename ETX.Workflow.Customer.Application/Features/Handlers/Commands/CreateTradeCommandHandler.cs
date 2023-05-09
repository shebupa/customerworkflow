namespace ETX.Workflow.Customer.Application.Features.Handlers.Commands;

public class CreateTradeCommandHandler
     : IRequestHandler<CreateFirstTradeCommandRequest, CreateFirstTradeCommandResponse>
{
    private readonly IMapper _mapper = default;
    private readonly IMetricsLoggerService _metricslogger = default;
    private readonly IPublisherService _publisherService = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IWorkflowResponseValidator _workflowResponseValidator = default;

    public CreateTradeCommandHandler(
        EventBusSettings eventBusSettings
        , IMapper mapper
        , IMetricsLoggerService metricslogger
        , IPublisherService publisherService
        , IWorkflowResponseValidator workflowResponseValidator)
    {
        _eventBusSettings = eventBusSettings;
        _mapper = mapper;
        _metricslogger = metricslogger;
        _publisherService = publisherService;
        _workflowResponseValidator = workflowResponseValidator;
    }

    public async Task<CreateFirstTradeCommandResponse> Handle(
        CreateFirstTradeCommandRequest request
        , CancellationToken cancellationToken)
    {
        CreateFirstTradeCommandResponse createFirstTradeCommandResponse = default;
        try
        {
            var registrationStatusEvent = _mapper.Map<Events.WorkflowEvents>(request);
            var workflow = new ETX.Workflow.Customer.Domain.Workflow(registrationStatusEvent);

            createFirstTradeCommandResponse = _mapper.Map<CreateFirstTradeCommandResponse>(
                                                      workflow.Trade);

            await _publisherService.ConnectToServerAsync();
            if (_workflowResponseValidator.IsValidResponseToSend(createFirstTradeCommandResponse))
            {
                await _publisherService.PublishEventAsync(
                                                   _eventBusSettings.FirstTradeTopicName,
                                                   createFirstTradeCommandResponse);
            }
        }
        catch (Exception exception)
        {
            _metricslogger.LogErrorMessage($"Error in creating Trade event trackingId={request?.CustomerWorkflowId} " +
                                           $"CustomerId={request?.CustomerId} " +
                                           $"EventDetails={request?.EventDetails} " +
                                           $"exception={exception} InnerException={exception?.InnerException}");
        }
        return createFirstTradeCommandResponse;
    }
}