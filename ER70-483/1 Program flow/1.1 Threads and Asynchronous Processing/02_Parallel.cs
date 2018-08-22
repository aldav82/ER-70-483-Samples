using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._1_Threads_and_Asynchronous_Processing
{

    #region Sample 1
    /// <summary>
    /// Showing different Parallel usages
    /// </summary>
    [SampleName(Name = "Sample 1 - Using Thread Factory ")]
    public class ParallelSample1 : BasicSample
    {
        protected override void UseSample()
        {
            Parallel.For(0, 10, i =>
            {
                Logger.Info($"Running a parallel For {i}");
                Thread.Sleep(1000);
            });
            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, i =>
            {
                Logger.Info($"Running a parallel ForEach {i}");
                Thread.Sleep(1000);
            });
            ParallelLoopResult result = 
                Parallel. For(0, 1000, (int i, ParallelLoopState loopState) =>
                {
                    Logger.Info($"Running a parallel For {i} before break");
                    if (i == 500)
                    {
                        Logger.Info("Breaking loop");
                        loopState.Break();
                    }
                    return;
                });
        }
    }
    #endregion
}
