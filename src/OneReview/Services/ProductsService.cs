using OneReview.Domain;
using OneReview.Errors;
using OneReview.Persistence.Repositories;

namespace OneReview.Services;

public class ProductsService(ProductsRepository productsRepository)
{
    private readonly ProductsRepository _productsRepository = productsRepository;

    public async Task CreateAsync(Product product)
    {
        if (await _productsRepository.ExistsAsync(product.Id))
        {
            throw new NotFoundException($"Product with ID {product.Id} already exists");
        }

        await _productsRepository.CreateAsync(product);
    }

    public async Task<Product?> GetAsync(Guid productId)
    {
        var product = await _productsRepository.GetByIdAsync(productId);

        return product is null
            ? throw new NotFoundException($"Product with ID {productId} not found")
            : product;
    }
}