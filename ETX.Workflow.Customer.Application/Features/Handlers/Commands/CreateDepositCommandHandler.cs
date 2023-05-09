namespace ETX.Workflow.Customer.Application.Features.Handlers.Commands;

public class CreateDepositCommandHandler
     : IRequestHandler<CreateFirstDepositCommandRequest, CreateFirstDepositCommandResponse>
{
    private readonly IMapper _mapper = default;
    private readonly IMetricsLoggerService _metricslogger = default;
    private readonly IPublisherService _publisherService = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IWorkflowResponseValidator _workflowResponseValidator = default;

    public CreateDepositCommandHandler(
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

    public async Task<CreateFirstDepositCommandResponse> Handle(
        CreateFirstDepositCommandRequest request
        , CancellationToken cancellationToken)
    {
        CreateFirstDepositCommandResponse createFirstDepositCommandResponse = default;
        try
        {
            var firstDepositEvent = _mapper.Map<Events.WorkflowEvents>(request);
            var workflow = new ETX.Workflow.Customer.Domain.Workflow(firstDepositEvent);
            createFirstDepositCommandResponse = _mapper
                                                   .Map<CreateFirstDepositCommandResponse>(workflow.Deposit);

            await _publisherService.ConnectToServerAsync();
            if (_workflowResponseValidator.IsValidResponseToSend(createFirstDepositCommandResponse))
            {
                await _publisherService.PublishEventAsync(
                                                      _eventBusSettings.FirstDepositTopicName,
                                                      createFirstDepositCommandResponse);
            }
        }
        catch (Exception exception)
        {
            _metricslogger.LogErrorMessage($"Error in creating Deposit event trackingId={request?.CustomerWorkflowId} " +
                                           $"CustomerId={request?.CustomerId} " +
                                           $"EventDetails={request?.EventDetails} " +
                                           $"exception={exception} InnerException={exception?.InnerException}");
        }

        return createFirstDepositCommandResponse;
    }
}