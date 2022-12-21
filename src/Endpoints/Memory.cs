namespace CrappyApi.Endpoints;

public static class Memory
{
    private static readonly ConcurrentBag<Leak> cache = new();

    public static IEndpointRouteBuilder MapMemoryApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("memory/{kilobytes}", AllocMemory).WithTags("memory");
        endpoints.MapDelete("memory/{kilobytes}", FreeMemory).WithTags("memory");

        return endpoints;
    }

    private static IResult AllocMemory(int kilobytes, CancellationToken cancellationToken)
    {
        for (var i = 0; i < kilobytes; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var oneKb = new string('#', 512); // unicode: 2 bytes * 512 = 1Kb
            cache.Add(new Leak(oneKb));
        }

        return Results.Ok();
    }

    private static IResult FreeMemory(int kilobytes, CancellationToken cancellationToken)
    {
        for (var i = 0; i < kilobytes; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (cache.IsEmpty) break;
            cache.TryTake(out var leak);
        }

        return Results.Ok();
    }
}
