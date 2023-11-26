using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Models.Services.Contracts;

namespace ApplicationServiceLayerTraining.ApplicationService.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<bool> DeleteAsync(ServiceDeletePersonDto obj)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertAsync(ServiceCreatePersonDto obj)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ServiceSelectPersonDto>> SelectAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceSelectPersonDto> SelectByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ServiceUpdatePersonDto obj)
    {
        throw new NotImplementedException();
    }
}
