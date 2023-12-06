using ApplicationServiceLayerTraining.Frameworks;
using ApplicationServiceLayerTraining.Models.DomainModels.PersonAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts;

public interface IPersonRepository : IRepository<Person, IEnumerable<Person>>
{
    Task<Response<Person>> SelectByNationalCode(string nationalCode);
}
