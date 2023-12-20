using System.ComponentModel.DataAnnotations;

namespace SinglePageArchitectureTraining.Models.DomainModels.ProductAggregates;

public class Product
{
    [Key]
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Title { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}