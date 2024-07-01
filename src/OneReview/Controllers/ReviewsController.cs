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
    public IActionResult Create(Guid productId, CreateReviewRequest request)
    {
        var review = request.ToDomain(productId);

        _reviewsService.Create(review);

        return CreatedAtAction(
            actionName: nameof(Get),
            routeValues: new { productId, ReviewId = review.Id },
            value: review);
    }

    [HttpGet("{reviewId:guid}")]
    public IActionResult Get(Guid productId, Guid reviewId)
    {
        var review = _reviewsService.Get(productId, reviewId);

        return review is null
            ? Problem(detail: $"review not found (review id: {reviewId})", statusCode: StatusCodes.Status404NotFound)
            : Ok(ReviewResponse.FromDomain(review));
    }

    [HttpGet]
    public IActionResult List(Guid productId)
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
