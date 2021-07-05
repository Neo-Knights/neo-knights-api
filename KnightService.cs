using Neo.SmartContract.Native;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace neo_knights_api
{
    public class KnightService
    {
        public async Task<string> GetTopKnight(string hash)
        {
            string urlTop = "https://www.reddit.com/r/NEO/top/.json?sort=top&t=week&limit=1";
            var url = Encoding.UTF8.GetBytes(urlTop);
           
            var urlHash = (string)StdLib.Base58Encode(url);
            if (!(urlHash == hash))
            {
                return string.Empty;
            }
            
            HttpResponseMessage message;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Neo Knights App");
            if (!Uri.TryCreate(urlTop, UriKind.Absolute, out var uri))
                return string.Empty;
            message = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);

            return await message.Content.ReadAsStringAsync(CancellationToken.None);
        }
        public async Task<string> VerifyKnight(string knight, string hash)
        {
            string urlVerify = "https://www.reddit.com/user/" + knight + "/gilded/.json?limit=1";
            var url = Encoding.UTF8.GetBytes(urlVerify);

            var urlHash = (string)StdLib.Base58Encode(url);
            if (!(urlHash == hash))
            {
                return string.Empty;
            }

            HttpResponseMessage message;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Neo Knights App");
            if (!Uri.TryCreate(urlVerify, UriKind.Absolute, out var uri))
                return string.Empty;
            message = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, CancellationToken.None);

            return await message.Content.ReadAsStringAsync(CancellationToken.None);
        }
    }
}
