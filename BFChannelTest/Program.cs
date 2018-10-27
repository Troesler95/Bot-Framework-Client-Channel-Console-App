using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace BFChannelTest
{
    class Program
    {
        private static string _dlSecret = string.Empty;
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
