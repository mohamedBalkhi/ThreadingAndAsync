using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CAConcurrentVsParallelism;

    public class Customer
    {
        public int Id { get; set; }
        public double[]? Metrics { get; set; }
        public double RiskScore { get; set; }
    }

    [MemoryDiagnoser]
    [RankColumn]
    public class RiskScoreBenchmark
    {
        private List<Customer> _customers;

        public RiskScoreBenchmark(List<Customer> customers)
        {
            _customers = customers;
        }

        [GlobalSetup]
        public void Setup()
        {
            const int customerCount = 1_000_000;
            const int metricsPerCustomer = 200;
            var random = new Random(42);

            _customers = new List<Customer>(customerCount);
            for (int i = 0; i < customerCount; i++)
            {
                var metrics = new double[metricsPerCustomer];
                for (int j = 0; j < metricsPerCustomer; j++)
                {
                    metrics[j] = random.NextDouble() * 200;
                }
                _customers.Add(new Customer { Id = i, Metrics = metrics });
            }
        }

        private double ComputeRiskScore(Span<double> metrics)
        {
            double score = 0;
            for (int i = 0; i < metrics.Length; i++)
            {
                score += Math.Pow(metrics[i], 2) * Math.Sin(metrics[i]) + Math.Sqrt(metrics[i]);
            }
            return score;
        }

        [Benchmark]
        public void ParallelProcessing()
        {
            Parallel.For(0, _customers.Count, i =>
            {
                var customer = _customers[i];
                customer.RiskScore = ComputeRiskScore(customer.Metrics.AsSpan());
            });
        }

        [Benchmark(Baseline = true)]
        public void SequentialProcessing()
        {
            for (var i = 0; i < _customers.Count; i++)
            {
                var customer = _customers[i];
                customer.RiskScore = ComputeRiskScore(customer.Metrics.AsSpan());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<RiskScoreBenchmark>();
        }
    }
