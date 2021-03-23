using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace ApiGateway.Aggregators.Base
{
    public abstract class AggregatorBase : IDefinedAggregator
    {
        public abstract Task<DownstreamResponse> Aggregate(List<HttpContext> responses);

        protected DownstreamResponse GetResponse(List<HttpContext> responses, string key)
        {
            return responses
                .Where(c => c.Items.DownstreamRoute().Key == key)
                .Select(c => c.Items.DownstreamResponse())
                .FirstOrDefault();
        }

        protected DownstreamResponse CreateResponse(JObject body, HttpStatusCode statusCode, string reasonPhrase)
        {
            var stringContent = new StringContent(body.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, statusCode, new List<KeyValuePair<string, IEnumerable<string>>>(), reasonPhrase);
        }

        protected bool AnyFailed(params DownstreamResponse[] responses)
        {
            return responses.Any(response => response.StatusCode != HttpStatusCode.OK);
        }
    }
}