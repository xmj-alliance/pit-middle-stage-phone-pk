using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleStagePhonePK.App.Services;
using MiddleStagePhonePK.Test.Infrastructure.Services;

namespace MiddleStagePhonePK.Test.Infrastructure;

public class ServiceFixture
{
    public IHost TestHost { get; set; }

    public ServiceFixture()
    {
        TestHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // add services
                services.AddSingleton<IPhoneService, MockPhoneService>();
            })
            .Build();
    }
}
