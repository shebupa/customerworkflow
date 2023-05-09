namespace ETX.Workflow.Customer.Application.Builders;

public class WorkflowBuilder : IWorkflowBuilder
{
    private readonly Dictionary<WorkflowEventType, Func<CreateWorkflowRequest, object>> _workflowEventTypeBuilders =
             new Dictionary<WorkflowEventType, Func<CreateWorkflowRequest, object>>();

    private readonly IMapper _mapper = default;
    private readonly IMetricsLoggerService _metricsloggerservice = default;

    public WorkflowBuilder(
          IMetricsLoggerService metricsLoggerService
        , IMapper mapper)
    {
        _metricsloggerservice = metricsLoggerService;
        _mapper = mapper;

        PopulateWorkFlowEventTypeBuilders();
    }

    private void PopulateWorkFlowEventTypeBuilders()
    {
        _workflowEventTypeBuilders.Add(WorkflowEventType.Demo, BuildRegistrationStatusWorkflow);
        _workflowEventTypeBuilders.Add(WorkflowEventType.Join, BuildRegistrationStatusWorkflow);
        _workflowEventTypeBuilders.Add(WorkflowEventType.Completed, BuildRegistrationStatusWorkflow);
        _workflowEventTypeBuilders.Add(WorkflowEventType.Authorised, BuildRegistrationStatusWorkflow);
        _workflowEventTypeBuilders.Add(WorkflowEventType.FirstDeposit, BuildFirstDepositWorkflow);
        _workflowEventTypeBuilders.Add(WorkflowEventType.FirstTrade, BuildFirstTradeWorkflow);
    }

    private CreateRegistrationStatusCommandRequest BuildRegistrationStatusWorkflow(CreateWorkflowRequest request)
    {
        var createRegistrationStatusCommandRequest = _mapper.Map<CreateRegistrationStatusCommandRequest>(request);
        return createRegistrationStatusCommandRequest;
    }

    private CreateFirstDepositCommandRequest BuildFirstDepositWorkflow(CreateWorkflowRequest request)
    {
        var createFirstDepositCommandRequest = _mapper.Map<CreateFirstDepositCommandRequest>(request);
        return createFirstDepositCommandRequest;
    }

    private CreateFirstTradeCommandRequest BuildFirstTradeWorkflow(CreateWorkflowRequest request)
    {
        var createFirstTradeCommandRequest = _mapper.Map<CreateFirstTradeCommandRequest>(request);
        return createFirstTradeCommandRequest;
    }

    public object BuildCommand(CreateWorkflowRequest request)
    {
        return _workflowEventTypeBuilders[(WorkflowEventType)request.EventTypeId](request);
    }
}