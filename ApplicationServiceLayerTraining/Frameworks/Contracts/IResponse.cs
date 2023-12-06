namespace ApplicationServiceLayerTraining.Frameworks.Contracts;

public interface IResponse<T>
{
    Task<bool> GetStatus();
    Task<string?> GetMessage();
    Task<T?> GetValue();
}
