namespace CAThreading;

public class Program
{
    public static void Main(string[] args)
    {
        Thread.CurrentThread.Name = "Main"; // We can set the name of the main thread, useful for Debugging
        PrintThreadInfo(Thread.CurrentThread);
        Thread thread = new Thread(() => PrintThreadInfo(Thread.CurrentThread))
        {
            Name = "Worker" // object initializer
        };
        thread.Start();
        thread.Join();
        // Task Abstraction ()
        Task.Run(() => PrintThreadInfo(Thread.CurrentThread)).Wait(); // .Wait is needed here or program may be terminated before the Task.Delay is done.
        // Another Method (Factory) => useful for creating LongRunning Tasks.
        Task.Factory.StartNew(() => PrintThreadInfo(Thread.CurrentThread), TaskCreationOptions.LongRunning | TaskCreationOptions.HideScheduler).Wait();
    }

    static void PrintThreadInfo(Thread thread)
    {
        // Task.Delay(2000); // No effect 
        Task.Delay(2000).Wait(); // Blocks
        Console.WriteLine($"Thread ID: {thread.ManagedThreadId}, Name: {thread.Name} IsBackground: {thread.IsBackground}");
    }
}
