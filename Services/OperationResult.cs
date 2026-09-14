namespace Huellitas.Services;

public enum OperationStatus
{
    Success,
    NotFound,
    Conflict
}

public sealed record OperationResult(OperationStatus Status, string? Error = null)
{
    public bool Succeeded => Status == OperationStatus.Success;

    public static OperationResult Success() => new(OperationStatus.Success);

    public static OperationResult NotFound() => new(OperationStatus.NotFound);

    public static OperationResult Conflict(string error) => new(OperationStatus.Conflict, error);
}
