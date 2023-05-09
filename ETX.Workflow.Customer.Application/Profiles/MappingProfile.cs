using ETX.Workflow.Customer.Domain;

namespace ETX.Workflow.Customer.Application.Profiles;

[ExcludeFromCodeCoverage]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerWorkflowEvents, CreateWorkflowRequest>();

        //Registration Status
        //CreateMap<CreateWorkflowRequest, CreateRegistrationStatusCommandRequest>();
        CreateMap<CreateWorkflowRequest, CreateRegistrationStatusCommandRequest>()
               .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.Id));
        CreateMap<CreateRegistrationStatusCommandRequest, Events.WorkflowEvents>();
        CreateMap<Registration, CreateRegistrationStatusCommandResponse>()
                .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.CustomerWorkflowId))
                .ForMember(dest => dest.CustomerId, source => source.MapFrom(x => x.CustomerId))
                .ForPath(dest => dest.EventValue.StatusId, source => source.MapFrom(x => x.RegistrationStatus.StatusId))
                .ForPath(dest => dest.EventValue.StatusDescription, source => source.MapFrom(x => x.RegistrationStatus.StatusDescription))
                .ForPath(dest => dest.EventDateTime, source => source.MapFrom(x => x.RegistrationStatus.EventDateTime));

        //FirstDeposit
        CreateMap<CreateWorkflowRequest, CreateFirstDepositCommandRequest>()
                 .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.Id));
        CreateMap<CreateFirstDepositCommandRequest, Events.WorkflowEvents>();
        CreateMap<Deposit, CreateFirstDepositCommandResponse>()
                 .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.CustomerWorkflowId))
                 .ForMember(dest => dest.CustomerId, source => source.MapFrom(x => x.CustomerId))
                 .ForPath(dest => dest.EventValue.Currency, source => source.MapFrom(x => x.FirstDeposit.Currency))
                 .ForPath(dest => dest.EventValue.Amount, source => source.MapFrom(x => x.FirstDeposit.Amount))
                 .ForPath(dest => dest.EventDateTime, source => source.MapFrom(x => x.FirstDeposit.EventDateTime));

        //FirstTrade
        CreateMap<CreateFirstTradeCommandRequest, Events.WorkflowEvents>();
        CreateMap<CreateWorkflowRequest, CreateFirstTradeCommandRequest>()
         .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.Id));
        CreateMap<Trade, CreateFirstTradeCommandResponse>()
                .ForMember(dest => dest.CustomerWorkflowId, source => source.MapFrom(x => x.CustomerWorkflowId))
                .ForMember(dest => dest.CustomerId, source => source.MapFrom(x => x.CustomerId))
                .ForPath(dest => dest.EventValue.Currency, source => source.MapFrom(x => x.FirstTrade.Currency))
                .ForPath(dest => dest.EventValue.Amount, source => source.MapFrom(x => x.FirstTrade.Amount))
                .ForPath(dest => dest.EventValue.TradeDate, source => source.MapFrom(x => x.FirstTrade.TradeDate))
                .ForPath(dest => dest.EventValue.MarketName, source => source.MapFrom(x => x.FirstTrade.MarketName))
                .ForPath(dest => dest.EventValue.ClientId, source => source.MapFrom(x => x.FirstTrade.ClientId))
                .ForPath(dest => dest.EventDateTime, source => source.MapFrom(x => x.FirstTrade.EventDateTime));
    }
}