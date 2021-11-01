using Neo.SmartContract.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace neo_knights_api
{
    public class KnightService
    {
        public async Task<string> GetKnight(string hash)
        {
            if(hash == string.Empty)
            {
                return string.Empty;
            }
            var urlBytes = StdLib.Base58Decode(hash);
            var url = Encoding.UTF8.GetString(urlBytes);
            HttpResponseMessage message;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Neo Knights App");
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return string.Empty;
            if (uri.Authority == "www.reddit.com" || uri.Authority == "reddit.com")
            {
                message = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                var r = await message.Content.ReadAsStringAsync(CancellationToken.None);
                var response = new JObject();
                //have to remove:
                // ups, upvote_ratio, score, downs
                response["result"] = (JToken)JsonConvert.DeserializeObject(r);
                response["result"][0]["data"]["children"][0]["data"]["ups"].Parent.Remove();
                response["result"][0]["data"]["children"][0]["data"]["upvote_ratio"].Parent.Remove();
                response["result"][0]["data"]["children"][0]["data"]["score"].Parent.Remove();
                response["result"][0]["data"]["children"][0]["data"]["downs"].Parent.Remove();
                return JsonConvert.SerializeObject(response);
            }
             return string.Empty;
        }
    }
}
