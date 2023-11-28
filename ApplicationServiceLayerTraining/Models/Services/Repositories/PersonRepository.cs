using ApplicationServiceLayerTraining.Models.DomainModels.PersonAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServiceLayerTraining.Models.Services.Repositories;

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

    public async Task<Person> SelectById(Guid id)
    {
        try
        {
            return await _dbContext.Person.FindAsync(id);
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
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var deletedPerson = await _dbContext.Person.FindAsync(id);
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
