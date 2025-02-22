using OnlineStoreApp.Application.DTO.Customers;

namespace OnlineStoreApp.Application.DTO.CustomerPurchases;

public class GetCustomerPurchaseDto : CustomerPurchaseDto
{
    public Guid Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string ReceiptReference { get; set; }
    public GetCustomerDto Customer { get; set; }
}
