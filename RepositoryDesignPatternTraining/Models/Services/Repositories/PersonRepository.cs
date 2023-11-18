using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
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
    public void Insert(Person obj)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region[Read]
    public IEnumerable<Person> SelectAll()
    {
        throw new NotImplementedException();
    }

    public Person SelectById(Guid Id)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region[Update]
    public void Update(Person obj)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region[Delete]
    public void Delete(Guid Id)
    {
        throw new NotImplementedException();
    }

    public void Delete(Person obj)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region[Save]
    public void Save()
    {
        throw new NotImplementedException();
    }
    #endregion
}
