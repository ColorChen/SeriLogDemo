using Serilog;
using Serilog.Extensions.Hosting;
using System.Text;
using System.Text.Json;

namespace SeriLogDemo_Net6
{
    public static class LogHelper
    {
        public static string RequestPayload = "";


        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }

        public static async void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            string requestBodyPayload = await ReadBodyFromRequest2(httpContext.Request);
            if(!string.IsNullOrEmpty(requestBodyPayload))
                diagnosticContext.Set("RequestBody", requestBodyPayload);


            string responseBodyPayload = await ReadBodyFromResponse(httpContext.Response);
            diagnosticContext.Set("ResponseBody", responseBodyPayload);

            // Set all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available. You're not sending sensitive data in a querystring right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.GetEndpoint();
            if (endpoint is object) // endpoint != null
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
            
        }

        private static async Task<string> ReadBodyFromResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{responseBody}";
        }

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Reset the request body stream position to the start so we can read it
            request.Body.Position = 0;

            // Leave the body open so the next middleware can read it.
            using StreamReader reader = new(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false);

            string requestbody = await reader.ReadToEndAsync();

            if (requestbody.Length > 0)
            {
                object? obj = JsonSerializer.Deserialize<object>(requestbody);
                if (obj is not null)
                {
                    return $"{requestbody}";
                }
            }

            return $"{requestbody}";
        }

        public static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));

        public static async Task<string> ReadBodyFromRequest2(HttpRequest request)
        {
            request.Body.Position = 0;
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
           
            return requestBody;
        }
    }


}
