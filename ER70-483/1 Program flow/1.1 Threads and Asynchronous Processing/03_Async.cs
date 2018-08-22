using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER70_483._1_Program_flow._1._1_Threads_and_Asynchronous_Processing
{

    #region Sample 1
    /// <summary>
    /// Showing how to use Task.WaitAny
    /// </summary>
    [SampleName(Name = "Sample 1 - Using Thread Factory ")]
    public class AsyncSample1 : BasicSample
    {
        public static async Task<string> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.microsoft.com");
                return result;
            }

        }
        protected override void UseSample()
        {
            try
            {
                string result = DownloadContent().Result;
                Logger.Info(result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
    #endregion
}
