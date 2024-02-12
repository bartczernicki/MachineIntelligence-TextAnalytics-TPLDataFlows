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
                        // Note: With OpenAI and AzureOpenAI, you can retrieve the 429 TTM for next request.
                        System.Diagnostics.Debug.WriteLine($"Failed request Status Code: {response.Result.StatusCode}, Request URI: {response.Result.RequestMessage.RequestUri}");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"**** Failed HttpRequest attempt. Waited for {calculatedWaitDuration} Retrying. {response.Result.StatusCode}, Request URI: {response.Result.RequestMessage.RequestUri}");
                    }
            );
            
            return retryPolicy;
        }
    }
}
