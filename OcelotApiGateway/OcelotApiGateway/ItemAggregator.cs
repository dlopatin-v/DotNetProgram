using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Configuration;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;

namespace OcelotApiGateway;

public class ItemAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var itemNames = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
        var itemDetails = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();
        JArray names = JArray.Parse(itemNames);
        JArray details = JArray.Parse(itemDetails);
        Dictionary<string, object?> response = new();
        foreach (string? name in names)
        {
            response.Add(name, details.FirstOrDefault(d => d.Value<string>("name") == name));
        }

        var headers = responses.SelectMany(x => x.Items.DownstreamResponse().Headers).ToList();
        return new DownstreamResponse(new StringContent(JsonConvert.SerializeObject(response)), HttpStatusCode.OK, headers, "some reason");
    }
}
