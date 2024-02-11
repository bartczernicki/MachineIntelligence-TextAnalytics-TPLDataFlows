using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace MachineIntelligenceTPLDataFlows.Policies
{
    public static class HttpPolicies
    {
        public static Polly.Retry.AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError() // HttpRequestException, 5XX and 408
            .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.NotFound) //  Handle NotFound 404
            .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.TooManyRequests) //  Handle too many requests 429
            .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)),
                    onRetry: (response, calculatedWaitDuration) =>
                    {
                        Console.WriteLine($"Failed attempt. Waited for {calculatedWaitDuration}");// Retrying. {response.Exception.Message} - {response.Exception.StackTrace}");
                    }
            );
            

            return retryPolicy;
        }
    }
}
