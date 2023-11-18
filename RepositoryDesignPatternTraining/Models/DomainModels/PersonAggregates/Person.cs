using System.ComponentModel.DataAnnotations;

namespace RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;

public class Person
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
}