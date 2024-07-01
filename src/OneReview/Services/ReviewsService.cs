using OneReview.Domain;

namespace OneReview.Services;

public class ReviewsService
{
    private static readonly List<Review> ReviewsRepository = [];

    public void Create(Review review)
    {
        ReviewsRepository.Add(review);
    }

    public Review? Get(Guid productId, Guid reviewId)
    {
        return ReviewsRepository.Find(x => x.Id == reviewId);
    }

    internal IReadOnlyList<Review> List(Guid productId)
    {
        return ReviewsRepository
            .Where(x => x.ProductId == productId)
            .ToList();
    }
}