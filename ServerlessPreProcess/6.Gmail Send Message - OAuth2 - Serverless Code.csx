#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;

private const string gmailMessageApiUrl = "https://www.googleapis.com/gmail/v1/users/me/messages/send";

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // Check the authorization header, BadRequest if it's not provided
    var authorizationHeader = req.Headers.FirstOrDefault( x => x.Key == "Authorization").Value?.FirstOrDefault();
    if(string.IsNullOrEmpty(authorizationHeader))
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Authorization header missing or empty.");
    }

    // Read the request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Let's create the email object payload. 
    var sb = new StringBuilder(1024);
    sb.Append($"To: {data.To}{Environment.NewLine}");
    sb.Append($"Cc: {data.Cc}{Environment.NewLine}");
    sb.Append($"Subject: {data.Subject}{Environment.NewLine}{Environment.NewLine}");
    sb.Append($"{data.Message}");

    // Create the raw payload by Base64 encoding our payload
    var raw = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));

    // Build a json object expected by Gmail servers. "raw" property will contain our
    // Base64 encoded email object
    var requestBody = new JObject();
    requestBody.Add("raw", raw);

    // Send the request to the endpoint URL. 
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, gmailMessageApiUrl);

    // Inject the OAuth2 token into the request
    request.Headers.Add("Authorization", authorizationHeader);
    request.Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

    return await client.SendAsync(request);
}
