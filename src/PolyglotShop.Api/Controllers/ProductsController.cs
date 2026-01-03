using Microsoft.AspNetCore.Mvc;
using PolyglotShop.Core.Entities;
using PolyglotShop.Core.Interfaces;
using System.Text.Json;

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
        // Unwrap System.Text.Json elementer så MongoDB kan læse det
        if (product.Details != null)
        {
            product.Details = SanitizeDictionary(product.Details);
        }

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

    // Helper metode
    // Konvertere raw "JsonElement" wrappers om til Strings, Ints, or Bools
    private Dictionary<string, object> SanitizeDictionary(Dictionary<string, object> source)
    {
        var result = new Dictionary<string, object>();
        foreach (var kvp in source)
        {
            if (kvp.Value is JsonElement element)
            {
                result[kvp.Key] = element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString() ?? string.Empty,
                    JsonValueKind.Number => element.TryGetInt32(out int i) ? i : element.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => element.ToString() // Fallback
                };
            }
            else
            {
                result[kvp.Key] = kvp.Value;
            }
        }
        return result;
    }
}