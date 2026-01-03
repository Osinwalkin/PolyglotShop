namespace PolyglotShop.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // Navigation property til EF Core
    public List<Order> Orders { get; set; } = new();
}
