using GraphQL.Client.Http;

namespace MiddleStagePhonePK.App.Relay;

public interface IGraphQLClientContext
{
    GraphQLHttpClient Client { get; }
}
