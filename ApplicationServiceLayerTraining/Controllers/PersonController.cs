using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.PersonDtos;
using ApplicationServiceLayerTraining.Controllers.Dtos.PersonDtos;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationServiceLayerTraining.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<IActionResult> Index()
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var people = await _personService.SelectAllAsync();
        var selectPeopleDtos = new List<SelectPeopleDto>();

        foreach (var person in people)
        {
            SelectPeopleDto selectPeopleDto = new SelectPeopleDto()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
            selectPeopleDtos.Add(selectPeopleDto);
        }

        return View(selectPeopleDtos);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = await _personService.SelectByIdAsync(id);
        if (person is not null)
        {
            var selectPersonDto = new SelectPersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                NationalCode = person.NationalCode
            };
            return View(selectPersonDto);
        }
        TempData["Index"] = "User not found";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto createPersonDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = await _personService.SelectByNationalCodeAsync(createPersonDto.NationalCode);
        if (ModelState.IsValid && existingPerson is null)
        {
            var serviceCreatePersonDto = new ServiceCreatePersonDto    /// Unreadable !!!
            {
                FirstName = createPersonDto.FirstName,
                LastName = createPersonDto.LastName,
                NationalCode = createPersonDto.NationalCode,
            };

            await _personService.InsertAsync(serviceCreatePersonDto);
            await _personService.SaveAsync();
            TempData["Index"] = $"Person \"{serviceCreatePersonDto.FirstName} {serviceCreatePersonDto.LastName}\" created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && existingPerson is not null)
        {
            TempData["Create"] = $"Person with NationalCode : {createPersonDto.NationalCode} already exist";
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

        var person = await _personService.SelectByIdAsync(id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }

        var updatePersonDto = new UpdatePersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode
        };

        return View(updatePersonDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePersonDto updatePersonDto)//, Guid Id) //what is id ? 
    {
        //if (Id != person.Id) return NotFound(); // Why ????????
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = await _personService.SelectByNationalCodeAsync(updatePersonDto.NationalCode);
        bool updateCondition = (existingPerson is not null && existingPerson.Id == updatePersonDto.Id) ||
                                existingPerson is null;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedPerson = await _personService.SelectByIdAsync(updatePersonDto.Id);

            var serviceUpdatePersonDto = new ServiceUpdatePersonDto  /// Unreadable !!!!!
            {
                Id = updatePersonDto.Id,
                FirstName = updatePersonDto.FirstName,
                LastName = updatePersonDto.LastName,
                NationalCode = updatePersonDto.NationalCode,
            };

            await _personService.UpdateAsync(serviceUpdatePersonDto);
            await _personService.SaveAsync();
            TempData["Index"] = $"Person \"{updatedPerson.FirstName} {updatedPerson.LastName}\" updated";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Person with NationalCode : {updatePersonDto.NationalCode} already exist";
            return View(updatePersonDto);
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");
        Console.WriteLine(id);
        var person = await _personService.SelectByIdAsync(id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }
        var deletePersonDto = new DeletePersonDto // این اصن خیلی عجیبه ! شِت !!!0
        {
            Id = new Guid(), // من اینجا آیدی جدید بهش دادم ولی باز قبلی رو شناسایی می کنه پاکش میکنه !0
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deletePersonDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeletePersonDto deletePersonDto)
    {
        if (_personService is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var deletedPerson = await _personService.SelectByIdAsync(deletePersonDto.Id);
        await _personService.DeleteByIdAsync(deletePersonDto.Id);
        await _personService.SaveAsync();
        TempData["Index"] = $"Person \"{deletedPerson.FirstName} {deletedPerson.LastName}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}