namespace CrappyApi.Endpoints;

public static class Cpu
{
    public static IEndpointRouteBuilder MapCpuApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("cpu/{milliseconds}", ConsumeCpu).WithTags("cpu");
        return endpoints;
    }

    private static IResult ConsumeCpu(int milliseconds, CancellationToken cancellationToken)
    {
        Parallel.For(0, Environment.ProcessorCount, _ =>
        {
            var sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (sw.ElapsedMilliseconds >= milliseconds)
                {
                    sw.Stop();
                    break;
                }
            }
        });

        return Results.Ok();
    }
}
