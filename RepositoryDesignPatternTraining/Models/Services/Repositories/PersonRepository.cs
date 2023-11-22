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
    public bool Insert(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Add(person);
                return true;
            }
            else
            {
                return false;
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

    public Person SelectByNationalCode(string nationalCode)
    {
        try
        {
            return _dbContext.Person.FirstOrDefault(p => p.NationalCode == nationalCode);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public bool Update(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Entry(person).State = EntityState.Modified;
                //_dbContext.Update(person);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Delete]
    public bool Delete(Guid Id)
    {
        try
        {
            var deletedPerson = _dbContext.Person.Find(Id);
            if (deletedPerson is not null)
            {
                _dbContext.Person.Remove(deletedPerson);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool Delete(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Person.Remove(person);
                return true;
            }
            else
            {
                return false;
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
