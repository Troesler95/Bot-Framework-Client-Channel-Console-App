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
        static HttpClient client = new HttpClient();

        /// <summary>
        /// Asynchronously gets a token from the Directline REST API for use in this client 
        /// </summary>
        /// <TODO>
        /// Should the /generate operation be used or should the /conversations operation be used for this purpose?
        /// we probably wouldn't intend on distribution of them, so likely the former!
        /// </TODO>
        /// <returns>Task</returns>
        public static async Task GetDirectlineConversationAsync()
        {
            if (string.IsNullOrEmpty(_dlSecret))
                throw new ArgumentNullException("Directline secret is empty! Please populate this value with GetDirectlineSecretFromConfig");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _dlSecret);
            
            // POST to token generate API
            //HttpResponseMessage resp = await client.PostAsync(
            //    new Uri("v3/directline/tokens/generate", UriKind.Relative),
            //    new StringContent("")
            //    );

            // POST to get a conversation object
            HttpResponseMessage resp = await client.PostAsync(
                new Uri("v3/directline/conversations", UriKind.Relative),
                new StringContent(string.Empty)
                );

            var serializer = new JsonSerializer();
            var stream = await resp.Content.ReadAsStreamAsync();
            using (var streamReader = new StreamReader(stream))
            {
                using (var txtReader = new JsonTextReader(streamReader))
                {
                    DLConversation conv = serializer.Deserialize<DLConversation>(txtReader);
                    Console.WriteLine(conv.ToString());
                }
            }

            // for now, just write out the content
            // var content = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(content);
        }

        /// <summary>
        /// Console app's entrypoint
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("https://directline.botframework.com/");

            // Note: not adding try block here on purpose for debugging purposes!
            // IMPORTANT: Change this accordingly!!!
            // TODO: manage secrets better probably
            GetDirectlineSecretFromConfig(@"C:\Dev\BFChannelTest\BFChannelTest\secrets.json");
            
            // get a token from DL for use in this client channel
            GetDirectlineConversationAsync().Wait();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Retrieves a Directline secret from the provided file path
        /// </summary>
        /// <TODO>
        /// Find a much better way to do this!!!
        /// </TODO>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool GetDirectlineSecretFromConfig(string filePath)
        {
            if(File.Exists(filePath))
            {
                try
                {
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        return (serializer.Deserialize(file, typeof(Dictionary<string, string>)) 
                            as Dictionary<string, string>).TryGetValue("directline", out _dlSecret);
                    } 
                }
                catch (Exception ex)
                {
                    string exType = ex.GetType().ToString();

                    Console.WriteLine($"BFChannelTest ERR: unable to read directline secret from secrets.json!");
                    #if DEBUG
                    Console.WriteLine($"{exType}: {ex.Message}");
                    #endif
                    return false;
                }
            }
            else
            {
                throw new FileLoadException("BFChannelTest ERR: Please configure with Directline secret!");
            }
        }
    }
}
