using Neo.SmartContract.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace neo_knights_api
{
    public class KnightService
    {
        private static string[] properties = {
            "title",
            "author",
            "name",
            "subreddit",
            "selftext",
            "url_overridden_by_dest",
            "created_utc"
            };
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
                response["result"] = (JToken)JsonConvert.DeserializeObject(r);
                if (response["result"][0]["data"]["children"][0]["data"] != null)
                {
                    List<string> keysToRemove = new List<string>();
                    foreach (var item in (JObject)response["result"][0]["data"]["children"][0]["data"])
                    {
                        var tmp = item.Key;
                        if(!properties.Contains(tmp))
                        {
                            keysToRemove.Add(tmp);
                        }
                    }
                    foreach (var item in keysToRemove)
                    {
                        response["result"][0]["data"]["children"][0]["data"][item].Parent.Remove();
                    }
                }
                return JsonConvert.SerializeObject(response);
            }
             return string.Empty;
        }
    }
}
