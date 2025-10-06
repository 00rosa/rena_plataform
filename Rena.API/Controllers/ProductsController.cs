using Microsoft.AspNetCore.Mvc;
using Rena.Application.DTOs.Products;
using Rena.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rena.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los productos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Producto con ID {id} no encontrado");
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener producto con ID {ProductId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByUser(Guid userId)
    {
        try
        {
            var products = await _productService.GetProductsByUserAsync(userId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos del usuario {UserId}", userId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // ✅ NUEVO ENDPOINT - Contar productos por usuario
    [HttpGet("user/{userId}/count")]
    public async Task<ActionResult<int>> GetUserProductsCount(Guid userId)
    {
        try
        {
            var products = await _productService.GetProductsByUserAsync(userId);
            return Ok(products.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al contar productos del usuario {UserId}", userId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // ✅ NUEVO ENDPOINT - Productos por estado
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByStatus(int status)
    {
        try
        {
            var products = await _productService.GetProductsByUserAsync(Guid.NewGuid()); // Temporal
            var filteredProducts = products.Where(p => (int)p.Status == status);
            return Ok(filteredProducts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por estado {Status}", status);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(
        [FromQuery] string q,
        [FromQuery] Guid? categoryId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("El término de búsqueda no puede estar vacío");
            }

            var products = await _productService.SearchProductsAsync(q, categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar productos con término {SearchTerm}", q);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TEMPORAL: Simular usuario autenticado (en producción usarías JWT)
            var temporaryUserId = Guid.NewGuid(); // Cambiar por ID real de usuario autenticado

            var product = await _productService.CreateProductAsync(createProductDto, temporaryUserId);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductDto updateProductDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TEMPORAL: Simular usuario autenticado
            var temporaryUserId = Guid.NewGuid(); // Cambiar por ID real

            var product = await _productService.UpdateProductAsync(id, updateProductDto, temporaryUserId);

            return Ok(product);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar producto con ID {ProductId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // ✅ NUEVO ENDPOINT - Actualizar estado del producto
    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateProductStatus(
        Guid id,
        [FromBody] UpdateProductStatusDto statusDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temporaryUserId = Guid.NewGuid(); // Cambiar por usuario real después
            var result = await _productService.ToggleProductStatusAsync(
                id, temporaryUserId, statusDto.NewStatus);

            if (!result)
                return NotFound("Producto no encontrado o no autorizado");

            return Ok(new { message = "Estado actualizado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado del producto {ProductId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        try
        {
            // TEMPORAL: Simular usuario autenticado
            var temporaryUserId = Guid.NewGuid(); // Cambiar por ID real

            var result = await _productService.DeleteProductAsync(id, temporaryUserId);

            if (!result)
            {
                return NotFound($"Producto con ID {id} no encontrado o no autorizado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar producto con ID {ProductId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

// ✅ NUEVO DTO - Agregar esta clase en el mismo archivo o crear archivo separado
public class UpdateProductStatusDto
{
    [Required]
    public Rena.Domain.Enums.ProductStatus NewStatus { get; set; }
}