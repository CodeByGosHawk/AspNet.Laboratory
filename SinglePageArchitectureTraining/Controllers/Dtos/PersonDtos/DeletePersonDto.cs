using System.ComponentModel.DataAnnotations;

namespace SinglePageArchitectureTraining.Controllers.Dtos.PersonDtos;

public class DeletePersonDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string NationalCode { get; set; }
}
