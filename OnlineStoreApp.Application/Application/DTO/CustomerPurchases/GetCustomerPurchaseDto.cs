namespace OnlineStoreApp.Application.DTO.CustomerPurchases;

public class GetCustomerPurchaseDto : CustomerPurchaseDto
{
    public Guid Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string ReceiptReference { get; set; }
}
