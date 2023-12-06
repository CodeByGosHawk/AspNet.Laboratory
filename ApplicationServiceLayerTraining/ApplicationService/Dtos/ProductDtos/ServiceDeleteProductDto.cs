namespace ApplicationServiceLayerTraining.ApplicationService.Dtos.ProductDtos;

public class ServiceDeleteProductDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Title { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
