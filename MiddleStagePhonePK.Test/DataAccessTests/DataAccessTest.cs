using MiddleStagePhonePK.App.Models;
using MiddleStagePhonePK.App.Models.Squidex;
using MiddleStagePhonePK.App.Services;
using Moq;

namespace MiddleStagePhonePK.Test.DataAccessTests;
public class DataAccessTest
{
    [Theory]
    [ClassData(typeof(SquidexQueryContentsResponseData))]
    public async Task Mockup_QueryContentsByIDs(SquidexQueryTypes response)
    {
        Mock<IDataAccessService> mock = new ();

        mock.Setup(service =>
            service.QueryContentsByIDs(
                It.IsAny<string>(),
                It.IsAny<List<string>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(response));

        IDataAccessService mockDataAccessService = mock.Object;

        var result = await mockDataAccessService.QueryContentsByIDs(
            "queryPhoneContents",
            new List<string>() { "someguid" },
            $@"{{
                id
                data {{
                   name {{
                    en
                   }}
                   description {{
                    en
                   }}
                }}
            }}"
        );

        Assert.NotNull(result);

        Assert.Equal("someguid", result.QueryPhoneContents![0].Id);
    }

    [Theory]
    [ClassData(typeof(SquidexCreateContentsResponseData))]
    public async Task Mockup_CreateContents(IEnumerable<SquidexMutationTypes> response)
    {
        Mock<IDataAccessService> mock = new();

        mock.Setup(service =>
            service.CreateContents(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<List<SquidexPhoneDataInputDto>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(response));

        IDataAccessService mockDataAccessService = mock.Object;

        var result = await mockDataAccessService.CreateContents(
            "createPhoneContent",
            "PhoneDataInputDto",
            new List<SquidexPhoneDataInputDto>() {
                new SquidexPhoneDataInputDto(
                    name: new SquidexI18NInputDto(
                        en: "Created Pingkang Phone"
                    ),
                    description: new SquidexI18NInputDto(
                        en: "Created Pingkang Phone"
                    )
                )
            },
            $@"{{
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
	        }}"
        );

        Assert.NotNull(result);

        Assert.Equal("Created Pingkang Phone", result.First().CreatePhoneContent!.Data!.Name.En);
    }

    [Theory]
    [ClassData(typeof(SquidexUpdateContentsResponseData))]
    public async Task Mockup_UpdateContents(IEnumerable<SquidexMutationTypes> response)
    {
        Mock<IDataAccessService> mock = new();

        mock.Setup(service =>
            service.UpdateContents(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, SquidexPhoneDataInputDto>> (),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(response));

        IDataAccessService mockDataAccessService = mock.Object;

        var result = await mockDataAccessService.UpdateContents(
            "updatePhoneContent",
            "PhoneDataInputDto",
            new Dictionary<string, SquidexPhoneDataInputDto>()
            {
                ["someguid"] = new SquidexPhoneDataInputDto(
                    name: new SquidexI18NInputDto(
                        en: "Updated Pingkang Phone"
                    ),
                    description: new SquidexI18NInputDto(
                        en: "Updated Pingkang Phone"
                    )
                )
            },
            $@"{{
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
            }}"
        );

        Assert.NotNull(result);

        Assert.Equal("Updated Pingkang Phone", result.First().UpdatePhoneContent!.Data!.Name.En);
    }

    [Theory]
    [ClassData(typeof(SquidexDeleteContentsResponseData))]
    public async Task Mockup_DeleteContents(IDictionary<string, SquidexMutationTypes> response)
    {
        Mock<IDataAccessService> mock = new();

        mock.Setup(service =>
            service.DeleteContents(
                It.IsAny<string>(),
                It.IsAny<List<string>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(response));

        IDataAccessService mockDataAccessService = mock.Object;

        var result = await mockDataAccessService.DeleteContents(
            "deletePhoneContent",
            new List<string>() { "someguid" },
            $@"{{
                version
            }}"
        );

        Assert.NotNull(result);

        var deletedPhoneResult = result["someguid"];

        Assert.True(deletedPhoneResult.DeletePhoneContent!.Version > 0);
    }
}

public class SquidexQueryContentsResponseData : TheoryData<SquidexQueryTypes>
{
    public SquidexQueryContentsResponseData()
    {
        Add(new SquidexQueryTypes(
            QueryPhoneContents: new List<PhoneQueryContentType>()
            {
                new PhoneQueryContentType()
                {
                    Id = "someguid",
                    Data = new PhoneGraphDataType(
                        Name: new SquidexI18NDto(
                            En: "Pingkang Phone"
                        ),
                        Description: new SquidexI18NDto(
                            En: "Pingkang Phone"
                        )
                    )
                }
            })
        );
    }

}

public class SquidexCreateContentsResponseData : TheoryData<IEnumerable<SquidexMutationTypes>>
{
    public SquidexCreateContentsResponseData()
    {
        Add(
            new List<SquidexMutationTypes>()
            {
                new SquidexMutationTypes()
                {
                    CreatePhoneContent = new PhoneQueryContentType()
                    {
                        Id = "someguid",
                        Created = DateTime.Now,
                        CreatedBy = "me",
                        Data = new PhoneGraphDataType(
                            Name: new SquidexI18NDto(
                                En: "Created Pingkang Phone"
                            ),
                            Description: new SquidexI18NDto(
                                En: "Created Pingkang Phone"
                            )
                        )
                    }
                }
            }
        );
    }
}

public class SquidexUpdateContentsResponseData : TheoryData<IEnumerable<SquidexMutationTypes>>
{
    public SquidexUpdateContentsResponseData()
    {
        Add(
            new List<SquidexMutationTypes>()
            {
                new SquidexMutationTypes()
                {
                    UpdatePhoneContent = new PhoneQueryContentType()
                    {
                        Id = "someguid",
                        LastModified = DateTime.Now,
                        LastModifiedBy = "me",
                        Data = new PhoneGraphDataType(
                            Name: new SquidexI18NDto(
                                En: "Updated Pingkang Phone"
                            ),
                            Description: new SquidexI18NDto(
                                En: "Updated Pingkang Phone"
                            )
                        )
                    }
                }
            }
        );
    }
}

public class SquidexDeleteContentsResponseData : TheoryData<IDictionary<string, SquidexMutationTypes>>
{
    public SquidexDeleteContentsResponseData()
    {
        Add(
            new Dictionary<string, SquidexMutationTypes>()
            {
                ["someguid"] = new SquidexMutationTypes()
                    {
                        DeletePhoneContent = new SquidexEntitySavedResultDto(
                            Version: 1
                        )
                    }
            }
        );
    }
}
