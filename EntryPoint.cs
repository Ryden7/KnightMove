using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace KnightMove
{
    public class EntryPoint
    {
        private static IServiceProvider _serviceProvider;

        static EntryPoint()
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var source = input.QueryStringParameters["source"];
            var target = input.QueryStringParameters["target"];

            if (source == null || target == null || OutOfRange(source, target))
            {
                return CreateResponse(400, "Please stay within the parameters of the board.");
            }

            if (source.Equals(target))
            {
                return CreateResponse(400, "Start position must be different then end position.");
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var function = scope.ServiceProvider.GetRequiredService<Function>();
                return await function.FunctionHandler(source, target, context);
            }
        }

        private static bool OutOfRange(string source, string target)
        {
            var sourceSpace = source.ToArray();
            var targetSpace = target.ToArray();

            if (source[0] > 'H' || source[0] < 'A' || source[1] <= '0' || source[1] > '8' || target[0] > 'H' || target[0] < 'A' || target[1] <= '0' || target[1] > '8'
                    || source.Length <= 0 || source.Length > 2 || target.Length <= 0 || target.Length > 2)
            {
                return true;
            }

            return false;
        }

        public static APIGatewayProxyResponse CreateResponse(int statusCode, string message)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = message,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
