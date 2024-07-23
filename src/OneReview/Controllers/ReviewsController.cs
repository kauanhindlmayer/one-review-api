using Microsoft.AspNetCore.Mvc;
using OneReview.Domain;
using OneReview.Services;

namespace OneReview.Controllers;

[ApiController]
public class ReviewsController(ReviewsService reviewsService) : ControllerBase
{
    private readonly ReviewsService _reviewsService = reviewsService;

    [HttpPost(ApiEndpoints.Reviews.Create)]
    public IActionResult CreateReview(Guid productId, CreateReviewRequest request)
    {
        var review = request.ToDomain(productId);

        _reviewsService.Create(review);

        return CreatedAtAction(
            actionName: nameof(GetReview),
            routeValues: new { productId, ReviewId = review.Id },
            value: review);
    }

    [HttpGet(ApiEndpoints.Reviews.Get)]
    public IActionResult GetReview(Guid productId, Guid reviewId)
    {
        var review = _reviewsService.Get(productId, reviewId);

        return review is null
            ? Problem(detail: $"review not found (review id: {reviewId})", statusCode: StatusCodes.Status404NotFound)
            : Ok(ReviewResponse.FromDomain(review));
    }

    [HttpGet(ApiEndpoints.Reviews.List)]
    public IActionResult ListReviews(Guid productId)
    {
        var reviews = _reviewsService.List(productId);

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
