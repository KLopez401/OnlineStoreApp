namespace OnlineStoreApp.Domain.Entities;

public class CustomerPurchases
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Products { get; set; }
    public double Total { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string ReceiptReference { get; set; }
    public bool IsDeleted { get; set; }
    public Customers Customer { get; set; }
}
