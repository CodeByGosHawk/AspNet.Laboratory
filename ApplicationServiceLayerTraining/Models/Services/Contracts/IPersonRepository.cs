using ApplicationServiceLayerTraining.Models.DomainModels.PersonAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person> SelectByNationalCode(string nationalCode);
}
