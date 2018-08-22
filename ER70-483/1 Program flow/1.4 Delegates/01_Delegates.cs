using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._4_Delegates
{


    #region Types
    public class Product
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string IsValid { get; set; }

        public static implicit operator Product(string n)
        {
            var p = new Product() { Id = n };
            return p;
        }
    }

    public class TV: Product
    {
        public int ScreenSize { get; set; }

        public static implicit operator TV(string n)
        {
            var p = new TV() { Name = n };
            return p;
        }
    }

    public class Laptop: Product
    {
        public int RAM { get; set; }

        public static implicit operator Laptop(int n)
        {
            var p = new Laptop() { RAM = n };
            return p;
        }
    }
    #endregion

    #region Sample 1
    /// <summary>
    /// Initial sample, showing how to assing existing functions to a delegate variable and then executing the function 
    /// through the assigned variable
    /// </summary>
    [SampleName(Name = "Sample 1 - Basic delegate usage")]
    public class DelegateSample1 : BasicSample
    {
        public delegate int Calculate(int x, int y);
        public int Add(int x, int y) { return x + y; }
        public int Multiply(int x, int y) { return x * y; }

        protected override void UseSample()
        {
            Calculate calc = Add;
            Logger.Info(calc(3, 4)); // Displays 7
            calc = Multiply;
            Logger.Info(calc(3, 4)); // Displays 12
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// In this sample we are showing how to define an anonymous function by using delegates
    /// </summary>
    [SampleName(Name = "Sample 2 - Anonymous function")]
    public class DelegateSample2 : BasicSample
    {
        delegate int ReturnDelegate(int a, int b);

        ReturnDelegate func = delegate (int a, int b)
        {
            return a + b;
        };

        protected override void UseSample()
        {
            var result = func(3, 4);
            Logger.Info($"{result}"); // Displays 7
        }
    }
    #endregion

    #region Sample 3  
    /// <summary>
    /// Delegate usage. The following sample shows how to use a delegate as a paremeter of another function. 
    /// In this case we are using the method <see cref="System.Collections.Generic.List{T}.Sort"/> that receives
    /// a delegate of type <see cref="System.Comparison{T}"/> 
    /// </summary>
    [SampleName(Name = "Sample 3 - Comparison")]
    public class DelegateSample3 : BasicSample
    {

        List<Product> products = new List<Product>() { "Product3", "Product2", "Product1" };
        Comparison<Product> comparer = delegate (Product a, Product b)
        {
            if (a.Id == null)
                return -1;
            if (b.Id == null)
                return 1;
            return a.Id.CompareTo(b.Id);
        };

        protected override void UseSample()
        {
            products.Sort(comparer);
            Logger.Info("Products ordered: ");
            foreach (var item in products)
            {
                Logger.Info(item.Id);
            }
        }
    }
    #endregion

    #region Sample 4  
    /// <summary>
    /// The feature of delegates that lets you combine them together is called multicasting
    /// You can use the + or += operator to add another method to the invocation list of an existing delegate instance
    /// You can also remove a method from an invocation list by using the decrement assignment operator (- or -=).
    /// </summary>
    [SampleName(Name = "Sample 4 - Multicasting")]
    public class DelegateSample4 : BasicSample
    {
        public void MethodOne()
        {
            Logger.Info("MethodOne");
        }
        public void MethodTwo()
        {
            Logger.Info("MethodTwo");
        }
        public delegate void Del();
        
        protected override void UseSample()
        {
            Del del = MethodOne;
            del += MethodTwo;
            Logger.Info($"Delegate invocation list consists of {del.GetInvocationList().GetLength(0)} elements");
            del();
        }
    }
    #endregion

    #region Sample 5  
    /// <summary>
    /// When you assign a method to a delegate, the method signature does not have to match the delegate exactly. 
    /// This is called covariance and contravariance. 
    /// Covariance makes it possible that a method has a return type that is more derived than that defined in the delegate. 
    /// Contravariance permits a method that has parameter types that are less derived than those in the delegate type.
    /// See https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/
    /// </summary>
    [SampleName(Name = "Sample 5 - Delegate covariance and contravariance")]
    public class DelegateSample5 : BasicSample
    {
        #region Covariance
        public delegate Product CovarianceDel();
        public TV MethodTV() { return new TV() { Name = "Samsung OLED", ScreenSize = 24 }; }
        public Laptop MethodLaptop() { return new Laptop() { Name = "Lenovo ThinkPad", RAM = 16 }; }

        private void CovarianceSample()
        {
            CovarianceDel del;
            del = MethodTV;
            Product product = del();
            Logger.Info($"Covariance result 1: {product.Name}");
            del = MethodLaptop;
            product = del();
            Logger.Info($"Covariance result 2: {product.Name}");
        } 
        #endregion

        #region Contravariace
        public delegate void ContravarianceDel(TV tv);
        public void MethodContravariance(Product product) { Logger.Info($"Contravariance, handling type {product.GetType().Name} result: {product.Name}"); }
        

        private void ContravarianceSample()
        {
            ContravarianceDel del;
            del = MethodContravariance;
            del(new TV() { Name = "Samsumg OLED" });
        } 

        #endregion

        protected override void UseSample()
        {
            CovarianceSample();
            ContravarianceSample();
        }
    }
    #endregion
}
