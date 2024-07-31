using System.Data;
using Dapper;
using OneReview.Domain;
using OneReview.Persistence.Database;
using Throw;

namespace OneReview.Persistence.Repositories;

public class ReviewsRepository(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task CreateAsync(Review review)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            INSERT INTO Reviews (id, product_id, rating, text)
            VALUES (@Id, @ProductId, @Rating, @Text)";

        var result = await connection.ExecuteAsync(query, review);

        result.Throw().IfNegativeOrZero();
    }

    public async Task<Review?> GetByIdAsync(Guid productId, Guid reviewId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT id, product_id AS ProductId, rating, text
            FROM reviews
            WHERE id = @ReviewId AND product_id = @ProductId";

        return await connection.QueryFirstOrDefaultAsync<Review>(
            query,
            new { ReviewId = reviewId, ProductId = productId });
    }

    public async Task<IReadOnlyList<Review>> ListAsync(Guid productId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT id, product_id AS ProductId, rating, text
            FROM reviews
            WHERE product_id = @ProductId";

        return (await connection.QueryAsync<Review>(
            query,
            new { ProductId = productId })).ToList();
    }

    public async Task<bool> ExistsAsync(Guid productId, Guid reviewId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT COUNT(*)
            FROM reviews
            WHERE id = @ReviewId AND product_id = @ProductId";

        var count = await connection.ExecuteScalarAsync<int>(
            query,
            new { ReviewId = reviewId, ProductId = productId });

        return count > 0;
    }
}