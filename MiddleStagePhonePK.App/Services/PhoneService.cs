using MiddleStagePhonePK.App.Models;
using MiddleStagePhonePK.App.Models.Squidex;

namespace MiddleStagePhonePK.App.Services;

public class PhoneService : IPhoneService
{
    private readonly IDataAccessService dataAccessService;

    public PhoneService(IDataAccessService dataAccessService)
    {
        this.dataAccessService = dataAccessService;
    }

    public async Task<List<Phone>> GetByIDs(IEnumerable<string> ids)
    {

        string gqlResultSelector = $@"{{
            id
            data {{
               name {{
                en
               }}
               description {{
                en
               }}
            }}
        }}";

        var data = (await dataAccessService.QueryContentsByIDs(
            "queryPhoneContents",
            ids,
            gqlResultSelector
        )).QueryPhoneContents;

        if (data is null)
        {
            return new List<Phone>();
        }

        var phones =
            from phoneResult in data
            where phoneResult.Data is not null
            select new Phone(
                Id: phoneResult.Id,
                Name: phoneResult.Data!.Name.En,
                Description: phoneResult.Data.Description.En
            );

        return phones.ToList();
    }

    public async Task<List<Phone>> Add(IEnumerable<InputPhone> newItems)
    {

        string gqlResultSelector = $@"
        {{
            id
		    createdBy
		    created
		    data {{
			    name {{
				    en
			    }}
			    description {{
				    en
			    }}
		    }}
	    }}
        ";

        List<SquidexPhoneDataInputDto> newSquidexContents = (
            from item in newItems
            select new SquidexPhoneDataInputDto(
                name: new SquidexI18NInputDto(
                    en: item.Name ?? "Noname phone"
                ),
                description: new SquidexI18NInputDto(
                    en: item.Description ?? item.Name ?? "Noname phone"
                )
            )
        ).ToList();

        var responses = await dataAccessService.CreateContents(
            "createPhoneContent",
            "PhoneDataInputDto",
            newSquidexContents,
            gqlResultSelector
        );

        return (
            from response in responses
            where response.CreatePhoneContent is { }
            select new Phone(
                Id: response.CreatePhoneContent!.Id,
                Name: response.CreatePhoneContent.Data!.Name.En,
                Description: response.CreatePhoneContent.Data.Description.En
            )
        ).ToList();

    }

    public async Task<List<Phone>> Update(IEnumerable<InputPhone> updatingItems)
    {

        string gqlResultSelector = $@"
        {{
            id
		    lastModified
		    lastModifiedBy
		    data {{
			    name {{
				    en
			    }}
			    description {{
				    en
			    }}
		    }}
        }}
        ";

        Dictionary<string, SquidexPhoneDataInputDto> idNewItemMap = new();

        IEnumerable<InputPhone> validUpdatingItems = (
            from item in updatingItems
            where item.Id is { }
            select item
        );

        foreach (var item in validUpdatingItems)
        {
            idNewItemMap.Add(item.Id!, new SquidexPhoneDataInputDto(
                name: new SquidexI18NInputDto(
                    en: item.Name ?? "Noname phone"
                ),
                description: new SquidexI18NInputDto(
                    en: item.Description ?? item.Name ?? "Noname phone"
                )
            ));
        }

        var responses = await dataAccessService.UpdateContents(
            "updatePhoneContent",
            "PhoneDataInputDto",
            idNewItemMap,
            gqlResultSelector
        );

        return (
            from response in responses
            where response.UpdatePhoneContent is { }
            select new Phone(
                Id: response.UpdatePhoneContent!.Id,
                Name: response.UpdatePhoneContent.Data!.Name.En,
                Description: response.UpdatePhoneContent.Data.Description.En
            )
        ).ToList();

    }

    public async Task<List<Phone>> Delete(IEnumerable<string> ids)
    {
        // Get the phones being deleted
        List<Phone> deletingPhones = await GetByIDs(ids);

        string gqlResultSelector = $@"
        {{
            version
        }}
        ";

        var idDeletingItemMap = await dataAccessService.DeleteContents(
            "deletePhoneContent",
            ids,
            gqlResultSelector
        );

        return deletingPhones.Where(
            (ele) => idDeletingItemMap.ContainsKey(ele.Id)
        ).ToList();
    }

}
