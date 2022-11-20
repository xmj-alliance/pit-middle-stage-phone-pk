namespace MiddleStagePhonePK.App.Models;

public record InputPhone(
    string? Id,
    string? Name,
    string? Description
)
{
    public InputPhone() : this(default, default, default) { }
}
