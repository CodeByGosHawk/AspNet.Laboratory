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

    public IActionResult Index()
    {
        if (_personRepository is not null)
        {
            var people = _personRepository.SelectAll();
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
        else
        {
            return Problem("Entity set 'TrainingProjectDbContext.Person'  is null.");
        }
    }

    public IActionResult Details(Guid id)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = _personRepository.SelectById(id);
        if (person is not null)
        {
            var selectPersonDto = new SelectPersonDto();
            selectPersonDto.Id = person.Id;
            selectPersonDto.FirstName = person.FirstName;
            selectPersonDto.LastName = person.LastName;
            selectPersonDto.NationalCode = person.NationalCode;
            return View(selectPersonDto);
        }
        TempData["Index"] = "User not found";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(CreatePersonDto createPersonDto)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = _personRepository.SelectByNationalCode(createPersonDto.NationalCode);
        if (ModelState.IsValid && existingPerson is null)
        {
            var createdPerson = new Person();
            createdPerson.FirstName = createPersonDto.FirstName;
            createdPerson.LastName = createPersonDto.LastName;
            createdPerson.NationalCode = createPersonDto.NationalCode;
            createdPerson.Id = new Guid();

            _personRepository.Insert(createdPerson);
            _personRepository.Save();
            TempData["Index"] = "New person created";
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

    public IActionResult Update(Guid id)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = _personRepository.SelectById(id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }

        var updatePersonDto = new UpdatePersonDto();
        updatePersonDto.Id = person.Id;
        updatePersonDto.FirstName = person.FirstName;
        updatePersonDto.LastName = person.LastName;
        updatePersonDto.NationalCode = person.NationalCode;

        return View(updatePersonDto);
    }

    [HttpPost]
    public IActionResult Update(UpdatePersonDto updatePersonDto)//, Guid Id) //what is id ? 
    {
        //if (Id != person.Id) return NotFound(); // Why ????????
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = _personRepository.SelectByNationalCode(updatePersonDto.NationalCode);
        bool updateCondition = (existingPerson is not null && existingPerson.Id == updatePersonDto.Id) ||
                                existingPerson is null;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedPerson = _personRepository.SelectById(updatePersonDto.Id);

            updatedPerson.FirstName = updatePersonDto.FirstName;
            updatedPerson.LastName = updatePersonDto.LastName;
            updatedPerson.NationalCode = updatePersonDto.NationalCode;

            _personRepository.Update(updatedPerson);
            _personRepository.Save();
            TempData["Index"] = "Update Successful";
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

    public IActionResult Delete(Guid id)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");
        Console.WriteLine(id);
        var person = _personRepository.SelectById(id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }
        var deletePersonDto = new DeletePersonDto // این اصن خیلی عجیبه ! شِت !!!0
        {
            Id = new Guid(),
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deletePersonDto);
    }

    [HttpPost]
    public IActionResult Delete(DeletePersonDto deletePersonDto)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var deletedPerson = _personRepository.SelectById(deletePersonDto.Id);

        _personRepository.Delete(deletedPerson);
        _personRepository.Save();
        TempData["Index"] = "Person deleted";
        return RedirectToAction(nameof(Index));
    }
}