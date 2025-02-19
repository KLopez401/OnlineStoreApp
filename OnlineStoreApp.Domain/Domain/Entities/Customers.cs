namespace OnlineStoreApp.Domain.Entities;

public class Customers
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<CustomerPurchases> CustomerPurchases { get; set; } // Navigation Property
}
