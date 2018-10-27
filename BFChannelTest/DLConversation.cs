using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;

namespace BFChannelTest
{
    /// <summary>
    /// Container for the Directline endpoint payload
    /// Not sure if I'm going to keep this or not.
    /// </summary>
    #region SIMPLE CLASS
    //class DLConversation
    //{
    //    public string ConversationId { get; set; }
    //    public string Token { get; set; }

    //    public int ExpiresIn { get; set; }
    //    public string StreamUrl { get; set; }

    //    // TODO: handle this as a GUID?
    //    public string ReferenceGrammarId { get; set; }

    //    public DLConversation() { }



    //    public override string ToString()
    //    {
    //        return $"{{\r\n" +
    //            $"\t\"conversationId\": \"{ConversationId}\",\r\n" +
    //            $"\t\"token\": \"{Token}\",\r\n" +
    //            $"\t\"expires_in\": {ExpiresIn},\r\n" +
    //            $"\t\"streamUrl\": \"{StreamUrl}\",\r\n" +
    //            $"\t\"referenceGrammarId\": \"{ReferenceGrammarId}\"\r\n" +
    //            $"}}";
    //    }

    //}
    #endregion

    #region COMPLEX CLASS
    class DLConversation : ISerializable
    {
        public string ConversationId { get; set; }

        // guess Directline tokens aren't JWT tokens,
        // only the connector tokens are?
        public string Token { get; set; }

        public int ExpiresIn { get; set; }
        public Uri StreamUrl { get; set; }

        public static DLConversation Empty { get => new DLConversation(); }

        // TODO: handle this as a GUID?
        public Guid ReferenceGrammarId { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("conversation-id", ConversationId, typeof(string));
            info.AddValue("token", Token, typeof(string));
            info.AddValue("expires-in", ExpiresIn);
            info.AddValue("stream-url", StreamUrl.ToString(), typeof(string));
            info.AddValue("reference-grammar-id", ReferenceGrammarId, typeof(string));
        }

        // required empty constructor
        public DLConversation() { }

        public DLConversation(SerializationInfo info, StreamingContext context)
        {
            ConversationId = info.GetString("conversation-id");
            Token = info.GetString("token");
            ExpiresIn = info.GetInt32("expires-in");
            StreamUrl = new Uri(info.GetString("stream-url"));
            ReferenceGrammarId = new Guid(info.GetString("reference-grammar-id"));
        }

        /// <summary>
        /// Constructor to handle if conversation is being created from results of either
        /// /v3/directline/generate/token or /v3/directline/converstaions
        /// </summary>
        /// <param name="c_id">Covnersation Id returned from Directline API</param>
        /// <param name="token">Token returned from Directline API</param>
        /// <param name="expiry">Time until token expires</param>
        /// <param name="stream">the StreamUrl returned from conversations operation</param>
        /// <param name="guid">the ReferenceGrammarId returned from conversations operation</param>
        public DLConversation(string c_id, string token, int expiry, Uri stream = null, string guid = "")
        {
            ConversationId = c_id;
            Token = token;
            ExpiresIn = expiry;
            StreamUrl = stream;
            ReferenceGrammarId = new Guid(guid);
        }

        public override string ToString()
        {
            return $"{{\r\n" +
                $"\t\"conversationId\": \"{ConversationId}\",\r\n" +
                $"\t\"token\": \"{Token}\",\r\n" +
                $"\t\"expires_in\": {ExpiresIn},\r\n" +
                $"\t\"streamUrl\": \"{StreamUrl.ToString()}\",\r\n" +
                $"\t\"referenceGrammarId\": \"{ReferenceGrammarId.ToString()}\"\r\n" +
                $"}}";
        }
    }
    #endregion
}
