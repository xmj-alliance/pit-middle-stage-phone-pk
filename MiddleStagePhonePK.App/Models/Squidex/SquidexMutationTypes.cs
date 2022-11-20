namespace MiddleStagePhonePK.App.Models.Squidex;

public record SquidexMutationTypes(
    PhoneQueryContentType? CreatePhoneContent,
    PhoneQueryContentType? UpdatePhoneContent,
    SquidexEntitySavedResultDto? DeletePhoneContent
)
{
    public SquidexMutationTypes() : this(default, default, default) { }
};
