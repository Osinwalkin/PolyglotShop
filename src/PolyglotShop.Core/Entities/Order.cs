namespace PolyglotShop.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    // Foreign Key
    public int UserId { get; set; }
    public User? User { get; set; }
}