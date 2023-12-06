using ApplicationServiceLayerTraining.Frameworks.Contracts;

namespace ApplicationServiceLayerTraining.Frameworks;

public class Response<T>
{
    public bool IsSuccessful { get; set; } = false;
    public Status Status { get; set; } = Status.Init;
    public string? Message { get; set; }
    public T? Value { get; set; }
}

public enum Status
{
    NullRef = 1,
    NotExist = -2,
    Successful = 1,
    Init = 0
}
