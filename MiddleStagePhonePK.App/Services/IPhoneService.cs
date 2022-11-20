using MiddleStagePhonePK.App.Models;

namespace MiddleStagePhonePK.App.Services;

public interface IPhoneService
{
    Task<List<Phone>> GetByIDs(IEnumerable<string> ids);
    Task<List<Phone>> Add(IEnumerable<InputPhone> newItems);
    Task<List<Phone>> Update(IEnumerable<InputPhone> updatingItems);
    Task<List<Phone>> Delete(IEnumerable<string> ids);
}
