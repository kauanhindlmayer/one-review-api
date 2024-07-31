using System.Data;
using Dapper;
using OneReview.Domain;
using OneReview.Persistence.Database;
using Throw;

namespace OneReview.Persistence.Repositories;

public class ProductsRepository(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task CreateAsync(Product product)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            INSERT INTO Products (id, name, category, sub_category)
            VALUES (@Id, @Name, @Category, @SubCategory)";

        var result = await connection.ExecuteAsync(query, product);

        result.Throw().IfNegativeOrZero();
    }

    public async Task<Product?> GetByIdAsync(Guid productId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT id, name, category, sub_category AS SubCategory
            FROM products
            WHERE id = @ProductId";

        return await connection.QueryFirstOrDefaultAsync<Product>(
            query,
            new { ProductId = productId });
    }

    public async Task<bool> ExistsAsync(Guid productId)
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        string query = @"
            SELECT COUNT(*)
            FROM products
            WHERE id = @ProductId";

        var count = await connection.ExecuteScalarAsync<int>(
            query,
            new { ProductId = productId });

        return count > 0;
    }
}