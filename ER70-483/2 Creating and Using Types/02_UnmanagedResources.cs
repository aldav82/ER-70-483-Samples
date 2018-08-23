using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER70_483._2_Creating_and_Using_Types
{
    #region Types
    public class TypeWithFinalizer
    {
        ~TypeWithFinalizer()
        {
            // Your clean up code goes here
            NLog.LogManager.GetCurrentClassLogger().Warn("Finalizer called");
            Thread.Sleep(2000); // Simulating a lot of clean up code
        }
    }

    public class TypeIDisposable : IDisposable
    {
        public void Dispose()
        {
            // Your clean up code goes here
            NLog.LogManager.GetCurrentClassLogger().Warn("Dispose method called");
            Thread.Sleep(2000); // Simulating a lot of clean up code
        }
    }
    #endregion

    #region Sample 1
    /// <summary>
    /// C# supports finalizers. Finalizers will be called when the Garbage Collector cleans up the resources associated to the instance
    /// When using Finalizers, you depend on whether the Garbage Collector decides to execute in order to clean up the resources 
    /// </summary>
    [SampleName(Name = "Sample 1 - Showing the call of the finalizer ")]
    public class UnmanagedResourcesSample1 : BasicSample
    {
        protected override void UseSample()
        {
            TypeWithFinalizer instance = new TypeWithFinalizer();
            instance = null;
            Logger.Info("Calling the Garbage Collector");
            GC.Collect(); // Calling the GC.Collect to force the finalizer call
            GC.WaitForPendingFinalizers(); // Making sure all finalizers have run before continuing to the next line
            Logger.Info("All finalizers were executed");
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// In the following sample, we will make use of the IDisposable interface
    /// Using the IDisposable interface allow us to call the clean up code immediately after it was used
    /// </summary>
    [SampleName(Name = "Sample 2 - Using IDisposable ")]
    public class UnmanagedResourcesSample2 : BasicSample
    {
        protected override void UseSample()
        {
            TypeIDisposable instance = new TypeIDisposable();
            Logger.Info("Disposing the object");
            instance.Dispose();
            Logger.Info("Object disposed");
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// Whilst the use of the Dispose method explicitly is available, we risk that it won't be called at all.
    /// For example, if an exception is thrown before the call to Dispose method, it won't be executed.
    /// To prevent this, the framework provides us with the using keyword
    /// </summary>
    [SampleName(Name = "Sample 3 - The 'using' keyword ")]
    public class UnmanagedResourcesSample3 : BasicSample
    {
        protected override void UseSample()
        {
            try
            {
                using (TypeIDisposable instance = new TypeIDisposable())
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Logger.Info("Even though an exception was thrown, the Dispose method was called");
            }
        }
    }
    #endregion

}
