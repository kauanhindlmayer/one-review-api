namespace OneReview;

public static class ApiEndpoints
{
    private const string ApiBase = "/api";

    public static class Products
    {
        private const string Base = $"{ApiBase}/products";

        public const string Create = Base;
        public const string Get = $"{Base}/{{productId:guid}}";
        public const string Delete = $"{Base}/{{productId:guid}}";
    }

    public static class Reviews
    {
        private const string Base = $"{ApiBase}/products/{{productId:guid}}/reviews";

        public const string Create = Base;
        public const string Get = $"{Base}/{{reviewId:guid}}";
        public const string List = Base;
    }
}