using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Application.Dtos;
using SmartStock.Application.Features.Products.Commands;
using SmartStock.Application.Features.Products.Queries;
using SmartStock.Domain.Repositories;

namespace SmartStock.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository productRepository, IMediator mediator, ILogger<ProductsController> logger)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] bool lowStock = false, [FromQuery] int page = 1)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetAllProductsQuery
            {
                UserId = userId,
                Search = search,
                Category = category,
                LowStockOnly = lowStock,
                Page = page
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "LIST_FAILED", message = ex.Message } });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetProductByIdQuery { ProductId = id, UserId = userId };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
            
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
        }
    }

    [HttpGet("by-barcode/{barcode}")]
    public async Task<ActionResult<ProductDto>> GetByBarcode(string barcode)
    {
        try
        {
            var userId = GetUserId();
            var query = new GetProductByBarcodeQuery { Barcode = barcode, UserId = userId };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
            
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        try
        {
            var userId = GetUserId();
            var command = new CreateProductCommand
            {
                UserId = userId,
                Name = request.Name,
                Sku = request.Sku,
                Barcode = request.Barcode,
                Category = request.Category,
                Description = request.Description,
                CostPrice = request.CostPrice,
                SellingPrice = request.SellingPrice,
                StockQuantity = request.StockQuantity,
                LowStockThreshold = request.LowStockThreshold,
                Unit = request.Unit,
                ImageUrl = request.ImageUrl
            };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = new { code = "CREATE_FAILED", message = ex.Message } });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var userId = GetUserId();
            var command = new UpdateProductCommand
            {
                Id = id,
                UserId = userId,
                Name = request.Name,
                Sku = request.Sku,
                Barcode = request.Barcode,
                Category = request.Category,
                Description = request.Description,
                CostPrice = request.CostPrice,
                SellingPrice = request.SellingPrice,
                StockQuantity = request.StockQuantity,
                LowStockThreshold = request.LowStockThreshold,
                Unit = request.Unit,
                ImageUrl = request.ImageUrl
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
        }
    }

    [HttpPatch("{id}/stock")]
    public async Task<IActionResult> AdjustStock(Guid id, [FromBody] StockAdjustmentRequest request)
    {
        try
        {
            var userId = GetUserId();
            var command = new AdjustStockCommand
            {
                ProductId = id,
                UserId = userId,
                Adjustment = request.Adjustment
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = new { code = "STOCK_ADJUSTMENT_FAILED", message = ex.Message } });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var command = new DeleteProductCommand { ProductId = id, UserId = userId };
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound(new { error = new { code = "PRODUCT_NOT_FOUND", message = "Product not found." } });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
    }
}
