using System;

namespace MonteCarloIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            var monteCarlo = new MonteCarlo();
            System.Console.WriteLine("Running with 10,000,000 samples.");
            var piEstimate = monteCarlo.Run(10000000);
            System.Console.WriteLine($"The estimate of pi is: {piEstimate.Estimate}");
            System.Console.WriteLine($"The percent error is: {piEstimate.PercentError}%");
        }
    }
}
