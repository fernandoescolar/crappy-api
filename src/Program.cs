using System.Collections.Concurrent;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (OperationCanceledException)
    {
        context.Response.StatusCode = 499;
    }
});

app.MapGet("/", () => "Welcome to crappy API!");

app.MapGet("cpu/{milliseconds}", (int milliseconds, CancellationToken cancellationToken) =>
{
    Parallel.For(0, Environment.ProcessorCount, _ =>
    {
        var sw = new Stopwatch();
        sw.Start();
        while(true)
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
});

var cache = new ConcurrentBag<Leak>();

app.MapGet("memory/{kilobytes}", (int kilobytes, CancellationToken cancellationToken) =>
{
    for (var i = 0; i < kilobytes; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var oneKb = new string('#', 512); // unicode: 2 bytes * 512 = 1Kb
        cache.Add(new Leak(oneKb));
    }

    return Results.Ok();
});

app.MapDelete("memory/{kilobytes}", (int kilobytes, CancellationToken cancellationToken) =>
{
    for (var i = 0; i < kilobytes; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (cache.IsEmpty) break;
        cache.TryTake(out var leak);
    }

    return Results.Ok();
});

app.Run();

public record Leak(string buffer);