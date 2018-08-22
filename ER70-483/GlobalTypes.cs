using System;
using System.Reflection;
using System.Linq;
using NLog;

namespace ER70_483
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SampleNameAttribute : Attribute
    {
        public string Name { get; set; }
    }


    public abstract class BasicSample
    {
        protected static Logger Logger
        {
            get
            {
                return LogManager.GetCurrentClassLogger();
            }
        }

        public void RunSample()
        {
            SampleNameAttribute attribute = GetType().GetCustomAttributes().Where(attr => attr.GetType().IsAssignableFrom(typeof(SampleNameAttribute))).FirstOrDefault() as SampleNameAttribute;
            if (attribute != null)
            {
                Logger.Info($"Running {attribute.Name}");
                Logger.Info($"-------------------------------------------------");
                Logger.Info($"");
                Logger.Info($"");
            }
            UseSample();
            Logger.Info($"");
            Logger.Info($"Press enter for next sample");
            Logger.Info($"");
            Logger.Info($"");
            Console.ReadLine();
        }
        protected abstract void UseSample();
    }
}
