using MediatR.Endpoints.App.Basic;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Text;
using Xunit;

namespace MediatR.Endpoints.App.Tests;
public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData(Method.Get, typeof(GetRequestMappedByAttributeAllDefaults))]
    [InlineData(Method.Get, typeof(GetRequestMappedByConfigAllDefaults))]
    [InlineData(Method.Get, typeof(GetRequestMappedByServicesAllDefaults))]
    [InlineData(Method.Get, typeof(GetRequestMappedByAttributeCustomRouteAndGroup), "some/custom/get/route", "someGroup")]
    [InlineData(Method.Get, typeof(GetRequestMappedByConfigCustomRouteAndGroupAndBuilder), "another/custom/get/route", "anotherGroup", nameof(EndpointFilter1), nameof(EndpointFilter2))]
    [InlineData(Method.Get, typeof(GetRequestMappedByServicesCustomRouteAndGroupAndBuilder), "cool/get/route", "coolGroup", nameof(EndpointFilter3))]
    [InlineData(Method.Post, typeof(PostRequestMappedByAttributeAllDefaults))]
    [InlineData(Method.Post, typeof(PostRequestMappedByConfigAllDefaults))]
    [InlineData(Method.Post, typeof(PostRequestMappedByServicesAllDefaults))]
    [InlineData(Method.Post, typeof(PostRequestMappedByAttributeCustomRouteAndGroup), "some/custom/post/route", "someGroup")]
    [InlineData(Method.Post, typeof(PostRequestMappedByConfigCustomRouteAndGroupAndBuilder), "another/custom/post/route", "anotherGroup", nameof(EndpointFilter1), nameof(EndpointFilter2))]
    [InlineData(Method.Post, typeof(PostRequestMappedByServicesCustomRouteAndGroupAndBuilder), "cool/post/route", "coolGroup", nameof(EndpointFilter3))]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByAttributeAllDefaults))]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByConfigAllDefaults))]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByServicesAllDefaults))]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByAttributeCustomRouteAndGroup), "some/custom/patch/route", "someGroup")]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder), "another/custom/patch/route", "anotherGroup", nameof(EndpointFilter1), nameof(EndpointFilter2))]
    [InlineData(Method.Patch, typeof(PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder), "cool/patch/route", "coolGroup", nameof(EndpointFilter3))]
    [InlineData(Method.Put, typeof(PutRequestMappedByAttributeAllDefaults))]
    [InlineData(Method.Put, typeof(PutRequestMappedByConfigAllDefaults))]
    [InlineData(Method.Put, typeof(PutRequestMappedByServicesAllDefaults))]
    [InlineData(Method.Put, typeof(PutRequestMappedByAttributeCustomRouteAndGroup), "some/custom/put/route", "someGroup")]
    [InlineData(Method.Put, typeof(PutRequestMappedByConfigCustomRouteAndGroupAndBuilder), "another/custom/put/route", "anotherGroup", nameof(EndpointFilter1), nameof(EndpointFilter2))]
    [InlineData(Method.Put, typeof(PutRequestMappedByServicesCustomRouteAndGroupAndBuilder), "cool/put/route", "coolGroup", nameof(EndpointFilter3))]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByAttributeAllDefaults))]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByConfigAllDefaults))]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByServicesAllDefaults))]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByAttributeCustomRouteAndGroup), "some/custom/delete/route", "someGroup")]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder), "another/custom/delete/route", "anotherGroup", nameof(EndpointFilter1), nameof(EndpointFilter2))]
    [InlineData(Method.Delete, typeof(DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder), "cool/delete/route", "coolGroup", nameof(EndpointFilter3))]
    public async Task Should_reach_endpoint(Method httpMethod, Type requestType, string expectedRoute = null, string expectedGroup = "", params string[] expectedFilters)
    {
        expectedFilters = new[] { nameof(GlobalEndpointFilter) }.Concat(expectedFilters).ToArray();

        expectedRoute ??= $"MediatR/Endpoints/App/Basic/{requestType.Name}";

        var response = await GetResponse(httpMethod, requestType, expectedRoute, expectedGroup);

        response.IsSuccessStatusCode.ShouldBeTrue($"{httpMethod}-> {expectedGroup}/{expectedRoute}: {response}");
        (await response.Content.ReadAsStringAsync()).ShouldBe($"{requestType.FullName} handled");

        response.Headers.TryGetValues("EndpointFilters", out IEnumerable<string> filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(expectedFilters.Length);
        for (var i = 0; i < expectedFilters.Length; i++)
        {
            filtersHit.ElementAt(i).ShouldBe(expectedFilters.ElementAt(i));
        }
    }

    private async Task<HttpResponseMessage> GetResponse(Method httpMethod, Type requestType, string route, string group)
    {
        var url = $"{group}/{route}";
        var body = new StringContent(JsonConvert.SerializeObject(Activator.CreateInstance(requestType)), Encoding.UTF8, "application/json");

        using var client = _factory.CreateClient();
        return httpMethod switch
        {
            Method.Get => await client.GetAsync(url),
            Method.Delete => await client.DeleteAsync(url),
            Method.Post => await client.PostAsync(url, body),
            Method.Patch => await client.PatchAsync(url, body),
            Method.Put => await client.PutAsync(url, body),
            _ => throw new Exception(),
        };
    }
}   
