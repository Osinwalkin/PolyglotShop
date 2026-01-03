using PolyglotShop.Core.Entities;
using PolyglotShop.Core.Interfaces;
using PolyglotShop.Infrastructure.Data;

namespace PolyglotShop.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _context;

    public OrderRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderWithTransactionAsync(Order order)
    {
        // U45: Bruger en Transaction for at sikre Atomicity
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Tilføj order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // (Her er hvor vi normalt caller Inventory Service 
            // eller Payment Gateway. Hvis de fejler så laver vi Rollback.)

            // 2. Commit transaction
            await transaction.CommitAsync();
            return order;
        }
        catch
        {
            // 3. Rollback på failure
            await transaction.RollbackAsync();
            throw;
        }
    }
}