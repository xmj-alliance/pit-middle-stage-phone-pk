using MiddleStagePhonePK.App.Models.Squidex;

namespace MiddleStagePhonePK.App.Services;

public interface IDataAccessService
{
    Task<SquidexQueryTypes> QueryContentsByIDs(string gqlQueryName, IEnumerable<string> ids, string gqlResultSelector);
    Task<IEnumerable<SquidexMutationTypes>> CreateContents<T>(
        string gqlMutationName,
        string gqlInputTypeName,
        IEnumerable<T> newItems,
        string gqlResultSelector
    );
    Task<IEnumerable<SquidexMutationTypes>> UpdateContents<T>(
        string gqlMutationName,
        string gqlInputTypeName,
        IDictionary<string, T> idNewItemMap,
        string gqlResultSelector
    );
    Task<IDictionary<string, SquidexMutationTypes>> DeleteContents(
        string gqlMutationName,
        IEnumerable<string> ids,
        string gqlResultSelector
    );
}
