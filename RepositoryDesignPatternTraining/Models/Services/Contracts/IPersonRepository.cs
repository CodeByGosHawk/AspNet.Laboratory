using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace RepositoryDesignPatternTraining.Models.Services.Contracts;

public interface IPersonRepository : IRepository<Person>
{

}
