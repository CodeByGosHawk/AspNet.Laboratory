using Microsoft.AspNetCore.Mvc;
using RepositoryDesignPatternTraining.Controllers.Dtos.PersonDtos;
using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts;

namespace RepositoryDesignPatternTraining.Controllers;

public class PersonController : Controller
{
    private readonly IPersonRepository _personRepository;

    public PersonController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IActionResult> Index()
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var people = await _personRepository.SelectAll();
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = await _personRepository.SelectById(id);
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = await _personRepository.SelectByNationalCode(createPersonDto.NationalCode);
        if (ModelState.IsValid && existingPerson is null)
        {
            var createdPerson = new Person
            {
                FirstName = createPersonDto.FirstName,
                LastName = createPersonDto.LastName,
                NationalCode = createPersonDto.NationalCode,
                Id = new Guid()
            };

            await _personRepository.Insert(createdPerson);
            await _personRepository.Save();
            TempData["Index"] = $"Person \"{createdPerson.FirstName} {createdPerson.LastName}\" created";
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = await _personRepository.SelectById(id);
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = await _personRepository.SelectByNationalCode(updatePersonDto.NationalCode);
        bool updateCondition = (existingPerson is not null && existingPerson.Id == updatePersonDto.Id) ||
                                existingPerson is null;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedPerson = await _personRepository.SelectById(updatePersonDto.Id);

            updatedPerson.FirstName = updatePersonDto.FirstName;
            updatedPerson.LastName = updatePersonDto.LastName;
            updatedPerson.NationalCode = updatePersonDto.NationalCode;

            await _personRepository.Update(updatedPerson);
            await _personRepository.Save();
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");
        Console.WriteLine(id);
        var person = await _personRepository.SelectById(id);
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
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var deletedPerson = await _personRepository.SelectById(deletePersonDto.Id);

        await _personRepository.Delete(deletedPerson);
        await _personRepository.Save();
        TempData["Index"] = $"Person \"{deletedPerson.FirstName} {deletedPerson.LastName}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}