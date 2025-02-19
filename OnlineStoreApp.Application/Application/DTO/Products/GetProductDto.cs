namespace OnlineStoreApp.Application.DTO.Products;

public class GetProductDto : ProductDto
{
    public Guid Id { get; set; }
    public DateTime DateAdded { get; set; }
}
