namespace CrappyApi.Endpoints;

public static class Deadlock
{
    private static readonly object _lock1 = new();
    private static readonly object _lock2 = new();

    public static IEndpointRouteBuilder MapDeadlockApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("deadlock", LockThread).WithTags("deadlock");
        return endpoints;
    }

    private static IResult LockThread(CancellationToken cancellationToken)
    {
        var task1 = Task.Run(() =>
        {
            lock (_lock1)
            {
                Thread.Sleep(1000);
                lock (_lock2)
                {
                    Thread.Sleep(1000);
                }
            }
        }, cancellationToken);

        var task2 = Task.Run(() =>
         {
             lock (_lock2)
             {
                 Thread.Sleep(1000);
                 lock (_lock1)
                 {
                     Thread.Sleep(1000);
                 }
             }
         }, cancellationToken);

        Task.WaitAll(task1, task2);

        return Results.Ok();
    }
}