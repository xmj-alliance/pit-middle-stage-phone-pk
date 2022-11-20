using GraphQL;
using GraphQL.Client.Http;
using MiddleStagePhonePK.App.Models.Squidex;
using MiddleStagePhonePK.App.Relay;

namespace MiddleStagePhonePK.App.Services;

public class DataAccessService : IDataAccessService
{
    private readonly GraphQLHttpClient client;
    private readonly ILogger<DataAccessService> logger;

    public DataAccessService(
        ILogger<DataAccessService> logger,
        IGraphQLClientContext clientContext
    )
    {
        this.logger = logger;
        client = clientContext.Client;
    }

    public async Task<SquidexQueryTypes> QueryContentsByIDs(string gqlQueryName, IEnumerable<string> ids, string gqlResultSelector)
    {
        string query =
            $"query {gqlQueryName}($filter: String) {{" +
                $"{gqlQueryName}(filter: $filter)" +
                $"{gqlResultSelector}" +
            $"}}";

        IEnumerable<string> quotedIDs =
            from id in ids
            select $@"'{id}'";

        var gqlRequest = new GraphQLRequest
        {
            Query = query,
            Variables = new
            {
                filter = $@"id in ({string.Join(',', quotedIDs)})"
            }
        };

        var graphQLResponse = await client.SendQueryAsync<SquidexQueryTypes>(gqlRequest);

        if (graphQLResponse.Errors?.Length > 0)
        {
            List<string> errors = (
                from error in graphQLResponse.Errors
                select error.Message
            ).ToList();
            throw new ApplicationException($@"GQLErrors: {string.Join("\n", errors)}");
        }

        return graphQLResponse.Data;
    }

    public async Task<IEnumerable<SquidexMutationTypes>> CreateContents<T>(
        string gqlMutationName,
        string gqlInputTypeName,
        IEnumerable<T> newItems,
        string gqlResultSelector
    )
    {
        // Unfortunately Squidex API does not support bulk
        List<SquidexMutationTypes> resultData = new();

        foreach (var item in newItems)
        {
            string mutation = 
                $"mutation {gqlMutationName}($inputContent: {gqlInputTypeName}!) {{" +
                    $"{gqlMutationName}(" +
                        $"data: $inputContent," +
                        $"publish: true" +
                    $@") { gqlResultSelector}" +
                $"}}";

            var gqlRequest = new GraphQLRequest
            {
                Query = mutation,
                OperationName = gqlMutationName,
                Variables = new
                {
                    inputContent = item
                }
            };

            var graphQLResponse = await client.SendMutationAsync<SquidexMutationTypes>(gqlRequest);

            if (graphQLResponse.Errors?.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    logger.LogError("Error Message: {errorMessage}", error.Message);
                }

                continue;
            }

            resultData.Add(graphQLResponse.Data);

        }

        return resultData;

    }

    /// <summary>
    /// Replace an entity with complete information
    /// Non-provided fields will become null
    /// </summary>
    /// <param name="gqlMutationName"></param>
    /// <param name="gqlInputTypeName"></param>
    /// <param name="idNewItemMap">ID to New Item Map</param>
    /// <param name="gqlResultSelector"></param>
    /// <returns></returns>
    public async Task<IEnumerable<SquidexMutationTypes>> UpdateContents<T>(
        string gqlMutationName,
        string gqlInputTypeName,
        IDictionary<string, T> idNewItemMap,
        string gqlResultSelector
    )
    {
        // Unfortunately Squidex API does not support bulk
        List<SquidexMutationTypes> resultData = new();

        foreach (var (key, value) in idNewItemMap)
        {
            string mutation =
                $"mutation {gqlMutationName}($id: String!, $inputContent: {gqlInputTypeName}!) {{" +
                    $"{gqlMutationName}(" +
                        $"id: $id," +
                        $"data: $inputContent" +
                    $@") { gqlResultSelector }" +
                $"}}";

            var gqlRequest = new GraphQLRequest
            {
                Query = mutation,
                OperationName = gqlMutationName,
                Variables = new
                {
                    id = key,
                    inputContent = value
                }
            };

            var graphQLResponse = await client.SendMutationAsync<SquidexMutationTypes>(gqlRequest);

            if (graphQLResponse.Errors?.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    logger.LogError("Error Message: {errorMessage}", error.Message);
                }

                continue;
            }

            resultData.Add(graphQLResponse.Data);
        }

        return resultData;

    }

    public async Task<IDictionary<string, SquidexMutationTypes>> DeleteContents(
        string gqlMutationName,
        IEnumerable<string> ids,
        string gqlResultSelector
    )
    {
        // Unfortunately Squidex API does not support bulk
        IDictionary<string, SquidexMutationTypes> idDeletingResponseMap = new Dictionary<string, SquidexMutationTypes>();

        foreach (var id in ids)
        {
            string mutation =
                $"mutation {gqlMutationName}($id: String!) {{" +
                    $"{gqlMutationName}(" +
                        $"id: $id" +
                    $@") { gqlResultSelector }" +
                $"}}";
            
            var gqlRequest = new GraphQLRequest
            {
                Query = mutation,
                OperationName = gqlMutationName,
                Variables = new
                {
                    id
                }
            };

            var graphQLResponse = await client.SendMutationAsync<SquidexMutationTypes>(gqlRequest);

            if (graphQLResponse.Errors?.Length > 0)
            {
                foreach (var error in graphQLResponse.Errors)
                {
                    logger.LogError("Error deleting: {item}", id);
                    logger.LogError("Error Message: {errorMessage}: ", error.Message);
                }

                continue;
            }

            if (graphQLResponse.Data is null)
            {
                continue;
            }

            idDeletingResponseMap.Add(id, graphQLResponse.Data);
        }

        return idDeletingResponseMap;
    }

}
