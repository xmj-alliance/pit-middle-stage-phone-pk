using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleStagePhonePK.App.Models;
using MiddleStagePhonePK.App.Services;
using MiddleStagePhonePK.Test.Infrastructure;

namespace MiddleStagePhonePK.Test.PhoneTests;
public class PhoneTest : IClassFixture<ServiceFixture>
{
    private readonly IHost testHost;
    private readonly IPhoneService phoneService;

    public PhoneTest(ServiceFixture fixture)
    {
        testHost = fixture.TestHost;
        phoneService = testHost.Services.GetService<IPhoneService>()!;
    }

    [Theory]
    [ClassData(typeof(PhoneQueryData))]
    public async Task GiveIDs_ReturnPhones(List<string> ids)
    {
        List<Phone> phones = await phoneService.GetByIDs(ids);

        Assert.Equal(ids.Count, phones.Count);

        Assert.Equal(ids.First(), phones.First().Id);
    }

    [Theory]
    [ClassData(typeof(PhoneCreateData))]
    public async Task GivePhones_ReturnAddedPhones(List<InputPhone> newPhones)
    {
        List<Phone> addedPhones = await phoneService.Add(newPhones);

        Assert.Equal(newPhones.Count, addedPhones.Count);

        Assert.Equal(newPhones.First().Name, addedPhones.First().Name);
    }

    [Theory]
    [ClassData(typeof(PhoneUpdateData))]
    public async Task GivePhones_ReturnUpdatedPhones(List<InputPhone> updatingPhones)
    {
        List<Phone> updatedPhones = await phoneService.Update(updatingPhones);

        Assert.Equal(updatingPhones.Count, updatedPhones.Count);

        Assert.Equal(updatingPhones.First().Name, updatedPhones.First().Name);
    }

    [Theory]
    [ClassData(typeof(PhoneDeleteData))]
    public async Task GiveIDs_ReturnDeletedPhones(List<string> ids)
    {
        List<Phone> deletedPhones = await phoneService.Delete(ids);

        Assert.Equal(ids.Count, deletedPhones.Count);

        Assert.Equal(ids.First(), deletedPhones.First().Id);
    }

}

public class PhoneQueryData : TheoryData<List<string>>
{
    public PhoneQueryData()
    {
        Add(
            new List<string>()
            {
                "someguid"
            }
        );
    }
}

public class PhoneCreateData : TheoryData<List<InputPhone>>
{
    public PhoneCreateData()
    {
        Add(
            new List<InputPhone>()
            {
                new InputPhone(
                    Id: null,
                    Name: "Testing iPad",
                    Description: "Testing iPad"
                ),
            }
        );
    }
}

public class PhoneUpdateData : TheoryData<List<InputPhone>>
{
    public PhoneUpdateData()
    {
        Add(
            new List<InputPhone>()
            {
                new InputPhone(
                    Id: "someguid",
                    Name: "updated iPad nimi 4",
                    Description: "Brand neeeeew iPad nimi 4"
                ),
            }
        );
    }
}

public class PhoneDeleteData : TheoryData<List<string>>
{
    public PhoneDeleteData()
    {
        Add(
            new List<string>()
            {
                "someguid"
            }
        );
    }
}
