using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KnightMove;

public class Function
{

    private readonly IHttpClientFactory _httpClientFactory;

    public Function(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Function that receives a source and target from a user, calls CalculateKnightMove, and returns the string GUID of the result.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> FunctionHandler(string source, string target, ILambdaContext context)
    {
        var operationId = Guid.NewGuid().ToString();

        KnightPathResponse KPR = new KnightPathResponse()
        {
            Starting = source,
            Ending = target,
            OperationId = operationId
        };

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders
          .Accept
          .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

        var response = await client.PostAsJsonAsync("https://bnd510o3zj.execute-api.us-east-1.amazonaws.com/KnightMove2", KPR);
        context.Logger.Log("request sent");
        context.Logger.Log(response.StatusCode.ToString());

        return EntryPoint.CreateResponse(200, $"Operation Id {operationId} was created. Please query it to find your results.");
    }
}
