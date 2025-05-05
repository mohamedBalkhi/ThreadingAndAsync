using System.Diagnostics;

namespace CAAsyncWebClient;

public class Program
{
    private static readonly HttpClient HttpClient = new HttpClient();
    private static readonly string[] Apis =
    [
        "https://jsonplaceholder.typicode.com/posts",
        "https://jsonplaceholder.typicode.com/comments",
        "https://jsonplaceholder.typicode.com/albums",
        "https://jsonplaceholder.typicode.com/photos",
        "https://jsonplaceholder.typicode.com/todos",
        "https://jsonplaceholder.typicode.com/users"
    ];

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Demonstrating different approaches to fetch data from multiple APIs");

        // Sequential approach
        await RunSequentialFetchDemo();

        // Concurrent approach with Task.WhenAll
        await RunConcurrentFetchDemo();

        // Concurrent approach with limited concurrency
        await RunLimitedConcurrencyDemo();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    private static async Task RunSequentialFetchDemo()
    {
        Console.WriteLine("\n1. Sequential API calls (one after another):");
        var stopwatch = Stopwatch.StartNew();

        var results = new List<string>();
        foreach (var api in Apis)
        {
            var result = await FetchDataFromApiAsync(api);
            results.Add($"{api}: {result.Length} bytes");
        }

        stopwatch.Stop();
        Console.WriteLine($"Sequential execution completed in {stopwatch.ElapsedMilliseconds}ms");
        foreach (var result in results)
        {
            Console.WriteLine($"  {result}");
        }
    }

    private static async Task RunConcurrentFetchDemo()
    {
        Console.WriteLine("\n2. Concurrent API calls (all at once with Task.WhenAll):");
        var stopwatch = Stopwatch.StartNew();

        var tasks = new List<Task<(string api, string data)>>();
        foreach (var api in Apis)
        {
            tasks.Add(FetchDataWithSourceAsync(api));
        }

        var results = await Task.WhenAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Concurrent execution completed in {stopwatch.ElapsedMilliseconds}ms");
        foreach (var (api, data) in results)
        {
            Console.WriteLine($"  {api}: {data.Length} bytes");
        }
    }

    private static async Task RunLimitedConcurrencyDemo()
    {
        Console.WriteLine("\n3. Concurrent API calls with limited concurrency (max 3 concurrent calls):");
        var stopwatch = Stopwatch.StartNew();

        // Using SemaphoreSlim to limit concurrency
        using var semaphore = new SemaphoreSlim(3);
        var tasks = new List<Task<(string api, string data)>>();

        foreach (var api in Apis)
        {
            tasks.Add(FetchWithLimitedConcurrencyAsync(api, semaphore));
        }

        var results = await Task.WhenAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Limited concurrency execution completed in {stopwatch.ElapsedMilliseconds}ms");
        foreach (var (api, data) in results)
        {
            Console.WriteLine($"  {api}: {data.Length} bytes");
        }
    }

    private static async Task<string> FetchDataFromApiAsync(string url)
    {
        try
        {
            return await HttpClient.GetStringAsync(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching {url}: {ex.Message}");
            return string.Empty;
        }
    }

    private static async Task<(string api, string data)> FetchDataWithSourceAsync(string url)
    {
        var data = await FetchDataFromApiAsync(url);
        return (url, data);
    }

    private static async Task<(string api, string data)> FetchWithLimitedConcurrencyAsync(
        string url, SemaphoreSlim semaphore)
    {
        await semaphore.WaitAsync();
        try
        {
            return await FetchDataWithSourceAsync(url);
        }
        finally
        {
            semaphore.Release();
        }
    }
}