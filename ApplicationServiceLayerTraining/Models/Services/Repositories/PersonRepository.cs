using ApplicationServiceLayerTraining.Frameworks;
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
    public async Task<Response<Person>> Insert(Person person)
    {
        var response = new Response<Person>();
        try
        {
            if (person is not null)
            {
                await _dbContext.AddAsync(person);
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NullRef;
                response.Message = "Person is null. operation failed.";
                return response;
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Read]
    public async Task<Response<IEnumerable<Person>>> SelectAll()
    {
        var response = new Response<IEnumerable<Person>>();
        try
        {           
            var people = await _dbContext.Person.ToListAsync();
            if(people is not null)
            {
                response.Value = new List<Person>();
                response.Value = people;
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NotExist;
                response.Message = $"Selected table is null";
                return response;
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Person>> SelectById(Guid id)
    {
        var response = new Response<Person>();
        try
        {
            var person = await _dbContext.Person.FindAsync(id);
            if(person is not null)
            {
                response.Value = new Person();
                response.Value = person;
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NotExist;
                response.Message = $"Person with Id : {id} does not exist in database";
                return response;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Person>> SelectByNationalCode(string nationalCode)
    {
        var response = new Response<Person>();
        try
        {
            var person = await _dbContext.Person.FirstOrDefaultAsync(p => p.NationalCode == nationalCode);
            if(person is not null)
            {
                response.Value = new Person();
                response.Value = person;
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NotExist;
                response.Message = $"Person with NationalCode : {nationalCode} does not exist in database";
                return response;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public async Task<Response<Person>> Update(Person person)
    {
        var response = new Response<Person>();
        try
        {
            if (person is not null)
            {
                //_dbContext.Entry(person).State = EntityState.Modified; // Bottleneck ?
                _dbContext.Update(person);
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NullRef;
                response.Message = "Person is null. operation failed.";
                return response;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Delete]
    public async Task<Response<Person>> Delete(Guid id)
    {
        var response = new Response<Person>();
        try
        {
            var deletedPerson = await _dbContext.Person.FindAsync(id);
            if (deletedPerson is not null)
            {
                _dbContext.Person.Remove(deletedPerson); // Bottleneck ? 
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NotExist;
                response.Message = $"Person with Id : {id} does not exist in database";
                return response;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<Person>> Delete(Person person)
    {
        var response = new Response<Person>();
        try
        {
            if (person is not null)
            {
                _dbContext.Person.Remove(person); // Bottleneck ?
                response.IsSuccessful = true;
                response.Status = Status.Successful;
                response.Message = "Operation successful";
                return response;
            }
            else
            {
                response.Status = Status.NullRef;
                response.Message = "Person is null. operation failed.";
                return response;
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
