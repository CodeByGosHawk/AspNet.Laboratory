using SinglePageArchitectureTraining.Frameworks.Enums;

namespace SinglePageArchitectureTraining.Frameworks.Contracts;

public interface IResponse<T>
{
    bool IsSuccessful { get; set; }
    Status Status { get; set; }
    string? Message { get; set; }
    T? Value { get; set; }
}
