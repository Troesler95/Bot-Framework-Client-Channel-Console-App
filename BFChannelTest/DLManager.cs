using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BFChannelTest
{
    /// <summary>
    /// Static manager class for Directline
    /// Need to think more about the SOLID design to avoid a "god" class
    /// </summary>
    public static class DLManager
    {
        private static DLConversation _conv = null;
        private static string _dlSecret = string.Empty;
        private static readonly string _confPath = @"C:\Dev\Bot-Framework-Client-Channel-Console-App\BFChannelTest\secrets.json";

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
            {
                try
                {
                    _setDirectlineSecretFromConfig();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DLManager ERR: Unable to set DL client secret from config file." +
                        "check that this file exists at the path and try again.");
#if DEBUG
                    Console.WriteLine($"{ex.GetType().ToString()}: {ex.Message}");
#endif
                    return;
                }
            }

            Program.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _dlSecret);

            // POST to get a conversation object
            HttpResponseMessage resp = await Program.Client.PostAsync(
                new Uri("v3/directline/conversations", UriKind.Relative),
                new StringContent(string.Empty)
                );

            var serializer = new JsonSerializer();
            var stream = await resp.Content.ReadAsStreamAsync();
            using (var streamReader = new StreamReader(stream))
            {
                using (var txtReader = new JsonTextReader(streamReader))
                {
                    var jsonParsed = (Dictionary<string, string>)serializer.Deserialize(txtReader, typeof(Dictionary<string, string>));
                    _conv = _createDLConvFromJson(jsonParsed);
                }
            }

            Console.WriteLine(_conv.ToString());
        }

        private static DLConversation _createDLConvFromJson(Dictionary<string, string> json)
        {
            try
            {
                DLConversation newConv = new DLConversation();
                string cont = string.Empty;


                json.TryGetValue("conversationId", out cont);
                newConv.ConversationId = cont;

                json.TryGetValue("token", out cont);
                newConv.Token = cont;

                int expiry;
                json.TryGetValue("expires_in", out cont);
                int.TryParse(cont, out expiry);
                newConv.ExpiresIn = expiry;

                json.TryGetValue("streamUrl", out cont);
                newConv.StreamUrl = new Uri(cont, UriKind.Absolute);

                json.TryGetValue("referenceGrammarId", out cont);
                newConv.ReferenceGrammarId = new Guid(cont);

                return newConv;
            }
            catch(Exception ex)
            {
                Console.WriteLine("DLManager ERR: Unable to create DLConversation from provided JSON. Is it deformed or empty?");
#if DEBUG
                Console.WriteLine($"{ex.GetType().ToString()}: {ex.Message}");
#endif
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves a Directline secret from the provided file path
        /// </summary>
        /// <TODO>
        /// Find a much better way to do this!!!
        /// </TODO>
        /// <returns></returns>
        private static bool _setDirectlineSecretFromConfig()
        {
            if (File.Exists(_confPath))
            {
                try
                {
                    using (StreamReader file = File.OpenText(_confPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        return (serializer.Deserialize(file, typeof(Dictionary<string, string>))
                            as Dictionary<string, string>).TryGetValue("directline", out _dlSecret);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BFChannelTest ERR: unable to read directline secret from secrets.json!");
#if DEBUG
                    Console.WriteLine($"{ex.GetType().ToString()}: {ex.Message}");
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
