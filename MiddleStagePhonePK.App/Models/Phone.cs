using MiddleStagePhonePK.App.Models.Squidex;

namespace MiddleStagePhonePK.App.Models;

public record Phone(
    string Id,
    string Name,
    string Description
);

public record PhoneQueryContentType(
    string Id,
    int Version,
    DateTime Created,
    string CreatedBy,
    DateTime LastModified,
    string LastModifiedBy,
    PhoneGraphDataType? Data
): SquidexContent<PhoneGraphDataType>(Id, Version, Created, CreatedBy, LastModified, LastModifiedBy, Data)
{
    public PhoneQueryContentType(): this(default, default, default, default, default, default, default) { }
}

public record PhoneGraphDataType(
    SquidexI18NDto Name,
    SquidexI18NDto Description
); 
