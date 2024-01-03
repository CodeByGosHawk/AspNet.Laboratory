using Microsoft.AspNetCore.Mvc;
using SinglePageArchitectureTraining.ApplicationService.Contracts;
using SinglePageArchitectureTraining.ApplicationService.Dtos.PersonDtos;
using SinglePageArchitectureTraining.Controllers.Dtos.PersonDtos;
using SinglePageArchitectureTraining.Frameworks.Enums;

namespace ApplicationServiceLayerTraining.Controllers;

public class PersonController(IPersonService personService) : Controller
{
    private readonly IPersonService _personService = personService;


    public IActionResult Person()
    {
        return View();
    }

    public async Task<IActionResult> GetPeople()
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var selectAllOperationResponse = await _personService.SelectAllAsync();

        var selectDtos = new List<SelectPersonDto>();

        foreach (var serviceSelectDto in selectAllOperationResponse.Value.SelectPersonDtosList)
        {
            SelectPersonDto selectDto = new()
            {
                Id = serviceSelectDto.Id,
                FirstName = serviceSelectDto.FirstName,
                LastName = serviceSelectDto.LastName,
                NationalCode = serviceSelectDto.NationalCode
            };
            selectDtos.Add(selectDto);
        }

        var selectAllDto = new SelectAllPersonsDto
        {
            SelectPersonDtosList = selectDtos
        };

        return Json(selectAllDto);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { Id = id };
        ;
        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse is null)
        {
            return Json("NotFound");
        }

        var selectDto = new SelectPersonDto
        {
            Id = selectOperationResponse.Value.Id,
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };

        return Json(selectDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto createDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { NationalCode = createDto.NationalCode };

        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (ModelState.IsValid && selectOperationResponse.Status == Status.NotFound) //Enum should be able to check with int !
        {
            var serviceCreateDto = new ServiceCreatePersonDto    // Unreadable !!!
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                NationalCode = createDto.NationalCode,
            };

            var insertOperationResponse = await _personService.InsertAsync(serviceCreateDto);
            await _personService.SaveAsync();

            return insertOperationResponse.IsSuccessful ? Ok() : BadRequest();

        }
        else if (ModelState.IsValid && selectOperationResponse.Status == Status.Successful)
        {
            return Conflict();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdatePersonDto updateDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { NationalCode = updateDto.NationalCode };

        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        bool updateCondition = (selectOperationResponse.Status == Status.Successful && selectOperationResponse.Value.Id == updateDto.Id) ||
                                selectOperationResponse.Status == Status.NotFound;

        if (ModelState.IsValid && updateCondition)
        {
            var serviceUpdateDto = new ServiceUpdatePersonDto
            {
                Id = updateDto.Id,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                NationalCode = updateDto.NationalCode,
            };

            var updateOpertaionResponse = await _personService.UpdateAsync(serviceUpdateDto);
            await _personService.SaveAsync();

            return updateOpertaionResponse.IsSuccessful ? Ok() : BadRequest();
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            return Conflict();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeletePersonDto deleteDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceDeleteDto = new ServiceDeletePersonDto { Id = deleteDto.Id };
        var deleteOperationResponse = await _personService.DeleteAsync(serviceDeleteDto);
        await _personService.SaveAsync();

        return deleteOperationResponse.IsSuccessful ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSelected([FromBody] DeleteSelectedPersonsDto deleteSelectedDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        foreach (var deleteDto in deleteSelectedDto.DeletePersonDtosList)
        {
            var serviceDeleteDto = new ServiceDeletePersonDto { Id = deleteDto.Id };
            await _personService.DeleteAsync(serviceDeleteDto);
        }
        await _personService.SaveAsync();

        return Ok();
    }
}