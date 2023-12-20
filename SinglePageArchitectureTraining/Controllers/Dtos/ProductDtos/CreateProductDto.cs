using System.ComponentModel.DataAnnotations;

namespace SinglePageArchitectureTraining.Controllers.Dtos.ProductDtos;

public class CreateProductDto
{
    [Required(ErrorMessage = "Enter ProductCode")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Enter Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Enter Quantity")]
    public decimal Quantity { get; set; }

    [Required(ErrorMessage = "Enter UnitPrice")]
    public decimal UnitPrice { get; set; }
}
