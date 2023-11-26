using Microsoft.EntityFrameworkCore;
using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts;
using RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;
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
    public async Task<bool> Insert(Person person)
    {
        try
        {
            if (person is not null)
            {
                await _dbContext.AddAsync(person);
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
    public async Task<IEnumerable<Person>> SelectAll()
    {
        try
        {
            return await _dbContext.Person.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Person> SelectById(Guid Id)
    {
        try
        {
            return await _dbContext.Person.FindAsync(Id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Person> SelectByNationalCode(string nationalCode)
    {
        try
        {
            return await _dbContext.Person.FirstOrDefaultAsync(p => p.NationalCode == nationalCode);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public async Task<bool> Update(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Entry(person).State = EntityState.Modified; // Bottleneck ?
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
    public async Task<bool> Delete(Guid Id)
    {
        try
        {
            var deletedPerson = await _dbContext.Person.FindAsync(Id);
            if (deletedPerson is not null)
            {
                _dbContext.Person.Remove(deletedPerson); // Bottleneck ? 
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

    public async Task<bool> Delete(Person person)
    {
        try
        {
            if (person is not null)
            {
                _dbContext.Person.Remove(person); // Bottleneck ?
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
    public async Task Save()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
