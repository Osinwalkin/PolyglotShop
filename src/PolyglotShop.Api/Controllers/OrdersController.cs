using Microsoft.AspNetCore.Mvc;
using PolyglotShop.Core.Entities;
using PolyglotShop.Core.Interfaces;

namespace PolyglotShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repo;

    public OrdersController(IOrderRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        try 
        {
            // Simpel logic: if UserId er 0, antag ny bruger i demoen
            if (order.UserId == 0 && order.User != null)
            {
                // EF Core insterter User automatisk på grund af relationsship
            }
            
            var result = await _repo.CreateOrderWithTransactionAsync(order);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Transaction failed: {ex.Message}");
        }
    }
}