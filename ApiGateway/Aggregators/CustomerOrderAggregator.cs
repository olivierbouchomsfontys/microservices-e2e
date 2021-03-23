using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Aggregators.Base;
using ApiGateway.Aggregators.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;

namespace ApiGateway.Aggregators
{
    public class CustomerOrderAggregator : AggregatorBase
    {
        public override async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var customerResponse = GetResponse(responses, "CustomerById");
            var orderResponse = GetResponse(responses, "OrdersByCustomerId");
            
            var body = new JObject();

            body.Add("customer", await customerResponse.GetBody());
            body.Add("orders", await orderResponse.GetBody());

            HttpStatusCode statusCode = HttpStatusCode.OK;
            string reasonPhrase = "OK";

            if (AnyFailed(customerResponse, orderResponse))
            {
                statusCode = HttpStatusCode.MultiStatus;
                reasonPhrase = ReasonPhrases.GetReasonPhrase((int) HttpStatusCode.MultiStatus);
            }

            return CreateResponse(body, statusCode, reasonPhrase);
        }
    }
}