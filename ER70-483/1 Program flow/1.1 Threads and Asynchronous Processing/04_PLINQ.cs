using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._1_Threads_and_Asynchronous_Processing
{

    #region Sample 1
    /// <summary>
    /// Parallel Language-Integrated Query is a great addition to the language
    /// With this tool, we can perform parallel queries over a traditionally sequential one
    /// The ultimate decision to convert a query into a parallel one rests on the runtime. 
    /// However, you can force this behavior by using the WithExecutionMode.
    /// </summary>
    [SampleName(Name = "Sample 1 - PLINQ basic usage ")]
    public class PLINQSample1 : BasicSample
    {
        /// <summary>
        /// We are going to use a very expensive function in order to show the actual advantage of a certain degree of parallelism.
        /// The reason behind this is that very simple work is not a good candidate for parallelism, because of the underlying overhead of the parallelization work.
        /// See https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/understanding-speedup-in-plinq
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>Returns true if the number is prime</returns>
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        protected override void UseSample()
        {
            Stopwatch sw = new Stopwatch();
            int limit = 20000000;
            TimeSpan delayParallel = TimeSpan.Zero;
            TimeSpan delaySequentially = TimeSpan.Zero;

            var numbers = Enumerable.Range(0, limit);
            sw.Start();
            var parallelResult = numbers.AsParallel()
                .Where(i => IsPrime(i)).Count();
            sw.Stop();
            delayParallel = sw.Elapsed;
            sw.Reset();
            sw.Start();
            numbers = Enumerable.Range(0, limit);
            var sequentialResult = numbers
                .Where(i => IsPrime(i)).Count();
            sw.Stop();
            delaySequentially = sw.Elapsed;

            Logger.Info($"Parallel operation: We found {parallelResult} prime numbers. Calculation took {delayParallel}");
            Logger.Info($"Sequential operation: We found {parallelResult} prime numbers. Calculation took {delaySequentially}");
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// It is important to keep in mind that using PLINQ does not guarantees the order of the result
    /// If you want to keep the order, you can add the AsOrdered operator, but this will impact on the performance
    /// </summary>
    [SampleName(Name = "Sample 2 - Using AsOrdered ")]
    public class PLINQSample2 : BasicSample
    {

        protected override void UseSample()
        {
            var numbers = Enumerable.Range(0, 10);
            Logger.Info("Default behavior of AsParallel. Results are obtained in no specific order");
            var parallelResult = numbers.AsParallel()
                .Where(i => i % 2 == 0)
                .ToArray();
            foreach (int i in parallelResult)
                Logger.Info(i);
            Logger.Info("Using AsOrdered. Results are obtained in order");
            parallelResult = numbers.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0)
                .ToArray();
            foreach (int i in parallelResult)
                Logger.Info(i);
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// Handling aggregate exceptions during a PLINQ execution
    /// </summary>
    [SampleName(Name = "Sample 3 - Aggregate exceptions ")]
    public class PLINQSample3 : BasicSample
    {

        protected override void UseSample()
        {
            var numbers = Enumerable.Range(0, 20);
            try
            {
                var parallelResult = numbers.AsParallel()
                .Where(i => SpecialNumber(i));
                parallelResult.ForAll(e => Logger.Info($"{e} is a special number!"));
            }
            catch (AggregateException e)
            {
                Console.WriteLine("There where {0} exceptions", e.InnerExceptions.Count);
                e.InnerExceptions.AsParallel().ForAll(ex => Logger.Error(ex.Message));
            }
        }

        /// <summary>
        /// Should pass if the number is divisible by 2 or 3
        /// </summary>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException">Throws the exception if the number is divisible by 6</exception>
        /// <returns>True if divisible by 2 or 3, False i.o.c. except when divisible by 6, in which case it throws the exception</returns>
        private bool SpecialNumber(int i)
        {
            if (i % 6 == 0)
                throw new ArgumentException($"{i} is not a valid value", nameof(i));
            return i % 2 == 0 || i % 3 == 0;

        }
    }
    #endregion
}
