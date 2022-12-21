namespace CrappyApi.Endpoints;

public static class StackOverflow
{
    public static IEndpointRouteBuilder MapStackOverflowApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("stackoverflow", InfiniteRecursive).WithTags("stackoverflow");
        return endpoints;
    }

    private static IResult InfiniteRecursive(CancellationToken cancellationToken)
    {
        return InfiniteRecursive(CancellationToken.None);
    }
}