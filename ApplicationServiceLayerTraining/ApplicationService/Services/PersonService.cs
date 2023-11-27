using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Models.DomainModels.PersonAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts;

namespace ApplicationServiceLayerTraining.ApplicationService.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    #region[Create]

    public async Task<bool> InsertAsync(ServiceCreatePersonDto createPersonDto)
    {
        var createdPerson = new Person()
        {
            Id = new Guid(),
            FirstName = createPersonDto.FirstName,
            LastName = createPersonDto.LastName,
            NationalCode = createPersonDto.NationalCode
        };

        bool result = await _personRepository.Insert(createdPerson);
        return result;
    }

    #endregion

    #region[Read]

    public async Task<IEnumerable<ServiceSelectPersonDto>> SelectAllAsync()
    {
        List<ServiceSelectPersonDto> peopleDto = new List<ServiceSelectPersonDto>();
        var people = await _personRepository.SelectAll();
        foreach (var person in people)
        {
            var personDto = new ServiceSelectPersonDto()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalCode = person.NationalCode
            };
            peopleDto.Add(personDto);
        }
        return peopleDto;
    }

    public async Task<ServiceSelectPersonDto> SelectByIdAsync(Guid id)
    {
        var selectedPerson = await _personRepository.SelectById(Id);
        var selectedPersonDto = new ServiceSelectPersonDto()
        {
            Id = selectedPerson.Id,
            FirstName = selectedPerson.FirstName,
            LastName = selectedPerson.LastName,
            NationalCode = selectedPerson.NationalCode
        };
        return selectedPersonDto;
    }

    public async Task<ServiceSelectPersonDto> SelectByNationalCodeAsync(string nationalCode)
    {
        var selectedPerson = await _personRepository.SelectByNationalCode(nationalCode);
        var selectedPersonDto = new ServiceSelectPersonDto()
        {
            Id = selectedPerson.Id,
            FirstName = selectedPerson.FirstName,
            LastName = selectedPerson.LastName,
            NationalCode = selectedPerson.NationalCode
        };
        return selectedPersonDto;
    }

    #endregion

    #region[Update]

    public async Task<bool> UpdateAsync(ServiceUpdatePersonDto updatePersonDto)
    {
        var updatedPerson = await _personRepository.SelectById(updatePersonDto.Id);
        updatedPerson.FirstName = updatePersonDto.FirstName;
        updatedPerson.LastName = updatePersonDto.LastName;
        updatedPerson.NationalCode = updatePersonDto.NationalCode;
        var result = await _personRepository.Update(updatedPerson);
        return result;
    }

    #endregion

    #region[Delete]

    public async Task<bool> DeleteAsync(ServiceDeletePersonDto deletePersonDto)
    {
        var deletedPerson = await _personRepository.SelectById(deletePersonDto.Id);
        var result = await _personRepository.Delete(deletedPerson);
        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var result = await _personRepository.Delete(Id);
        return result;
    }

    #endregion

    #region[Save]

    public async Task SaveAsync()
    {
        await _personRepository.Save();
    }

    #endregion
}
