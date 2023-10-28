using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Endpoints.Tests
{
    public class TestWebApplication : IDisposable
    {
        public WebApplication Application { get; }
        public RouteEndpoint[] Endpoints => Application.Services.GetRequiredService<IEnumerable<EndpointDataSource>>()
            .SelectMany(x => x.Endpoints)
            .Cast<RouteEndpoint>()
            .ToArray();

        public TestWebApplication(Action<IServiceCollection> servicesAction)//, Action<WebApplication> webAppAction)
        {
            var builder = WebApplication.CreateBuilder();

            servicesAction.Invoke(builder.Services);

            Application = builder.Build();

            Application.MapMediatREndpoints();
            //webAppAction.Invoke(Application);

            Application.RunAsync();
        }

        public void Dispose()
        {            
            Application.StopAsync();
            Application.DisposeAsync();
        }
    }
}