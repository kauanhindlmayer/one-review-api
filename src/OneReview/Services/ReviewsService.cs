using OneReview.Domain;
using OneReview.Errors;
using OneReview.Persistence.Repositories;

namespace OneReview.Services;

public class ReviewsService(ReviewsRepository reviewsRepository)
{
    private readonly ReviewsRepository ReviewsRepository = reviewsRepository;

    public async Task CreateAsync(Review review)
    {
        if (await ReviewsRepository.ExistsAsync(review.ProductId, review.Id))
        {
            throw new NotFoundException($"Review with ID {review.Id} already exists");
        }

        await ReviewsRepository.CreateAsync(review);
    }

    public async Task<Review?> GetAsync(Guid productId, Guid reviewId)
    {
        var review = await ReviewsRepository.GetByIdAsync(productId, reviewId);

        return review is null
            ? throw new NotFoundException($"Review with ID {reviewId} not found")
            : review;
    }

    public async Task<IReadOnlyList<Review>> ListAsync(Guid productId)
    {
        return await ReviewsRepository.ListAsync(productId);
    }
}