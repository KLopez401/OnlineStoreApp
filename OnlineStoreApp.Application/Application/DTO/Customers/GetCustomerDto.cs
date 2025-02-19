namespace OnlineStoreApp.Application.DTO.Customers;

public class GetCustomerDto : CustomerDto
{
    public Guid Id { get; set; }
    public DateTime DateAdded { get; set; }
}
