using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;

namespace ApiGateway.Aggregators.Extensions
{
    internal static class DownstreamResponseExtensions
    {
        public static async Task<JToken> GetBody(this DownstreamResponse response)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            return JToken.Parse(responseString);
        }
    }
}