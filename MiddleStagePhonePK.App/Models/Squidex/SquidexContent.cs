namespace MiddleStagePhonePK.App.Models.Squidex;

public record SquidexContent<T>(
    string Id,
    int Version,
    DateTime Created,
    string CreatedBy,
    DateTime LastModified,
    string LastModifiedBy,
    T? Data
);
