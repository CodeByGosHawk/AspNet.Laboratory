using System.ComponentModel.DataAnnotations;

namespace RepositoryDesignPatternTraining.Controllers.DTOs.PersonDTOs;

public class CreateDTO
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string NationalCode { get; set; }
}
