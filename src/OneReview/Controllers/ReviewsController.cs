using Microsoft.AspNetCore.Mvc;
using OneReview.Domain;
using OneReview.Services;

namespace OneReview.Controllers;

[ApiController]
[Route("products/{productId:guid}/[controller]")]
public class ReviewsController(ReviewsService reviewsService) : ControllerBase
{
    private readonly ReviewsService _reviewsService = reviewsService;

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        Guid productId,
        CreateReviewRequest request)
    {
        var review = request.ToDomain(productId);

        await _reviewsService.CreateAsync(review);

        return CreatedAtAction(
            actionName: nameof(GetReview),
            routeValues: new { productId, ReviewId = review.Id },
            value: review);
    }

    [HttpGet("{reviewId:guid}")]
    public async Task<IActionResult> GetReview(Guid productId, Guid reviewId)
    {
        var review = await _reviewsService.GetAsync(productId, reviewId);

        return review is null
            ? Problem(
                statusCode: StatusCodes.Status404NotFound,
                detail: $"Review not found (review id: {reviewId})")
            : Ok(ReviewResponse.FromDomain(review));
    }

    [HttpGet]
    public async Task<IActionResult> ListReviews(Guid productId)
    {
        var reviews = await _reviewsService.ListAsync(productId);

        var response = reviews
            .ToList()
            .ConvertAll(ReviewResponse.FromDomain);

        return Ok(response);
    }
}

public record CreateReviewRequest(int Rating, string Text)
{
    public Review ToDomain(Guid productId) => new()
    {
        Rating = Rating,
        Text = Text,
        ProductId = productId
    };
}

public record ReviewResponse(Guid Id, int Rating, string Text)
{
    public static ReviewResponse FromDomain(Review review) => new(
        review.Id,
        review.Rating,
        review.Text);
}
