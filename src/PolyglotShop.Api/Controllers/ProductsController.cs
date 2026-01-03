using Microsoft.AspNetCore.Mvc;
using PolyglotShop.Core.Entities;
using PolyglotShop.Core.Interfaces;

namespace PolyglotShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _repo.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product is not null ? Ok(product) : NotFound();
    }
    
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAll() => await _repo.GetAllAsync();
}