namespace MiddleStagePhonePK.App.Models;

public record CUDMessage<T>(
    bool OK,
    string Content,
    IEnumerable<T> instances
);
