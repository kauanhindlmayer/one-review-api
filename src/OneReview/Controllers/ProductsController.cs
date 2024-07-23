using Microsoft.AspNetCore.Mvc;
using OneReview.Domain;
using OneReview.Services;

namespace OneReview.Controllers;

[ApiController]
public class ProductsController(ProductsService productsService) : ControllerBase
{
    private readonly ProductsService _productsService = productsService;

    [HttpPost(ApiEndpoints.Products.Create)]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var product = request.ToDomain();

        await _productsService.CreateAsync(product);

        return CreatedAtAction(
            actionName: nameof(GetProduct),
            routeValues: new { ProductId = product.Id },
            value: ProductResponse.FromDomain(product));
    }

    [HttpGet(ApiEndpoints.Products.Get)]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var product = await _productsService.GetAsync(productId);

        return product is null
            ? Problem(statusCode: StatusCodes.Status404NotFound, detail: "Product not found")
            : Ok(ProductResponse.FromDomain(product));
    }

    public record CreateProductRequest(string Name, string Category, string SubCategory)
    {
        public Product ToDomain()
        {
            return new Product
            {
                Name = Name,
                Category = Category,
                SubCategory = SubCategory,
            };
        }
    }

    public record ProductResponse(Guid Id, string Name, string Category, string SubCategory)
    {
        public static ProductResponse FromDomain(Product product)
        {
            return new ProductResponse(
                product.Id,
                product.Name,
                product.Category,
                product.SubCategory);
        }
    }
}