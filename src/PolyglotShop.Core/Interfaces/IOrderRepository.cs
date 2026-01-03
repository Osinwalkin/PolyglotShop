using PolyglotShop.Core.Entities;

namespace PolyglotShop.Core.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderWithTransactionAsync(Order order);
}