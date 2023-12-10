using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Controllers.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Frameworks.Abstracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationServiceLayerTraining.Controllers;

public class PersonController(IPersonService personService) : Controller
{
    private readonly IPersonService _personService = personService;

    public async Task<IActionResult> Index()
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
                LastName = serviceSelectDto.LastName
            };
            selectDtos.Add(selectDto);
        }

        var selectAllDto = new SelectAllPeopleDto
        {
            SelectPersonDtosList = selectDtos
        };

        return View(selectAllDto);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { Id = id };
;
        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse is null)
        {
            TempData["Index"] = "User not found";
            return RedirectToAction(nameof(Index));
        }

        var selectDto = new SelectPersonDto
        {
            Id = selectOperationResponse.Value.Id,
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };

        return View(selectDto);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto createDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { NationalCode =  createDto.NationalCode };

        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (ModelState.IsValid && selectOperationResponse.Status == Status.NotFound) //Enum should be able to check with int !
        {
            var serviceCreateDto = new ServiceCreatePersonDto    // Unreadable !!!
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                NationalCode = createDto.NationalCode,
            };

            await _personService.InsertAsync(serviceCreateDto);
            await _personService.SaveAsync();

            TempData["Index"] = $"Person \"{serviceCreateDto.FirstName} {serviceCreateDto.LastName}\" created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && selectOperationResponse.Status == Status.Successful)
        {
            TempData["Create"] = $"Person with NationalCode : {createDto.NationalCode} already exist";
            return View();
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Update(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { Id = id };

        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse.Status == Status.NotFound)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }

        var updateDto = new UpdatePersonDto
        {
            Id = selectOperationResponse.Value.Id,
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePersonDto updateDto)
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

            await _personService.UpdateAsync(serviceUpdateDto);
            await _personService.SaveAsync();

            TempData["Index"] = $"Person \"{updateDto.FirstName}" +
                                $" {updateDto.LastName}\" updated";

            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Person with NationalCode : {updateDto.NationalCode} already exist";
            return View(updateDto);
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceSelectDto = new ServiceSelectPersonDto { Id = id };
        var selectOperationResponse = await _personService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse.Status == Status.NotFound)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }

        var deleteDto = new DeletePersonDto
        {
            Id = new Guid(), 
            FirstName = selectOperationResponse.Value.FirstName,
            LastName = selectOperationResponse.Value.LastName,
            NationalCode = selectOperationResponse.Value.NationalCode
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deleteDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeletePersonDto deleteDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var serviceDeleteDto = new ServiceDeletePersonDto { Id = deleteDto.Id };
        await _personService.DeleteAsync(serviceDeleteDto);
        await _personService.SaveAsync();

        TempData["Index"] = $"Person \"{deleteDto.FirstName} {deleteDto.LastName}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}