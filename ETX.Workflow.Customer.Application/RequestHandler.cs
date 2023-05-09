namespace ETX.Workflow.Customer.Application;

public class RequestHandler : IRequestHandler
{
    private readonly IMetricsLoggerService _metricsLoggerService = default;
    private readonly IWorkflowBuilder _customerWorkflowBuilder = default;
    private readonly IMediator _mediator = default;
    private readonly IMapper _mapper = default;

    private readonly Func<WorkflowEventSource, IWorkflowSource>
        _customerWorkflowEventSourceDelegate = default;

    public RequestHandler(
        IMetricsLoggerService metricsLoggerService
        , IMediator mediator
        , IMapper mapper
        , IWorkflowBuilder customerWorkflowBuilder
        , Func<WorkflowEventSource, IWorkflowSource> customerWorkflowEventSourceDelegate)
    {
        _metricsLoggerService = metricsLoggerService;
        _mediator = mediator;
        _mapper = mapper;
        _customerWorkflowBuilder = customerWorkflowBuilder;
        _customerWorkflowEventSourceDelegate = customerWorkflowEventSourceDelegate;
    }

    public async Task HandleCommandsAsync(
        WorkflowEventSource customerWorkflowEventSource)
    {
        List<ResponseBase> responseList = new List<ResponseBase>();
        try
        {
            var customerWorkflowSource = _customerWorkflowEventSourceDelegate(customerWorkflowEventSource);
            var workflowEvents = await customerWorkflowSource.GetWorkflowEventsAsync();
            workflowEvents.ForEach(async events =>
            {
                responseList.Add(new ResponseBase
                {
                    CustomerId = events.CustomerId,
                    CustomerWorkflowId = events.Id
                });
                var request = _mapper.Map<CreateWorkflowRequest>(events);
                var createWorkflowEventRequest = _customerWorkflowBuilder.BuildCommand(request);

                var response = await _mediator.Send(createWorkflowEventRequest);
            });

            //Update status
            var workflowWithProcessingStatus = (customerWorkflowSource as WorkflowWithProcessingStatus);
            if (workflowWithProcessingStatus != null)
            {
                await workflowWithProcessingStatus.UpdateWorkflowEventsAsync(
                                                   QueryConditionConstants.COMPLETED
                                                   , responseList);
            }
        }
        catch (Exception exception)
        {
            _metricsLoggerService.LogDebugMessage($"MethodName={MethodBase.GetCurrentMethod}" +
                                                  $"Message ={ exception.Message}");
        }
    }
}