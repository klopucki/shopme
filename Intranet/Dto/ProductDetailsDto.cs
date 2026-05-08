namespace Intranet.Dto;

public class ProductDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    // product details
    public string Description { get; set; }

    public decimal Weight { get; set; }

    public List<ProductImageDto> Images { get; set; }
}