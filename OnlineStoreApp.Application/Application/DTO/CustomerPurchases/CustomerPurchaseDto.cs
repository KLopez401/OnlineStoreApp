using OnlineStoreApp.Application.DTO.Products;

namespace OnlineStoreApp.Application.DTO.CustomerPurchases;

public class CustomerPurchaseDto
{

    public Guid CustomerId { get; set; }
    public List<GetProductDto> Products { get; set; }
    public double Total { get; set; }
}
