using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapGet("/webhook/v1/line/{clientId}", ([FromRoute] string clientId) => 
{
    return $"From: {clientId}";
});

app.MapPost("/webhook/v1/line/{clientId}", (
    IHttpContextAccessor contextAccessor, 
    [FromRoute] string clientId, 
    [FromBody] object content
) => 
{
    // get the x-line-signature header for validation
    string xLineSig = contextAccessor.HttpContext!
        .Request.Headers["X-Line-Signature"].ToString();
    
    // get line secret id from appsetting.json
    string lineSecret = configuration["LineProvider:SecretId"];

    // create a new instance of HMACSHA256
    var key = Encoding.UTF8.GetBytes(lineSecret);
    var hmac = new HMACSHA256(key);

    // Compute the HMAC of the request body
    // the request body need to be string
    var requestBody = JsonSerializer.Serialize(content);
    var bodyBytes = Encoding.UTF8.GetBytes(requestBody);
    var hmacBytes = hmac.ComputeHash(bodyBytes);

    // Compare the computed HMAC to the one sent in the request headers
    var receivedHmac = Convert.FromBase64String(xLineSig);
    if (hmacBytes.SequenceEqual(receivedHmac))
    {
        // Request is valid
        Console.WriteLine("Success!");
    }
    else
    {
        // Request is not valid
        Console.WriteLine("Failed!");
    }

    return content;
});
app.Run();
