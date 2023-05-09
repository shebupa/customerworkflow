namespace ETX.Workflow.Customer.Application.Contracts.Persistence;

public interface IRepositoryBase<DataContext, EType>
{
    Task<IEnumerable<EType>> FindByConditionAsync(
         string sqlQuery
         , object param
         , IDbTransaction transaction
         , CancellationToken cancellationToken);

    Task<int> UpdateByConditionAsync(
         string sqlQuery
         , object param
         , IDbTransaction transaction
         , CancellationToken cancellationToken);
}