#r "Newtonsoft.Json"

using System.Net;
using System.Xml;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // the api key to be used against the Dictionary API
    string apiKey = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "APIKEY", true) == 0)
        .Value;

    // the word to check 
    string word = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "word", true) == 0)
        .Value;

    var worddef = await GetWordDefinition(word, apiKey);
 
    return req.CreateResponse(HttpStatusCode.OK, worddef, "application/json");
}

/// <summary>
/// The getdef() method calls the API and retrieves the definition as JSON.
/// </summary>
/// <param name="word">The word looked up in the dictionary.</param>
/// <param name="key">The Dictionary.com API Key.</param>
/// <returns></returns>
public static async Task<Word> GetWordDefinition(string word, string apiKey)
{
    // established variables used in the method to make the call
    string endpointroot = "http://www.dictionaryapi.com/api/v1/references/collegiate/xml/";
    string apifrag = "?key=";
    string dictionarycall = endpointroot + word + apifrag + apiKey;

    // Call the dictionary API and read the response back
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, dictionarycall);
    request.Headers.Add("cache-control", "no-cache");
    var response = await client.SendAsync(request);
    string wordres = await response.Content.ReadAsStringAsync();

    // Retrive the values from the response
    XmlDocument wordxml = new XmlDocument();
    wordxml.LoadXml(wordres);

    XmlNode root = wordxml.DocumentElement;
    XmlNode nodename = root.SelectSingleNode("/entry_list/entry[1]/ew");
    XmlNode nodepart = root.SelectSingleNode("/entry_list/entry[1]/fl");
    XmlNode nodedefinition = root.SelectSingleNode("/entry_list/entry[1]/def/dt[1]");

    // Instantiate a word object and assign the values from the response.
    Word wordobj = new Word();
    wordobj.name = nodename.InnerXml;
    wordobj.part = nodepart.InnerXml;
    wordobj.definition = nodedefinition.InnerXml;

    return wordobj;
}

/// <summary>
/// The word object holds the key attributes for a word definition.
/// </summary>
internal class Word
{
    public string name { get; set; }
    public string part { get; set; }
    public string definition { get; set; }
}
