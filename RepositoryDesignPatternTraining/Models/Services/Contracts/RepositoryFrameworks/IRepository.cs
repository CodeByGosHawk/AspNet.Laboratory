namespace RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T> where T : class
{
    IEnumerable<T> SelectAll();
    T SelectById(Guid Id);
    string Insert(T obj);
    string Update(T obj);
    //string Update(Guid Id, string values);
    string Delete(Guid Id);
    string Delete(T obj);
    void Save();
}