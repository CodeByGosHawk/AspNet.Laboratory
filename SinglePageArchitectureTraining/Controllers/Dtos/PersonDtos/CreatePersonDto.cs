﻿using System.ComponentModel.DataAnnotations;

namespace SinglePageArchitectureTraining.Controllers.Dtos.PersonDtos;

public class CreatePersonDto
{
    [Required(ErrorMessage = "Enter FirstName")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Enter LastName")]
    public string LastName { get; set; }

    [RegularExpression("^\\d{10}$", ErrorMessage = "NationalCode wrong, must be 10 digits"), Required(ErrorMessage = "Enter NationalCode")]
    public string NationalCode { get; set; }
}
