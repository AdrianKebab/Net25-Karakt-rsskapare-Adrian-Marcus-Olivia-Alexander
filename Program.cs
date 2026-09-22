using OpenAI;
using OpenAI.Responses;
using System.ClientModel;
using DotNetEnv;

#pragma warning disable OPENAI001

Env.Load();

const string deploymentName = "gpt-5.4-mini";
var endpoint = Environment.GetEnvironmentVariable("ENDPOINT") ?? throw new Exception("fel endpoint");
var apiKey = Environment.GetEnvironmentVariable("KEY") ?? throw new Exception("fel key");

var client = new ResponsesClient(
    new ApiKeyCredential(apiKey),
    new ResponsesClientOptions
    {
        Endpoint = new Uri(endpoint)
    });


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ResponseResult response = client.CreateResponse(
    deploymentName,
    $"""
    Du ska skriva om alla ord från inmatningen till "Kebab"
    input pls
    """
);

//Console.WriteLine(response.GetOutputText());

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
