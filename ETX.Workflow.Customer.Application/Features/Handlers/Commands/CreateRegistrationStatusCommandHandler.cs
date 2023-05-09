namespace ETX.Workflow.Customer.Application.Features.Handlers.Commands;

public class CreateRegistrationStatusCommandHandler
    : IRequestHandler<CreateRegistrationStatusCommandRequest, CreateRegistrationStatusCommandResponse>
{
    private readonly IMapper _mapper = default;
    private readonly IMetricsLoggerService _metricslogger = default;
    private readonly IPublisherService _publisherService = default;
    private readonly EventBusSettings _eventBusSettings = default;
    private readonly IWorkflowResponseValidator _workflowResponseValidator = default;

    public CreateRegistrationStatusCommandHandler(
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

    public async Task<CreateRegistrationStatusCommandResponse> Handle(
        CreateRegistrationStatusCommandRequest request
        , CancellationToken cancellationToken)
    {
        CreateRegistrationStatusCommandResponse createRegistrationStatusCommandResponse = default;
        try
        {
            var registrationStatusEvent = _mapper.Map<Events.WorkflowEvents>(request);
            var workflow = new ETX.Workflow.Customer.Domain.Workflow(registrationStatusEvent);

            createRegistrationStatusCommandResponse = _mapper
                                                          .Map<CreateRegistrationStatusCommandResponse>(
                                                          workflow.Registration);

            await _publisherService.ConnectToServerAsync();
            if (_workflowResponseValidator.IsValidResponseToSend(createRegistrationStatusCommandResponse))
            {
                await _publisherService.PublishEventAsync(
                                                   _eventBusSettings.RegistrationStatusTopicName,
                                                   createRegistrationStatusCommandResponse);
            }
        }
        catch (Exception exception)
        {
            _metricslogger.LogErrorMessage($"Error in creating Registration status event trackingId={request?.CustomerWorkflowId} " +
                                           $"CustomerId={request?.CustomerId} " +
                                           $"EventDetails={request?.EventDetails} " +
                                           $"exception={exception} InnerException={exception?.InnerException}");
        }
        return createRegistrationStatusCommandResponse;
    }
}