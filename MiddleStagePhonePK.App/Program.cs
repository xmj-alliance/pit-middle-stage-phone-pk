using GraphQL;
using GraphQL.Types;
using IdentityModel.Client;
using MiddleStagePhonePK.App.Controllers;
using MiddleStagePhonePK.App.Relay;
using MiddleStagePhonePK.App.Services;
using MiddleStagePhonePK.App.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAccessTokenManagement( options =>
    {

        string backendAuthAddress = builder.Configuration.GetValue<string>("backendAuthURL")
                                    ?? throw new ApplicationException("backendAuthURL cannot be null");
        string backendAuthClientID = builder.Configuration.GetValue<string>("backendAuthClientID")
                                    ?? throw new ApplicationException("backendAuthClientID cannot be null");
        string backendAuthClientSecret = builder.Configuration.GetValue<string>("backendAuthClientSecret")
                                    ?? throw new ApplicationException("backendAuthClientSecret cannot be null");
        string backendAuthClientScope = builder.Configuration.GetValue<string>("backendAuthClientScope")
                                    ?? throw new ApplicationException("backendAuthClientScope cannot be null");

        string serverName = (new Uri(backendAuthAddress)).Host;

        options.Client.Clients.Add(serverName, new ClientCredentialsTokenRequest 
        {
            Address = backendAuthAddress,
            ClientId = backendAuthClientID,
            ClientSecret = backendAuthClientSecret,
            Scope = backendAuthClientScope
        });
    }
);

builder.Services.AddClientAccessTokenHttpClient("client", configureClient: client =>
{
    string backendBaseURL = builder.Configuration.GetValue<string>("backendBaseURL")
                                ?? throw new ApplicationException("backendBaseURL cannot be null");

    client.BaseAddress = new Uri(backendBaseURL);
});

builder.Services.AddTransient<IGraphQLClientContext, GraphQLClientContext>();
builder.Services.AddTransient<IDataAccessService, DataAccessService>();
builder.Services.AddSingleton<IPhoneService, PhoneService>();

// middle end setup
builder.Services.AddSingleton<Query>();
builder.Services.AddSingleton<Mutation>();
builder.Services.AddSingleton<PhoneGraph>();

builder.Services.AddSingleton<ISchema>((provider) =>
    {
        var schema = Schema.For(Graph.LoadDefinitions(), _ =>
        {
            _.Types.Include<Query>();
            _.Types.Include<Mutation>();
            _.ServiceProvider = provider;
        }
        );
        return schema;
    }
);


builder.Services.AddGraphQL(gqlBuilder =>
    gqlBuilder
        .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
        .AddSystemTextJson()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();
app.UseGraphQL<ISchema>("/graphql");

app.MapControllers();

app.Run();
