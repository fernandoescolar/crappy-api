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
app.MapCpuApi()
   .MapMemoryApi()
   .MapDeadlockApi()
   .MapStackOverflowApi();

app.Run();

public record Leak(string buffer);