using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._2_Manage_multithreading
{

    #region Sample 1
    /// <summary>
    /// The lock operator gives us the ability to block certain areas of our code so only one thread at a time can access it 
    /// </summary>
    [SampleName(Name = "Sample 1 - Using the lock keyword")]
    public class SynchronizingSample1 : BasicSample
    {
        protected override void UseSample()
        {
            int n = 0;
            object _lock = new object();
            var up = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    lock (_lock)
                    {
                        n++;
                    }
                }
            });
            for (int i = 0; i < 1000000; i++)
            {
                lock (_lock)
                {
                    n--;
                }
            }
            up.Wait();
            Logger.Info(n); // Will output 0 because variable n is synchronized
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// The volatile keyword indicates that a field might be modified by multiple threads that are executing at the same time. 
    /// Fields that are declared volatile are not subject to compiler optimizations that assume access by a single thread.
    /// See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile
    /// </summary>
    [SampleName(Name = "Sample 2 - The volatile keyword")]
    public class SynchronizingSample2 : BasicSample
    {
        private static volatile int _flag = 0;
        private static int _value = 0;
        public static void Thread1()
        {
            _value = 5;
            _flag = 1;
        }
        public static void Thread2()
        {
            if (_flag == 1)
                Logger.Info(_value); // Will always output 5
        }
        protected override void UseSample()
        {
            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);
            thread1.Start();
            thread2.Start();
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// The Interlocked class provides the ability to perform operations in an atomic way
    /// </summary>
    [SampleName(Name = "Sample 3 - The Interlocked class")]
    public class SynchronizingSample3 : BasicSample
    {
        
        protected override void UseSample()
        {
            int n = 0;
            var up = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                    Interlocked.Increment(ref n);
            });
            for (int i = 0; i < 1000000; i++)
                Interlocked.Decrement(ref n);
            up.Wait();
            Logger.Info(n);
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// To enable the cancellation of a long running task, the framework provides a class named CancellationToken
    /// In this case, when the task is canceled, it's final state will be RanToCompletion
    /// </summary>
    [SampleName(Name = "Sample 4 - Using CancellationToken to cancel tasks")]
    public class SynchronizingSample4 : BasicSample
    {        
        protected override void UseSample()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Logger.Info("*");
                    Thread.Sleep(1000);
                }
                Logger.Info("Task canceled");
            }, token);

            Logger.Info("Press enter to stop the task");
            Console.ReadLine();
            cancellationTokenSource.Cancel();
        }
    }
    #endregion

    #region Sample 5
    /// <summary>
    /// To notify outside users of the Task that it was canceled, we can make use of the method ThrowIfCancellationRequested
    /// </summary>
    [SampleName(Name = "Sample 5 - Signaling outside users of the Task that it was canceled")]
    public class SynchronizingSample5 : BasicSample
    {        
        protected override void UseSample()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Logger.Info("*");
                    Thread.Sleep(1000);
                }
                token.ThrowIfCancellationRequested();
            }, token);

            try
            {
                Logger.Info("Press enter to stop the task");
                Console.ReadLine();
                cancellationTokenSource.Cancel();
                task.Wait();
            }
            catch (AggregateException e)
            {
                Logger.Info(e.InnerExceptions[0].Message);
            }
        }
    }
    #endregion

    #region Sample 6
    /// <summary>
    /// You can react to a cancellation by executing a continuation task specifically on a Cancellation scenario
    /// </summary>
    [SampleName(Name = "Sample 6 - ContinueWith in a cancellation scenario")]
    public class SynchronizingSample6 : BasicSample
    {        
        protected override void UseSample()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.Write("*");
                    Thread.Sleep(1000);
                }
                token.ThrowIfCancellationRequested();
            }, token).ContinueWith((t) =>
            {
                t.Exception?.Handle((e) => true);
                Logger.Info("You have canceled the task");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Logger.Info("Press enter to stop the task");
            Console.ReadLine();
            cancellationTokenSource.Cancel();
            task.Wait();
        }
    }
    #endregion

}
