using System;
using System.Net.Http;

namespace BFClientChannelNetCore
{
    class Program
    {
        public static HttpClient Client = new HttpClient() { BaseAddress = new Uri("https://directline.botframework.com/") };

        /// <summary>
        /// Console app's entrypoint
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            // get a token from DL for use in this client channel
            DLManager.GetDirectlineConversationAsync().Wait();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
