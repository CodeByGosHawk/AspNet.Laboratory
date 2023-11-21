using System.ComponentModel.DataAnnotations;

namespace RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Enter ProductCode")]
    public string ProductCode { get; set; }

    [Required(ErrorMessage = "Enter Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Enter Quantity")]
    public decimal Quantity { get; set; }

    [Required(ErrorMessage = "Enter UnitPrice")]
    public decimal UnitPrice { get; set; }
}