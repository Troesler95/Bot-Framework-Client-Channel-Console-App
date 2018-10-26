using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BFChannelTest
{
    /// <summary>
    /// Container for the Directline endpoint payload
    /// Not sure if I'm going to keep this or not.
    /// </summary>
    class DLConversation
    {
        public string ConversationId { get; set; }
        public string Token { get; set; }

        public int ExpiresIn { get; set; }
        public string StreamUrl { get; set; }

        // TODO: handle this as a GUID?
        public string ReferenceGrammarId { get; set; }

        public DLConversation() { }

        public override string ToString()
        {
            return $"{{\r\n" +
                $"\t\"conversationId\": \"{ConversationId}\",\r\n" +
                $"\t\"token\": \"{Token}\",\r\n" +
                $"\t\"expires_in\": {ExpiresIn},\r\n" +
                $"\t\"streamUrl\": \"{StreamUrl}\",\r\n" +
                $"\t\"referenceGrammarId\": \"{ReferenceGrammarId}\"\r\n" +
                $"}}";
        }

    }

    // [Serializable]
    //class DLConversation : ISerializable
    //{
    //    public string ConversationId { get; set; }
    //    public JwtSecurityToken Token { get; set; }

    //    public int ExpiresIn { get; set; }
    //    public Uri StreamUrl { get; set; }

    //    // TODO: handle this as a GUID?
    //    public string ReferenceGrammarId { get; set; }

    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        info.AddValue("conversation-id", ConversationId, typeof(string));
    //        info.AddValue("token", Token.RawAuthenticationTag, typeof(string));
    //        info.AddValue("expires-in", ExpiresIn);
    //        info.AddValue("stream-url", StreamUrl.ToString(), typeof(string));
    //        info.AddValue("reference-grammar-id", ReferenceGrammarId, typeof(string));
    //    }

    //    // required empty constructor
    //    public DLConversation() { }

    //    public DLConversation(SerializationInfo info, StreamingContext context)
    //    {
    //        ConversationId = info.GetString("conversation-id");
    //        Token = new JwtSecurityToken(info.GetString("token"));
    //        ExpiresIn = info.GetInt32("expires-in");
    //        StreamUrl = new Uri(info.GetString("stream-url"));
    //        ReferenceGrammarId = info.GetString("reference-grammar-id");
    //    }

    //    public DLConversation(string c_id, JwtSecurityToken token, int expiry, Uri stream = null, string guid = "")
    //    {
    //        ConversationId = c_id;
    //        Token = token;
    //        ExpiresIn = expiry;
    //        StreamUrl = stream;
    //        ReferenceGrammarId = guid;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{{\n" +
    //            $"\t\"conversationId\": \"{ConversationId}\"," +
    //            $"\t\"token\": \"{Token.RawAuthenticationTag}\"," +
    //            $"\t\"expires_in\": {ExpiresIn}," +
    //            $"\t\"streamUrl\": \"{StreamUrl.ToString()}\"," +
    //            $"\t\"referenceGrammarId\": \"{ReferenceGrammarId}\"" +
    //            $"}}";
    //    }
    //}
}
