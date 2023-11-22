using Microsoft.AspNetCore.Mvc;
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
            return View(people);
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
        if (person is not null) return View(person);
        TempData["Index"] = "User not found";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Person person)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var existingPerson = _personRepository.SelectByNationalCode(person.NationalCode);
        if (ModelState.IsValid && existingPerson is null)
        {
            _personRepository.Insert(person);
            _personRepository.Save();
            TempData["Index"] = "New person created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && existingPerson is not null)
        {
            TempData["Create"] = $"Person with NationalCode : {person.NationalCode} already exist";
            return View();
        }
        else
        {
            return View();
        }
    }

    public IActionResult Update(Guid Id)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = _personRepository.SelectById(Id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Person person)//, Guid Id) //what is id ? 
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        //if (Id != person.Id) return NotFound(); // Why ???????? 
        var existingPerson = _personRepository.SelectByNationalCode(person.NationalCode);
        bool updateCondition = (existingPerson is not null && existingPerson.Id == person.Id) ||
                                existingPerson is null;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedPerson = _personRepository.SelectById(person.Id);
            updatedPerson.FirstName = person.FirstName;
            updatedPerson.LastName = person.LastName;
            updatedPerson.NationalCode = person.NationalCode;
            _personRepository.Update(updatedPerson);
            _personRepository.Save();
            TempData["Index"] = "Update Successful";
            return RedirectToAction(nameof(Index));
        }
        else if(ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Person with NationalCode : {person.NationalCode} already exist";
            return View(person);
        }
        else
        {
            return View();
        }
    }

    public IActionResult Delete(Guid Id)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        var person = _personRepository.SelectById(Id);
        if (person is null)
        {
            TempData["Index"] = "Person does not exist";
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    [HttpPost]
    public IActionResult Delete(Person person)
    {
        if (_personRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Person' is null.");

        _personRepository.Delete(person);
        _personRepository.Save();

        TempData["Index"] = "Person deleted";
        return RedirectToAction(nameof(Index));
    }
}