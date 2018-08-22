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
    /// Initial sample, showing a basic thread initialization
    /// In this sample we are using the <see cref="Thread.Join"/>. 
    /// By using this method, the main thread waits for its children to finish before continuing execution
    /// </summary>
    [SampleName(Name = "Sample 1 - Basic thread initialization")]
    public class ThreadSample1 : BasicSample
    {
        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Logger.Info("ThreadProc: {0}", i);
                Thread.Sleep(500);
            }
        }

        protected override void UseSample()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.Start();
            for (int i = 0; i < 4; i++)
            {
                Logger.Info("Main thread: Do some work.");
                Thread.Sleep(250);
            }
            t.Join();
            Logger.Info("Main thread: Continuing after all the threads are finished do to Join method.");
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// Foreground threads can be used to keep an application alive. 
    /// Only when all foreground threads end does the common language runtime (CLR) shut down your application. 
    /// Background threads are then terminated
    /// </summary>
    [SampleName(Name = "Sample 2 - Using a background thread")]
    public class ThreadSample2 : BasicSample
    {
        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Logger.Info("Sample 2 - ThreadProc: {0}", i);
                Thread.Sleep(500);
            }
        }

        protected override void UseSample()
        {
            // If you run this sample with the IsBackground property set to true, the application exits immediately. 
            // If you set it to false (creating a foreground thread), the application prints the ThreadProc message ten times
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.IsBackground = true;
            t.Start();
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// The Thread constructor has another overload that takes an instance of a ParameterizedThreadStart delegate. 
    /// This overload can be used if you want to pass some data through the start method of your thread to your worker method
    /// </summary>
    [SampleName(Name = "Sample 3 - Using a background thread")]
    public class ThreadSample3 : BasicSample
    {
        public static void ThreadMethod(object o)
        {
            Logger.Info($"ThreadProc: Receiving {o} as a parameter");
            // Here we are assuming o is of type int always. Since this is only an example and the only caller of this method is 
            // method UseSample, this works. In real life scenarios, protection should be implemented in the form of a valid cast validation
            for (int i = 0; i < (int)o; i++)
            {
                Logger.Info($"ThreadProc: {i} out of {o}");
                // Thread.Sleep(0) is used to notify the OS this thread is partially finished, so the CPU can switch to another thread
                Thread.Sleep(0);
            }
        }

        protected override void UseSample()
        {
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));
            t.Start(5);
            t.Join();
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// Marking a field with the ThreadStatic attribute. By doing this, each thread gets their own copy of the field
    /// </summary>
    [SampleName(Name = "Sample 4 - Marking a field with the TrheadStatic attribute")]
    public class ThreadSample4 : BasicSample
    {
        [ThreadStatic]
        public static int _field;
        public static int _commonField;

        protected override void UseSample()
        {
            ThreadStart action = () =>
               {
                   for (int x = 0; x < 10; x++)
                   {
                       _field++;
                       _commonField++;                       
                       Logger.Info($"Thread {Thread.CurrentThread.ManagedThreadId}: {_field} / {_commonField}");
                       Thread.Sleep(0);
                   }
               };
            new Thread(action).Start();            
            new Thread(action).Start();
        }
    }
    #endregion

    #region Sample 5
    /// <summary>
    /// Thread pools are a better way to handle multiple threads. Creating and destroying threads is an expensive procedure.
    /// By using a thread pool, we can reuse those threads, similar to the way in which a database connection pool works.
    /// </summary>
    [SampleName(Name = "Sample 5 - Thread pools")]
    public class ThreadSample5 : BasicSample
    {

        protected override void UseSample()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    Logger.Info($"Working on a thread from thread pool. Thread {Thread.CurrentThread.ManagedThreadId}");
                }); 
            }
        }
    }
    #endregion

    #region Sample 6
    /// <summary>
    /// By using the method Thread.Wait() we can freeze the execution of the calling thread to wait for the child thread to finish
    /// </summary>
    [SampleName(Name = "Sample 6 - Waiting for a thread to finish")]
    public class ThreadSample6 : BasicSample
    {

        protected override void UseSample()
        {
            Task t = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write("*");
                    Thread.Sleep(20);
                }
            });
            t.Wait();
        }
    }
    #endregion

    #region Sample 7
    /// <summary>
    /// Showing how to use a thread that returns a value
    /// </summary>
    [SampleName(Name = "Sample 7 - Using a thread that returns a value")]
    public class ThreadSample7 : BasicSample
    {

        protected override void UseSample()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });
            Logger.Info(t.Result); // Displays 42
        }
    }
    #endregion

    #region Sample 8
    /// <summary>
    /// Thread object provides a way to execute a Task after the thread has finished. By using the Continuation method, we can achieve this
    /// </summary>
    [SampleName(Name = "Sample 8 - Using Thread continuation")]
    public class ThreadSample8 : BasicSample
    {

        protected override void UseSample()
        {
            Task<int> t = Task.Run(() =>
            {
                Logger.Info("Thread A returns 42"); 
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });
            Logger.Info($"Thread B returns {t.Result}"); // Displays 84
        }
    }
    #endregion

    #region Sample 9
    /// <summary>
    /// By using a continuation task, we are able to provide different options that can be used to define which task we want to continue with 
    /// </summary>
    [SampleName(Name = "Sample 9 - Using Thread continuation according to thread result status")]
    public class ThreadSample9 : BasicSample
    {

        protected override void UseSample()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
               
            });
            t.ContinueWith((i) =>
            {
                Logger.Info("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);
            t.ContinueWith((i) =>
            {
                Logger.Info("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);
            var completedTask = t.ContinueWith((i) =>
            {
                Logger.Info("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            completedTask.Wait();
        }
    }
    #endregion

    #region Sample 10
    /// <summary>
    /// Showing how to attach child tasks to a parent task
    /// </summary>
    [SampleName(Name = "Sample 10 - Linking threads to a parent thread")]
    public class ThreadSample10 : BasicSample
    {
        protected override void UseSample()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                new Task(() => results[0] = 0,
                TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1,
                TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2,
                TaskCreationOptions.AttachedToParent).Start();
                return results;
            });
            // The final task will only run after the parent Task finishes
            // The parent Task will finish when all three children are finished
            var finalTask = parent.ContinueWith(
            parentTask =>
            {
                foreach (int i in parentTask.Result)
                    Logger.Info(i);
            });
            finalTask.Wait();
        }
    }
    #endregion

    #region Sample 11
    /// <summary>
    /// Showing how to use a TaskFactory
    /// This is the same example as Sample 10, but in this case, we will use a Task Factory to provide a common configuration for the tasks 
    /// </summary>
    [SampleName(Name = "Sample 11 - Using Thread Factory ")]
    public class ThreadSample11 : BasicSample
    {
        protected override void UseSample()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.ExecuteSynchronously);
                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });
            var finalTask = parent.ContinueWith(
            parentTask => {
                foreach (int i in parentTask.Result)
                    Logger.Info(i);
            });
            finalTask.Wait();
        }
    }
    #endregion

    #region Sample 12
    /// <summary>
    /// Showing how to use Task.WaitAny
    /// </summary>
    [SampleName(Name = "Sample 12 - Using Thread Factory ")]
    public class ThreadSample12 : BasicSample
    {
        protected override void UseSample()
        {
            Task<int>[] tasks = new Task<int>[3];
            tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });
            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];
                Logger.Info(completedTask.Result);
                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }
    }
    #endregion
}
