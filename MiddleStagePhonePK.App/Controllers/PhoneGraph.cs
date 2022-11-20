using GraphQL;
using MiddleStagePhonePK.App.Models;
using MiddleStagePhonePK.App.Services;

namespace MiddleStagePhonePK.App.Controllers;

public class PhoneGraph
{
    private readonly IPhoneService phoneService;

    public PhoneGraph(
        IPhoneService phoneService
    )
    {
        this.phoneService = phoneService;
    }

    public async Task<List<Phone>> Get(IEnumerable<string> ids)
    {
        return await phoneService.GetByIDs(ids);
    }

    public async Task<List<Phone>> Add(IEnumerable<InputPhone> newItems)
    {
        return await phoneService.Add(newItems);
    }

    public async Task<List<Phone>> Update(IEnumerable<InputPhone> updatingItems)
    {
        return await phoneService.Update(updatingItems);
    }

    public async Task<List<Phone>> Delete(IEnumerable<string> ids)
    {
        return await phoneService.Delete(ids);
    }

}

public partial class Query
{
    [GraphQLMetadata("phones")]
    public async Task<List<Phone>> GetPhones(IEnumerable<string> ids)
        => await phoneGraph.Get(ids);
}

public partial class Mutation
{
    [GraphQLMetadata("addPhones")]
    public async Task<List<Phone>> AddPhones(IEnumerable<InputPhone> newItems)
        => await phoneGraph.Add(newItems);

    [GraphQLMetadata("updatePhones")]
    public async Task<List<Phone>> UpdatePhones(IEnumerable<InputPhone> updatingItems)
        => await phoneGraph.Update(updatingItems);

    [GraphQLMetadata("deletePhones")]
    public async Task<List<Phone>> DeletePhones(IEnumerable<string> ids)
        => await phoneGraph.Delete(ids);
}
