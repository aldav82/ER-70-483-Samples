using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._4_Delegates
{
    #region Types
    public class Pub
    {
        public class MyArgs : EventArgs
        {
            public MyArgs(int value)
            {
                Value = value;
            }
            public int Value { get; set; }
        }

        public event EventHandler<MyArgs> OnChange = delegate { };

        private event EventHandler<MyArgs> onAccessorChange = delegate { };

        /// <summary>
        /// Sample using custom event accessors. Much like a property
        /// It is important to use some lock mechanism in order to guarantee thread safety
        /// </summary>
        public event EventHandler<MyArgs> OnAccessorChange {
            add
            {
                lock (onAccessorChange)
                {
                    onAccessorChange += value;
                }
            }
            remove
            {
                lock (onAccessorChange)
                {
                    onAccessorChange -= value;
                }
            }
        }

        public void Raise()
        {
            if (OnChange != null)
            {
                OnChange(this, new MyArgs(42));
            }
        }

        public void ManuallyRaise()
        {
            var exceptions = new List<Exception>();
            foreach (Delegate handler in OnChange.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, new MyArgs(77));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public void RaiseCustom()
        {
            if (onAccessorChange != null)
            {
                onAccessorChange(this, new MyArgs(88));
            }
        }
    }
    #endregion

    #region Sample 1
    /// <summary>
    /// Initial sample, showing how to subscribe to an event
    /// </summary>
    [SampleName(Name = "Sample 1 - Basic event usage")]
    public class EventSample1 : BasicSample
    {
        protected override void UseSample()
        {
            Pub p = new Pub();
            p.OnChange += (s,e) => Logger.Info($"Event raised to method 1. Arguments received {e.Value}");
            p.OnChange += (s, e) => Logger.Info($"Event raised to method 2. Arguments received {e.Value}");
            p.Raise(); // Will output the messages
        }
    }
    #endregion

    #region Sample 2
    /// <summary>
    /// Event usage with custom event accessors
    /// </summary>
    [SampleName(Name = "Sample 2 - Event usage with custom event accessors")]
    public class EventSample2 : BasicSample
    {
        protected override void UseSample()
        {
            Pub p = new Pub();
            p.OnAccessorChange += (s,e) => Logger.Info($"Event raised to method 1 by subscribing using custom accessors. Arguments received {e.Value}");
            p.OnAccessorChange += (s, e) => Logger.Info($"Event raised to method 2 by subscribing using custom accessors. Arguments received {e.Value}");
            p.RaiseCustom(); // Will output the messages
        }
    }
    #endregion

    #region Sample 3
    /// <summary>
    /// Showing events execution order. Subscribed functions are executed in the order in which they were added
    /// </summary>
    [SampleName(Name = "Sample 3 - Event execution order")]
    public class EventSample3 : BasicSample
    {
        protected override void UseSample()
        {
            Pub p = new Pub();
            p.OnChange += (s, e) => Logger.Info($"Subscriber 1 executed");
            p.OnChange += (s, e) => throw new Exception();
            p.OnChange += (s, e) => Logger.Info($"Subscriber 3 executed");
            try
            {
                p.Raise(); // Will output the messages
            }
            catch (Exception)
            {
                Logger.Info($"Exception received");
            }
        }
    }
    #endregion

    #region Sample 4
    /// <summary>
    /// Manually executing each subscriber to guarantee that no matter if an exception is launched, the rest will continue to be executed
    /// </summary>
    [SampleName(Name = "Sample 4 - Event execution order")]
    public class EventSample4 : BasicSample
    {
        protected override void UseSample()
        {
            Pub p = new Pub();
            p.OnChange += (sender, e)
            => Logger.Info($"Subscriber 1 called. Argument value {e.Value}");
            p.OnChange += (sender, e)
            => { throw new Exception(); };
            p.OnChange += (sender, e)
            => Logger.Info($"Subscriber 3 called. Argument value {e.Value}");
            try
            {
                p.ManuallyRaise();
            }
            catch (AggregateException ex)
            {
                Logger.Info($"Total exceptions thrown: {ex.InnerExceptions.Count}");
            }
        }
    }
    #endregion
}
