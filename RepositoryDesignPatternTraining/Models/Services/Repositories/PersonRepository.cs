using Microsoft.EntityFrameworkCore;
using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts;
using System.Net;

namespace RepositoryDesignPatternTraining.Models.Services.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly OnlineShopDbContext _dbContext;

    public PersonRepository(OnlineShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region[Create]
    public string Insert(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Add(person);
                return "Succesfull";
            }
            else
            {
                return "person is null";
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Read]
    public IEnumerable<Person> SelectAll()
    {
        try
        {
            return _dbContext.Person;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Person SelectById(Guid Id)
    {
        try
        {
            return _dbContext.Person.Find(Id);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public string Update(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Entry(person).State = EntityState.Modified;
                return "Succesfull";
            }
            else
            {
                return "person is null";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Delete]
    public string Delete(Guid Id)
    {
        try
        {
            var deletedPerson = _dbContext.Person.Find(Id);
            if (deletedPerson is not null)
            {
                _dbContext.Person.Remove(deletedPerson);
                return "Succesfull";
            }
            else
            {
                return "person is null";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string Delete(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Person.Remove(person);
                return "Succesfull";
            }
            else
            {
                return "person is null";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Save]
    public void Save()
    {
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
