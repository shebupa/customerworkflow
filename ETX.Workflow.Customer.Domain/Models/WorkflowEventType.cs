namespace ETX.Workflow.Customer.Domain.Models;

public enum WorkflowEventType
{
    [Description("None")]
    None = 0,

    [Description("Demo")]
    Demo = 1,

    [Description("Join")]
    Join = 2,

    [Description("Completed")]
    Completed = 3,

    [Description("Authorised")]
    Authorised = 4,

    [Description("FirstDeposit")]
    FirstDeposit = 5,

    [Description("FirstTrade")]
    FirstTrade = 6
}