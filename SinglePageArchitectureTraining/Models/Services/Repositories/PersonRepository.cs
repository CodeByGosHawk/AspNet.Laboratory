using SinglePageArchitectureTraining.Frameworks;
using SinglePageArchitectureTraining.Frameworks.Contracts;
using SinglePageArchitectureTraining.Models.DomainModels.PersonAggregates;
using SinglePageArchitectureTraining.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using SinglePageArchitectureTraining.Frameworks.Enums;

namespace SinglePageArchitectureTraining.Models.Services.Repositories;

public class PersonRepository(OnlineShopDbContext dbContext) : IPersonRepository
{
    private readonly OnlineShopDbContext _dbContext = dbContext;


    // Create
    public async Task<IResponse<Person>> Insert(Person insertedPerson)
    {
        var response = new Response<Person>();
        try
        {
            if (insertedPerson is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            await _dbContext.AddAsync(insertedPerson);
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Read
    public async Task<IResponse<IEnumerable<Person>>> SelectAll()
    {
        var response = new Response<IEnumerable<Person>>();
        try
        {           
            var people = await _dbContext.Person.AsNoTracking().ToListAsync();

            if(people is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Selected table was not found";
                return response;
            }

            response.Value = people;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponse<Person>> SelectById(Guid id)
    {
        var response = new Response<Person>();
        try
        {
            var selectedPerson = await _dbContext.Person.FindAsync(id);

            if (selectedPerson is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Person with Id : {id} does not exist in database";
                return response;
            }

            _dbContext.Entry(selectedPerson).State = EntityState.Detached; // Ef has no FindAsync with AsNoTracking
            response.Value = selectedPerson;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponse<Person>> SelectByNationalCode(string nationalCode)
    {
        var response = new Response<Person>();
        try
        {
            if(nationalCode is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            var selectedPerson = await _dbContext.Person.AsNoTracking().FirstOrDefaultAsync(p => p.NationalCode == nationalCode);

            if(selectedPerson is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Person with NationalCode : {nationalCode} does not exist in database";
                return response;
            }

            response.Value = selectedPerson;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Update
    public async Task<IResponse<Person>> Update(Person updatedPerson)
    {
        var response = new Response<Person>();
        try
        {
            if (updatedPerson is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            //_dbContext.Entry(person).State = EntityState.Modified; // Bottleneck ?
            _dbContext.Update(updatedPerson);
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Delete
    public async Task<IResponse<Person>> Delete(Person deletedPerson)
    {
        var response = new Response<Person>();
        try
        {
            if (deletedPerson is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            _dbContext.Person.Remove(deletedPerson); // Bottleneck ?
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Save
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
}
