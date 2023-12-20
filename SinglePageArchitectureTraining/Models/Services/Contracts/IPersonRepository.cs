using SinglePageArchitectureTraining.Frameworks.Contracts;
using SinglePageArchitectureTraining.Models.DomainModels.PersonAggregates;
using SinglePageArchitectureTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace SinglePageArchitectureTraining.Models.Services.Contracts;

public interface IPersonRepository : IRepository<Person, IEnumerable<Person>>
{
    Task<IResponse<Person>> SelectByNationalCode(string nationalCode);
}
