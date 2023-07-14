using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Policies
{
    public static class HttpPolicies
    {
        public static Polly.Retry.AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError() // HttpRequestException, 5XX and 408
            .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) // 429
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return retryPolicy;
        }
    }
}
