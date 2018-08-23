using ER70_483._1_Program_flow._1._1_Threads_and_Asynchronous_Processing;
using ER70_483._1_Program_flow._1._2_Manage_multithreading;
using ER70_483._1_Program_flow._1._4_Delegates;
using ER70_483._2_Creating_and_Using_Types;
using ER70_483._3_Debuging_and_Security._3._2_Encryption;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            List<BasicSample> runningSamples = new List<BasicSample>();

            #region Thread Samples
            //List<BasicSample> delegateSamples = new List<BasicSample>() {
            //    new ThreadSample1(),
            //    new ThreadSample2(),
            //    new ThreadSample3(),
            //    new ThreadSample4(),
            //    new ThreadSample5(),
            //    new ThreadSample6(),
            //    new ThreadSample7(),
            //    new ThreadSample8(),
            //    new ThreadSample9(),
            //    new ThreadSample10(),
            //    new ThreadSample11(),
            //    new ThreadSample12()
            //};
            //runningSamples.AddRange(delegateSamples);
            #endregion

            #region Parallel Samples
            //List<BasicSample> delegateSamples = new List<BasicSample>() {
            //    new ParallelSample1()
            //};
            //runningSamples.AddRange(delegateSamples);
            #endregion

            #region Async Samples
            //List<BasicSample> asyncSamples = new List<BasicSample>() {
            //    new AsyncSample1()
            //};
            //runningSamples.AddRange(asyncSamples);
            #endregion

            #region PLINQ Samples
            //List<BasicSample> plinqSamples = new List<BasicSample>() {
            //    new PLINQSample1(),
            //    new PLINQSample2(),
            //    new PLINQSample3()
            //};
            //runningSamples.AddRange(plinqSamples);
            #endregion

            #region Synchronization Samples
            //List<BasicSample> synchronizationSamples = new List<BasicSample>() {
            //    new SynchronizingSample1(),
            //    new SynchronizingSample2(),
            //    new SynchronizingSample3(),
            //    new SynchronizingSample4(),
            //    new SynchronizingSample5(),
            //    new SynchronizingSample6()
            //};
            //runningSamples.AddRange(synchronizationSamples);
            #endregion

            #region Delegate Samples
            //List<BasicSample> delegateSamples = new List<BasicSample>() {
            //    new DelegateSample1(),
            //    new DelegateSample2(),
            //    new DelegateSample3(),
            //    new DelegateSample4(),
            //    new DelegateSample5()
            //};
            //runningSamples.AddRange(delegateSamples);
            #endregion

            #region Lambda Samples
            //List<BasicSample> lambdaSamples = new List<BasicSample>() {
            //    new LambdaSample1(),
            //    new LambdaSample2(),
            //    new LambdaSample3(),
            //    new LambdaSample4(),
            //    new LambdaSample5()
            //};
            //runningSamples.AddRange(lambdaSamples);
            #endregion

            #region Events
            //List<BasicSample> eventSamples = new List<BasicSample>() {
            //    new EventSample1(),
            //    new EventSample2(),
            //    new EventSample3(),
            //    new EventSample4()
            //};
            //runningSamples.AddRange(eventSamples);
            #endregion

            #region Unmanaged resources
            List<BasicSample> eventSamples = new List<BasicSample>() {
                new UnmanagedResourcesSample1(),
                new UnmanagedResourcesSample2(),
                new UnmanagedResourcesSample3()
            };
            runningSamples.AddRange(eventSamples);
            #endregion

            #region Encryption
            //List<BasicSample> encryptionSamples = new List<BasicSample>() {
            //    new SymmetricEncriptionSample1(),
            //    new SymmetricEncriptionSample2(),
            //    new SymmetricEncriptionSample3(),
            //    new SymmetricEncriptionSample3(),
            //    new SymmetricEncriptionSample4()
            //};
            //runningSamples.AddRange(encryptionSamples);
            #endregion

            foreach (var item in runningSamples)
            {
                try
                {
                    item.RunSample();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
            logger.Info("No more samples, press Enter to quit");
            Console.ReadLine();


        }
    }
}
