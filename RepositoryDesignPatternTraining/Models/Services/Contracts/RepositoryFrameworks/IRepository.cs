namespace RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T> where T : class
{
    IEnumerable<T> SelectAll();
    T SelectById(Guid Id);
    void Insert(T obj);
    void Update(T obj);
    //void Update(Guid Id, string values);
    void Delete(Guid Id);
    void Delete(T obj);
    void Save();
}