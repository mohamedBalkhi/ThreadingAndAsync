# Threading and Asynchronous Programming in C#

This solution demonstrates various threading and asynchronous programming concepts in C#.

## Projects

### 1. CAThreading
Basic examples of thread creation and management in C#, showing:
- Creating and naming threads
- Using Task abstraction
- Task factory for long-running operations

### 2. CAConcurrentVsParallelism
Benchmark comparison between sequential and parallel processing for a CPU-intensive task:
- Computing risk scores for a large number of customers
- Demonstrates performance gains from parallelization for CPU-bound operations
- Uses BenchmarkDotNet for accurate performance measurements

### 3. CAAsyncWebClient
Real-world example of asynchronous programming for I/O-bound operations:
- Fetching data from multiple web APIs
- Comparing sequential vs Concurrent approaches
- Demonstrating controlled concurrency with SemaphoreSlim
- Shows significant performance improvements when using Task.WhenAll for I/O operations

## Key Concepts Demonstrated

- **Threading**: Basic thread creation and management
- **Task Parallelism**: Using Parallel.For for CPU-bound operations
- **Asynchronous Programming**: Using async/await for I/O-bound operations
- **Concurrency Control**: Limiting concurrent operations with SemaphoreSlim
- **Performance Measurement**: Using BenchmarkDotNet for accurate benchmarking

## When to Use What

- **Threads**: Low-level control, specialized scenarios
- **Task Parallelism**: CPU-bound operations that benefit from multiple cores
- **Async/Await**: I/O-bound operations (network, disk, etc.) to avoid blocking threads
- **Limited Concurrency**: When you need to control resource usage while maintaining parallelism