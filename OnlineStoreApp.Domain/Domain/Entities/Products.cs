namespace OnlineStoreApp.Domain.Entities;

public class Products
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsDeleted { get; set; }
}
