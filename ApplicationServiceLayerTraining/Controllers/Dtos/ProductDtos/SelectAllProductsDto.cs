namespace ApplicationServiceLayerTraining.Controllers.Dtos.ProductDtos;

public class SelectAllProductsDto
{
    public IEnumerable<SelectProductDto> SelectProductDtosList { get; set; }
}
