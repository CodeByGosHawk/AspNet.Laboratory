using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Frameworks;
using ApplicationServiceLayerTraining.Frameworks.Abstracts;
using ApplicationServiceLayerTraining.Frameworks.Contracts;
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

  

    // Create
    public async Task<IResponse<ServiceCreatePersonDto>> InsertAsync(ServiceCreatePersonDto createPersonDto)
    {
        var response = new Response<ServiceCreatePersonDto>();

        if (createPersonDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var createdPerson = new Person()
        {
            Id = new Guid(),
            FirstName = createPersonDto.FirstName,
            LastName = createPersonDto.LastName,
            NationalCode = createPersonDto.NationalCode
        };

        var insertOperationResponse = await _personRepository.Insert(createdPerson);

        if(insertOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository insert operation response is null";
            return response;
        }

        if (!insertOperationResponse.IsSuccessful)
        {
            response.Status = insertOperationResponse.Status;
            response.Message = insertOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }

    

    // Read
    public async Task<IResponse<ServiceSelectAllPeopleDto>> SelectAllAsync()
    {
        var response = new Response<ServiceSelectAllPeopleDto>();
        var selectAllOpertaionResponse = await _personRepository.SelectAll();

        if (selectAllOpertaionResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select all operation response is null";
            return response;
        }

        if (!selectAllOpertaionResponse.IsSuccessful)
        {
            response.Status = selectAllOpertaionResponse.Status;
            response.Message = selectAllOpertaionResponse.Message;
            return response;
        }

        if (selectAllOpertaionResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select all operation is null";
            return response;
        }

        var selectAllPeopleDto = new ServiceSelectAllPeopleDto
        {
            SelectPersonDtosList = []
        };

        foreach (var person in selectAllOpertaionResponse.Value)
        {
            var personDto = new ServiceSelectPersonDto()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalCode = person.NationalCode
            };
            selectAllPeopleDto.SelectPersonDtosList.Add(personDto);
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        response.Value = selectAllPeopleDto;
        return response;
    }

    public async Task<IResponse<ServiceSelectPersonDto>> SelectAsync(ServiceSelectPersonDto selectPersonDto)
    {
        var response = new Response<ServiceSelectPersonDto>();

        if (selectPersonDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        IResponse<Person> selectOperationResponse;

        if (selectPersonDto.NationalCode is null)
        {
            selectOperationResponse = await _personRepository.SelectById(selectPersonDto.Id);
        }
        else
        {
            selectOperationResponse = await _personRepository.SelectByNationalCode(selectPersonDto.NationalCode);
        }

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        if(selectPersonDto.NationalCode is null)
        {
            selectPersonDto.FirstName = selectOperationResponse.Value.FirstName;
            selectPersonDto.LastName = selectOperationResponse.Value.LastName;
            selectPersonDto.NationalCode = selectOperationResponse.Value.NationalCode;
        }
        else
        {
            selectPersonDto.Id = selectOperationResponse.Value.Id;
            selectPersonDto.FirstName = selectOperationResponse.Value.FirstName;
            selectPersonDto.LastName = selectOperationResponse.Value.LastName;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        response.Value = selectPersonDto;
        return response;
    }



    // Update
    public async Task<IResponse<ServiceUpdatePersonDto>> UpdateAsync(ServiceUpdatePersonDto updatePersonDto)
    {
        var response = new Response<ServiceUpdatePersonDto>();

        if (updatePersonDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var selectOperationResponse = await _personRepository.SelectById(updatePersonDto.Id);

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        var updatedPerson = selectOperationResponse.Value;
        
        // Guard : How to set ModelState for dtos to avoid update with null properties ?
        updatedPerson.FirstName = updatePersonDto.FirstName;
        updatedPerson.LastName = updatePersonDto.LastName;
        updatedPerson.NationalCode = updatePersonDto.NationalCode;

        var updateOperationResponse = await _personRepository.Update(updatedPerson);

        if (updateOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository update operation response is null";
            return response;
        }

        if (!updateOperationResponse.IsSuccessful)
        {
            response.Status = updateOperationResponse.Status;
            response.Message = updateOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }

    

    // Delete
    public async Task<IResponse<ServiceDeletePersonDto>> DeleteAsync(ServiceDeletePersonDto deletePersonDto)
    {
        var response = new Response<ServiceDeletePersonDto>();

        if (deletePersonDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var selectOperationResponse = await _personRepository.SelectById(deletePersonDto.Id);

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        var deletedPerson = selectOperationResponse.Value;

        var deleteOperationResponse = await _personRepository.Delete(deletedPerson);

        if (deleteOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository delete operation response is null";
            return response;
        }

        if (!deleteOperationResponse.IsSuccessful)
        {
            response.Status = deleteOperationResponse.Status;
            response.Message = deleteOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }



    // Save
    public async Task SaveAsync()
    {
        await _personRepository.Save();
    }
}