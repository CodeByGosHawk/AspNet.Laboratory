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
        if (createPersonDto is null) return false;

        var createdPerson = new Person()
        {
            Id = new Guid(),
            FirstName = createPersonDto.FirstName,
            LastName = createPersonDto.LastName,
            NationalCode = createPersonDto.NationalCode
        };

        var insertOperationResponse = await _personRepository.Insert(createdPerson);
        return insertOperationResponse.IsSuccessful;
    }

    #endregion

    #region[Read]

    public async Task<IEnumerable<ServiceSelectPersonDto>?> SelectAllAsync()
    {
        List<ServiceSelectPersonDto> peopleDto = new List<ServiceSelectPersonDto>();
        var selectAllOpertaionResponse = await _personRepository.SelectAll();
        if (!selectAllOpertaionResponse.IsSuccessful) return null;

        foreach (var person in selectAllOpertaionResponse.Value!)
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

    public async Task<ServiceSelectPersonDto?> SelectByIdAsync(Guid id)
    {
        var selectOperationResponse = await _personRepository.SelectById(id);
        if (!selectOperationResponse.IsSuccessful) return null;

        var selectedPersonDto = new ServiceSelectPersonDto()
        {
            Id = selectOperationResponse.Value!.Id,
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };
        return selectedPersonDto;
    }

    public async Task<ServiceSelectPersonDto?> SelectByNationalCodeAsync(string nationalCode)
    {
        var selectOperationResponse = await _personRepository.SelectByNationalCode(nationalCode);
        if (!selectOperationResponse.IsSuccessful) return null;

        var selectedPersonDto = new ServiceSelectPersonDto()
        {
            Id = selectOperationResponse.Value!.Id,
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };
        return selectedPersonDto;
    }

    #endregion

    #region[Update]

    public async Task<bool> UpdateAsync(ServiceUpdatePersonDto updatePersonDto)
    {
        var selectOperationResponse = await _personRepository.SelectById(updatePersonDto.Id);

        if (!selectOperationResponse.IsSuccessful) return false;

        var updatedPerson = selectOperationResponse.Value;

        updatedPerson!.FirstName = updatePersonDto.FirstName;
        updatedPerson.LastName = updatePersonDto.LastName;
        updatedPerson.NationalCode = updatePersonDto.NationalCode;

        var updateOperationResponse = await _personRepository.Update(updatedPerson);

        return updateOperationResponse.IsSuccessful;
    }

    #endregion

    #region[Delete]

    public async Task<bool> DeleteAsync(ServiceDeletePersonDto deletePersonDto)
    {
        var selectOperationResponse = await _personRepository.SelectById(deletePersonDto.Id);
        if (!selectOperationResponse.IsSuccessful) return false;

        var deletedPerson = selectOperationResponse.Value;

        var deleteOperationResponse = await _personRepository.Delete(deletedPerson!);
        return deleteOperationResponse.IsSuccessful;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var deleteOperationResponse = await _personRepository.Delete(id);
        return deleteOperationResponse.IsSuccessful;
    }

    #endregion

    #region[Save]

    public async Task SaveAsync()
    {
        await _personRepository.Save();
    }

    #endregion
}
