using OneReview.Domain;
using OneReview.Persistence.Repositories;

namespace OneReview.Services;

public class ProductsService(ProductsRepository productsRepository)
{
    private readonly ProductsRepository _productsRepository = productsRepository;

    public async Task CreateAsync(Product product)
    {
        await _productsRepository.CreateAsync(product);
    }

    public async Task<Product?> GetAsync(Guid productId)
    {
        return await _productsRepository.GetByIdAsync(productId);
    }
}