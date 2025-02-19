using OnlineStoreApp.Application.Application.DTO.Products;

namespace OnlineStoreApp.Application.DTO.CustomerPurchases;

public class CustomerPurchaseDto
{

    public Guid CustomerId { get; set; }
    public List<ProductInfoDto> Products { get; set; }
    public double Total { get; set; }
}
