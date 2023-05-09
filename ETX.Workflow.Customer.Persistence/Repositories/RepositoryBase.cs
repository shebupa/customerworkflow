namespace ETX.Workflow.Customer.Persistence.Repositories;

public class RepositoryBase<DataContext, EType> :
    IRepositoryBase<DataContext, EType>
    where EType : class
    where DataContext : DbContext
{
    private readonly DbContext _dbContext = default;

    public RepositoryBase(
            DataContext context)

    {
        _dbContext = context;
    }

    public async Task<IEnumerable<EType>> FindByConditionAsync(
          string sqlQuery,
          object param,
          IDbTransaction transaction = null,
          CancellationToken cancellationToken = default)
    {
        IEnumerable<EType> result = default;
        var conditionList = param as QueryCondition;

        await OpenConnection();

        if (_dbContext.Database.GetDbConnection().State == ConnectionState.Open)
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (conditionList != null)
            {
                var dbArgs = new DynamicParameters();
                foreach (var condition in conditionList.ConditionTags)
                {
                    dbArgs.Add(condition.Key, condition.Value);
                }
                result = await connection.QueryAsync<EType>(sqlQuery, dbArgs);
            }
            else
            {
                result = await connection.QueryAsync<EType>(sqlQuery, new { conditionalValue = param });
            }
        }

        await CloseConnection();

        return result;
    }

    public async Task<int> UpdateByConditionAsync(
          string sqlQuery,
          object param,
          IDbTransaction transaction = null,
          CancellationToken cancellationToken = default)
    {
        int result = default;
        var conditionList = param as QueryCondition;

        try
        {
            await OpenConnection();

            if (_dbContext.Database.GetDbConnection().State == ConnectionState.Open)
            {
                var connection = _dbContext.Database.GetDbConnection();

                if (conditionList != null)
                {
                    var dbArgs = new DynamicParameters();
                    if (conditionList.ConditionTags.Count() > 1)
                    {
                        foreach (var condition in conditionList.ConditionTags)
                        {
                            dbArgs.Add(condition.Key, conditionList.ConditionTagsForWhereIns);
                        }
                    }
                    else if (conditionList.ConditionTagsForWhereIns != null)
                    {
                        foreach (var condition in conditionList.ConditionTagsForWhereIns)
                        {
                            if (condition.Value.Count() == 1)
                            {
                                dbArgs.Add(condition.Key, condition.Value);
                            }
                            else
                            {
                                dbArgs.Add(conditionList.ConditionTagsForWhereIns.First().Key
                                      , conditionList.ConditionTagsForWhereIns.First().Value);
                            }
                        }
                    }

                    result = await connection.ExecuteAsync(sqlQuery, dbArgs);
                }
                else
                {
                    result = await connection.ExecuteAsync(sqlQuery, new { conditionalValue = param });
                }

                await CloseConnection();
            }
        }
        catch (Exception e)
        {
            //Log
            e = e;
        }

        return result;
    }

    private async Task OpenConnection()
    {
        if (_dbContext.Database.GetDbConnection().State == ConnectionState.Closed)
        {
            await _dbContext.Database.OpenConnectionAsync();
        }
    }

    private async Task CloseConnection()
    {
        if (_dbContext.Database.GetDbConnection().State == ConnectionState.Open)
        {
            await _dbContext.Database.CloseConnectionAsync();
        }
    }
}