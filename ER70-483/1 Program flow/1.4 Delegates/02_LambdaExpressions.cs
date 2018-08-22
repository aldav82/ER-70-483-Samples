using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._4_Delegates
{
    #region Sample 1
    /// <summary>
    /// As opposed to <see cref="DelegateSample1"/>, in this sample we will replace the function definition 
    /// with an inline lambda expression
    /// </summary>
    [SampleName(Name = "Sample 1 - Lambda expressions to create a delegate")]
    public class LambdaSample1 : BasicSample
    {
        public delegate int Calculate(int x, int y);

        protected override void UseSample()
        {
            Calculate calc = (x, y) => x + y;
            Logger.Info(calc(3, 4)); // Displays 7
            calc = (x, y) => x * y;
            Logger.Info(calc(3, 4)); // Displays 12
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// As opposed to <see cref="DelegateSample1"/>, in this sample we will replace the function definition 
    /// In this case, the lambda expression will contain multiple statements
    /// with an inline lambda expression
    /// </summary>
    [SampleName(Name = "Sample 2 - Lambda expression with multiple statements")]
    public class LambdaSample2 : BasicSample
    {
        public delegate int Calculate(int x, int y);

        protected override void UseSample()
        {
            Calculate calc = (x, y) =>
                                {
                                    Logger.Info("Adding numbers");
                                    return x + y;
                                };
            Logger.Info(calc(3, 4)); // Displays 7
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// In this sample we will use the built-in <see cref="Func{T1, T2, TResult}"/> type
    /// This type of built-in type defines a delegate that can take from 0 to 16 parameters, 
    /// and the last type (TResult) defines the return type of the function
    /// </summary>
    [SampleName(Name = "Sample 3 - Using Func<int,int,int>")]
    public class LambdaSample3 : BasicSample
    {
        protected override void UseSample()
        {
            Func<int, int, int> calc = (x, y) =>
                                {
                                    Logger.Info("Adding numbers using Func<int,int,int>");
                                    return x + y;
                                };
            Logger.Info(calc(3, 4)); // Displays 7
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// In this sample we will use the built-in <see cref="Action{T1, T2}"/> type
    /// This type describes a function than can take from 0 to 16 parameters and returns void 
    /// </summary>
    [SampleName(Name = "Sample 4 - Using Action<int,int>")]
    public class LambdaSample4 : BasicSample
    {
        protected override void UseSample()
        {
            Action<int, int> calc = (x, y) =>
                                {
                                    Logger.Info("Adding numbers using Action<int,int>");
                                    Logger.Info($"{x + y}");
                                };
            calc(3, 4); // Displays 7
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// In this sample we will use the built-in <see cref="Predicate{T}"/> type
    /// This type describes a function receives one parameter and returns a boolean result 
    /// </summary>
    [SampleName(Name = "Sample 5 - Using Predicate<int>")]
    public class LambdaSample5 : BasicSample
    {
        protected override void UseSample()
        {
            Predicate<int> dividedBy = (x) =>
                                {
                                    return (x % 2 == 0);
                                };
            int number = new Random((int)DateTime.Now.Ticks).Next(int.MaxValue);
            bool divisibleBy = dividedBy(number); 
            if (divisibleBy)
                Logger.Info($"{number} can be divided by 2");
            else
                Logger.Info($"{number} cannot be divided by 2");
        }
    }
    #endregion
}
