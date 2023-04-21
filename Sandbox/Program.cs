using Dapper;
using Npgsql;

//dotnet counters monitor -p (Get-Process Sandbox).Id
Console.WriteLine($"Processor {Environment.ProcessorCount}");
ThreadPool.GetMinThreads(out var worker, out var completion);
ThreadPool.SetMaxThreads(21, 1);
Console.WriteLine($"worker {worker} - completion {completion}");
int min = int.Parse(Console.ReadLine()!);
ThreadPool.SetMinThreads(min,completion);
//dotnet counters monitor Npgsql -p (Get-Process Sandbox).Id
//await Task.WhenAll(Enumerable.Range(0, 1000).Select(Sql));
await Task.WhenAll(Enumerable.Range(0, 1000).Select(i => Task.Run(() => ThreadBusy(i))));

async static Task ThreadBusy(int i)
{
    Thread.Sleep(3000);
    Task.Delay(3000).Wait();
    Console.WriteLine($"Finished {i} on {Thread.CurrentThread.ManagedThreadId}");
}

async static Task Sql(int i)
{
    var connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=user;Password=password;" + "" +
        "Maximum Pool Size=1;Timeout=0;Application Name=Dotnet Sandbox";
    using var conn = new NpgsqlConnection(connectionString);
    var sql = "select pg_sleep(3);";
    var result = await conn.ExecuteAsync(sql);
    Console.WriteLine($"Finished {i}");
}
