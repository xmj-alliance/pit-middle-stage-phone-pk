using MiddleStagePhonePK.App.Models;
using MiddleStagePhonePK.App.Models.Squidex;
using MiddleStagePhonePK.App.Services;
using Moq;

namespace MiddleStagePhonePK.Test.Infrastructure.Services;
public class MockPhoneService : PhoneService
{
    private static MockPhoneService _instance;

    public MockPhoneService(): base(SetupMock())
    { }

    public static IDataAccessService SetupMock()
    {
        Mock<IDataAccessService> mock = new();

        Build()
            .SetupCreateContents(mock)
            .SetupQueryContentsByIDs(mock)
            .SetupUpdateContents(mock)
            .SetupDeleteContents(mock);

        return mock.Object;
    }

    private static MockPhoneService Build()
    {
        if (_instance is null)
        {
            _instance = new MockPhoneService();
        }
        return _instance;
    }

    private MockPhoneService SetupQueryContentsByIDs(Mock<IDataAccessService> mock)
    {
        mock.Setup(service =>
            service.QueryContentsByIDs(
                It.IsAny<string>(),
                It.IsAny<List<string>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(
            new SquidexQueryTypes(
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
                }
            )
        ));

        return _instance;
    }

    private MockPhoneService SetupCreateContents(Mock<IDataAccessService> mock)
    {
        mock.Setup(service =>
            service.CreateContents(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<List<SquidexPhoneDataInputDto>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(
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
                                En: "Testing iPad"
                            ),
                            Description: new SquidexI18NDto(
                                En: "Testing iPad"
                            )
                        )
                    }
                }
            } as IEnumerable<SquidexMutationTypes>
        ));

        return _instance;
    }

    private MockPhoneService SetupUpdateContents(Mock<IDataAccessService> mock)
    {
        mock.Setup(service =>
            service.UpdateContents(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, SquidexPhoneDataInputDto>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(
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
                                En: "updated iPad nimi 4"
                            ),
                            Description: new SquidexI18NDto(
                                En: "updated iPad nimi 4"
                            )
                        )
                    }
                }
            } as IEnumerable<SquidexMutationTypes>
        ));

        return _instance;
    }

    private MockPhoneService SetupDeleteContents(Mock<IDataAccessService> mock)
    {
        mock.Setup(service =>
            service.DeleteContents(
                It.IsAny<string>(),
                It.IsAny<List<string>>(),
                It.IsAny<string>()
            )
        ).Returns(Task.FromResult(
            new Dictionary<string, SquidexMutationTypes>()
            {
                ["someguid"] = new SquidexMutationTypes()
                {
                    DeletePhoneContent = new SquidexEntitySavedResultDto(
                        Version: 1
                    )
                }
            } as IDictionary<string, SquidexMutationTypes>
        ));

        return _instance;
    }


}
