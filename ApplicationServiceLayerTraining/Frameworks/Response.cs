using ApplicationServiceLayerTraining.Frameworks.Abstracts;
using ApplicationServiceLayerTraining.Frameworks.Contracts;

namespace ApplicationServiceLayerTraining.Frameworks;

public class Response<T> : IResponse<T>
{
    public bool IsSuccessful { get; set; } = false;
    public Status Status { get; set; } = Status.Init;
    public string? Message { get; set; }
    public T? Value { get; set; }
}