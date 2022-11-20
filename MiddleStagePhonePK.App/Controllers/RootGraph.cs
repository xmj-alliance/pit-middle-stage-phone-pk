namespace MiddleStagePhonePK.App.Controllers;

public partial class Query
{
    private readonly PhoneGraph phoneGraph;

    public Query(
        PhoneGraph phoneGraph
    )
    {
        this.phoneGraph = phoneGraph;
    }

}

public partial class Mutation
{
    private readonly PhoneGraph phoneGraph;

    public Mutation(
        PhoneGraph phoneGraph
    )
    {
        this.phoneGraph = phoneGraph;
    }

}
