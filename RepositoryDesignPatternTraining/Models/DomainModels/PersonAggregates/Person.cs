using System.ComponentModel.DataAnnotations;

namespace RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;

public class Person
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Enter FirstName")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Enter LastName")]
    public string LastName { get; set; }

    [RegularExpression("^\\d{10}$", ErrorMessage = "NationalCode wrong"), Required(ErrorMessage = "Enter NationalCode")]
    public string NationalCode { get; set; }
}