namespace OneReview.Domain;

public class Review
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required Guid ProductId { get; init; }

    public required int Rating { get; init; }

    public required string Text { get; init; }
}