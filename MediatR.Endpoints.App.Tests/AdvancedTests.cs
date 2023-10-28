using MediatR.Endpoints.App.Advanced;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Text;
using Xunit;

namespace MediatR.Endpoints.App.Tests;

public class AdvancedTests : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;
    public AdvancedTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;        
    }

    [Fact]
    public async Task Should_put_with_custom_handler()
    {
        var number = 123;
        var name = "thename";
        var description = "thedescription";

        var url = $"custom/handler/for/put/{number}";
        var modelIn = new PutWithCustomHandler.ModelIn
        {
            Name = name,
            Description = description
        };

        var body = new StringContent(JsonConvert.SerializeObject(modelIn), Encoding.UTF8, "application/json");

        using var client = _factory.CreateClient();

        var response = await client.PutAsync(url, body);

        response.IsSuccessStatusCode.ShouldBeTrue();

        var modelOut = JsonConvert.DeserializeObject<PutWithCustomHandler.ModelOut>(await response.Content.ReadAsStringAsync());

        modelOut.ShouldNotBeNull();
        modelOut.Number.ShouldBe(number);
        modelOut.Name.ShouldBe(name);
        modelOut.Description.ShouldBe(description);

        response.Headers.TryGetValues("EndpointFilters", out IEnumerable<string> filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(2);
        filtersHit.ElementAt(0).ShouldBe(nameof(GlobalEndpointFilter));
        filtersHit.ElementAt(1).ShouldBe(nameof(EndpointFilter1));
    }

    [Fact]
    public async Task Should_patch_with_custom_handler_and_dependency()
    {
        var id = 123;
        var name = "thename";

        var url = $"custom/handler/for/patch/{id}/{name}";

        using var client = _factory.CreateClient();

        var response = await client.PatchAsync(url, null);

        response.IsSuccessStatusCode.ShouldBeTrue(response.ToString());

        var dependency = new SomeDependency();
        (await response.Content.ReadAsStringAsync())
            .ShouldBe($"\"{id}: {dependency.DoSomething(name)} on {DateTime.Now.AddDays(3).Date}\"");

        response.Headers.TryGetValues("EndpointFilters", out IEnumerable<string> filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(1);
        filtersHit.ElementAt(0).ShouldBe(nameof(GlobalEndpointFilter));
    }

    [Fact]
    public async Task Should_get_with_group_filters()
    {
        using var client = _factory.CreateClient();

        var inGroupResponse1 = await client.GetAsync("testGroup/first");
        inGroupResponse1.IsSuccessStatusCode.ShouldBeTrue();
        inGroupResponse1.Headers.TryGetValues("EndpointFilters", out IEnumerable<string> filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(3);
        filtersHit.ElementAt(0).ShouldBe(nameof(GlobalEndpointFilter));
        filtersHit.ElementAt(1).ShouldBe(nameof(EndpointFilter3));
        filtersHit.ElementAt(2).ShouldBe(nameof(EndpointFilter2));
        filtersHit = null;

        var inGroupResponse2 = await client.GetAsync("testGroup/second");
        inGroupResponse2.IsSuccessStatusCode.ShouldBeTrue();
        inGroupResponse2.Headers.TryGetValues("EndpointFilters", out filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(3);
        filtersHit.ElementAt(0).ShouldBe(nameof(GlobalEndpointFilter));
        filtersHit.ElementAt(1).ShouldBe(nameof(EndpointFilter3));
        filtersHit.ElementAt(2).ShouldBe(nameof(EndpointFilter2));
        filtersHit = null;

        var outGroupResponse = await client.GetAsync("someGroup/some/custom/get/route");
        outGroupResponse.IsSuccessStatusCode.ShouldBeTrue();
        outGroupResponse.Headers.TryGetValues("EndpointFilters", out filtersHit).ShouldBeTrue();
        filtersHit.Count().ShouldBe(1);
        filtersHit.ElementAt(0).ShouldBe(nameof(GlobalEndpointFilter));
    }

    [Fact]
    public async Task Should_get_model_from_number()
    {
        var number = "5";
        using var client = _factory.CreateClient();
        var response = await client.GetAsync($"models/get/{number}");

        response.IsSuccessStatusCode.ShouldBeTrue();
        var content = await response.Content.ReadAsStringAsync();
        var model = JsonConvert.DeserializeObject<GetModelFromNumber.Model>(content);

        model.ShouldNotBeNull();
        model.Number.ToString().ShouldBe(number);
        model.Name.ShouldContain(number);
        model.Description.ShouldContain(number);
        model.Words.Length.ShouldBe(3);
        model.Submodels.Length.ShouldBe(3);
    }

    [Fact]
    public async Task Should_post_model_to_number()
    {
        var command = new PostModelToNumber.Command
        { 
            Model = new PostModelToNumber.Model
            {
                Number = 2,
                Name = "name",
                Description = "description",  
                Words = new string[] {"w1", "w2", "w3" },
                Submodels = new PostModelToNumber.Submodel[]
                {
                    new PostModelToNumber.Submodel {Code = 1, Name = "c1", Date = DateTime.Now },
                    new PostModelToNumber.Submodel {Code = 2, Name = "c2", Date = DateTime.Now.AddDays(-1) },
                    new PostModelToNumber.Submodel {Code = 3, Name = "c3", Date = DateTime.Now.AddDays(-2) },
                }
            }
        };

        var body = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

        using var client = _factory.CreateClient();
        var response = await client.PostAsync("models/post", body);
        
        response.IsSuccessStatusCode.ShouldBeTrue(response.ToString());
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe($"{command.Model.Number}");
    }

    [Fact]
    public async Task Should_delete_with_no_response()
    {
        using var client = _factory.CreateClient();
        var response = await client.DeleteAsync("delete/5");
        response.IsSuccessStatusCode.ShouldBeTrue();
        (await response.Content.ReadAsStringAsync()).ShouldBeEmpty();
    }

}




























//public async Task<T> GetShouldSucceed<T>(string url)
//{
//    using var client = _factory.CreateClient();
//    var response = await client.GetAsync(url);

//    return await ShouldBeOkStatusCode<T>(response);
//}

//public async Task<T> PostShouldSucceed<T>(string url, object request)
//{
//    var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
//    using var client = _factory.CreateClient();
//    var response = await client.PostAsync(url, body);            

//    return await ShouldBeOkStatusCode<T>(response);
//}

//public async Task<string[]> PostShouldFail<T>(string url, IRequest<T> request, HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
//{
//    var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
//    using var client = _factory.CreateClient();
//    var response = await client.PostAsync(url, body);

//    return await ShouldBeFailureStatusCode(response, expectedStatusCode);

//}

//public async Task<string[]> GetShouldFail(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
//{
//    using var client = _factory.CreateClient();
//    var response = await client.GetAsync(url);

//    return await ShouldBeFailureStatusCode(response, expectedStatusCode);
//}

//private static async Task<string[]> ShouldBeFailureStatusCode(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
//{
//    response.IsSuccessStatusCode.ShouldBeFalse(response.StatusCode.ToString());

//    if (response.StatusCode != expectedStatusCode)
//    {
//        Assert.Fail($"Response should have had status code {expectedStatusCode} ({(int)expectedStatusCode}) but did not\n{DescribeResponse(response)}");
//    }

//    response.Content.ShouldNotBeNull();

//    var content = await response.Content.ReadAsStringAsync();

//    var errors = JsonConvert.DeserializeAnonymousType(content, new { Errors = Array.Empty<string>() })
//        ?.Errors;

//    return errors;
//}

//private static async Task<T> ShouldBeOkStatusCode<T>(HttpResponseMessage responseMessage)
//{
//    if (responseMessage.IsSuccessStatusCode)
//    {
//        var content = await responseMessage.Content.ReadAsStringAsync();
//        try
//        {
//            var result = JsonConvert.DeserializeObject<T>(content);
//            return result;
//        }
//        catch (Exception ex)
//        {
//            Assert.Fail($"Status code was Ok but could not deserialize content to type '{typeof(T)}'\n{ex}");
//        }
//    }

//    Assert.Fail($"Response should have had Ok status code but did not\n{DescribeResponse(responseMessage)}");

//    return default;
//}

//private static string DescribeResponse(HttpResponseMessage responseMessage)
//{
//    var requestMethod = responseMessage.RequestMessage?.Method?.Method ?? "Unknown";
//    var requestUrl = responseMessage.RequestMessage?.RequestUri?.AbsolutePath ?? "Unknown";

//    var response = "Response was null";
//    if (responseMessage != null)
//    {
//        var statusCodeName = responseMessage.StatusCode.ToString();
//        var statusCode = (int)responseMessage.StatusCode;
//        var content = responseMessage.Content.ReadAsStringAsync().Result;

//        if (string.IsNullOrEmpty(content))
//        {
//            content = "No content";
//        }
//        else
//        {
//            content = JToken.Parse(content)
//              .ToString(Formatting.Indented)
//              .Replace("\\r\\n", "\n").Replace("\\t", "\t");
//        }

//        response = $"Response: {statusCodeName} ({statusCode})\n{content}";
//    }
//    return $"{requestMethod} {requestUrl}\n{response}";
//}



//public class TestsWebApplicationFactory<TProgram>
//    : WebApplicationFactory<TProgram> where TProgram : class
//{

//    public Action<IWebHostBuilder> MockWebHostBuilder { get; set; } = a => { };

//    protected override void ConfigureWebHost(IWebHostBuilder builder)
//    {
//        MockWebHostBuilder(builder);
//    }

//    public async Task<T> PostShouldSucceed<T>(string url, IRequest<T> request)
//    {
//        var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
//        using var client = CreateClient();
//        var response = await client.PostAsync(url, body);

//        return await ShouldBeOkStatusCode<T>(response);
//    }

//    public async Task<string[]> PostShouldFail<T>(string url, IRequest<T> request)
//    {
//        var body = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
//        using var client = CreateClient();
//        var response = await client.PostAsync(url, body);

//        return await ShouldBeBadRequestStatusCode(response);

//    }

//    private static async Task<T> ShouldBeOkStatusCode<T>(HttpResponseMessage responseMessage)
//    {
//        if (responseMessage.IsSuccessStatusCode)
//        {
//            var content = await responseMessage.Content.ReadAsStringAsync();
//            try
//            {
//                var result = JsonConvert.DeserializeObject<T>(content);
//                return result;
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"Status code was Ok but could not deserialize content to type '{typeof(T)}'\n{ex}");
//            }
//        }

//        Assert.Fail($"Response should have had Ok status code but did not\n{DescribeResponse(responseMessage)}");

//        return default;
//    }

//    private static async Task<string[]> ShouldBeBadRequestStatusCode(HttpResponseMessage response)
//    {
//        response.IsSuccessStatusCode.ShouldBeFalse(response.StatusCode.ToString());

//        if (response.StatusCode != HttpStatusCode.BadRequest)
//        {
//            Assert.Fail($"Response should have had status code BadRequest (400) but did not\n{DescribeResponse(response)}");
//        }

//        response.Content.ShouldNotBeNull();

//        var content = await response.Content.ReadAsStringAsync();

//        var errors = JsonConvert.DeserializeAnonymousType(content, new { Errors = Array.Empty<string>() })
//            ?.Errors;

//        return errors;
//    }

//    private static string DescribeResponse(HttpResponseMessage responseMessage)
//    {
//        var requestMethod = responseMessage.RequestMessage?.Method?.Method ?? "Unknown";
//        var requestUrl = responseMessage.RequestMessage?.RequestUri?.AbsolutePath ?? "Unknown";

//        var response = "Response was null";
//        if (responseMessage != null)
//        {
//            var statusCodeName = responseMessage.StatusCode.ToString();
//            var statusCode = (int)responseMessage.StatusCode;
//            var content = responseMessage.Content.ReadAsStringAsync().Result;

//            if (string.IsNullOrEmpty(content))
//            {
//                content = "No content";
//            }
//            else
//            {
//                content = JToken.Parse(content)
//                  .ToString(Formatting.Indented)
//                  .Replace("\\r\\n", "\n").Replace("\\t", "\t");
//            }

//            response = $"Response: {statusCodeName} ({statusCode})\n{content}";
//        }
//        return $"{requestMethod} {requestUrl}\n{response}";
//    }
//}
