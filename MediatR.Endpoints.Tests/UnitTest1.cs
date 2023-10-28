using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Reflection;

namespace MediatR.Endpoints.Tests
{
    public class AttributeTests
    {
        [Endpoint(Method.Get, Group = "mygroup")]
        public class GetSomething : IRequest<string> { }
        public class GetSomethingHandler : IRequestHandler<GetSomething, string>
        {
            public Task<string> Handle(GetSomething request, CancellationToken cancellationToken) => Task.FromResult("Handled");
        }

        [Endpoint(Method.Post, "my/custom/route")]
        public class DoSomething : IRequest { }
        public class DoSomethingHandler : IRequestHandler<DoSomething>
        {
            public Task Handle(DoSomething request, CancellationToken cancellationToken) => Task.CompletedTask;
        }

        [Fact]
        public void Should_auto_map_attributed_requests()
        {
            var assembly = GetMock.Assembly(
                typeof(GetSomething), typeof(GetSomethingHandler),
                typeof(DoSomething), typeof(DoSomethingHandler));

            using var app = new TestWebApplication(services =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
                services.AddMediatREndoints(assembly, cfg =>
                {
                    cfg.MapGet<GetSomething>(group: "mygroup", builder: a => a.RequireAuthorization());
                });
            });

            var endpoints = app.Endpoints.ToList();
            endpoints.Count.ShouldBe(2);
            endpoints.ShouldAllBe(e => e.RequestDelegate != null);

            var getendpoint = endpoints.Single(e => e.RoutePattern.RawText == "mygroup/MediatR/Endpoints/Tests/AttributeTestsGetSomething");
            getendpoint.Metadata.GetMetadata<HttpMethodMetadata>().HttpMethods[0].ShouldBe("GET");

            var postendpoint = endpoints.Single(e => e.RoutePattern.RawText == "/my/custom/route");
            postendpoint.Metadata.GetMetadata<HttpMethodMetadata>().HttpMethods[0].ShouldBe("POST");
        }
            //var w = Host.CreateDefaultBuilder()               
            //    .ConfigureServices(services =>
            //    {
            //        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            //        services.AddMediatREndoints(cfg =>
            //        {
            //            cfg.MapGet<GetSomething>();
            //            cfg.MapGet<GetSomethingElse>();
            //            cfg.MapPost<DoSomething>();
            //            cfg.MapPost<DoSomethingElse>();
            //        });
            //    })
            //    .ConfigureWebHost(app => 
            //    {
            //        var r = app.Build();
            //        app.
            //    })
            //    .Build();

            //w.M

            //var s = w.Services.GetRequiredService<IMediator>();


            //var s = _factory.Server.Services.GetRequiredService<IEnumerable<EndpointDataSource>>().ToList();


            //var services = new ServiceCollection();
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(GetMock.Assembly(typesInMockAssembly)));
            //services.AddMediatREndoints(cfg =>
            //{
            //    cfg.MapGet<GetSomething>();
            //    cfg.MapGet<GetSomethingElse>();
            //    cfg.MapPost<DoSomething>();
            //    cfg.MapPost<DoSomethingElse>();
            //});

            //Registrar.Registrations.Count().ShouldBe(4);


            //config.RegisterFromAssemblies();

            //Registrar.Initialize(config);

            //var registrations = Registrar.Registrations.ToList();

            //registrations.Count.ShouldBe(registerFromAssembly ? typesInMockAssembly.Length : 0);

            //if (registerFromAssembly)
            //    typesInMockAssembly.ToList().ForEach(t => registrations.ShouldContain(a => a.RequestType == t));

            //app.StopAsync();
            // app.DisposeAsync();

       // }

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void Should_register_attributed_operations_only_when_configured(bool registerFromAssembly)
        //{
        //    var config = new OperatRConfiguration
        //    {
        //        RegisterFromAssembly = registerFromAssembly
        //    };

        //    var typesInMockAssembly = new Type[]
        //    {
        //        typeof(GetSomething),
        //        typeof(GetSomethingElse),
        //        typeof(AttributedGetSomethingWithCustomRoute),
        //        typeof(AttributedGetSomethingElseWithCustomRoute)
        //    };

        //    var registrar = new Registrar(config, GetMockAssembly(typesInMockAssembly));

        //    var registrations = registrar.OperationRegistrations.ToList();
        //    registrations.Count.ShouldBe(registerFromAssembly ? 4 : 2);

        //    registrations.ShouldContain(a =>
        //        a.OperationRequestType == typeof(AttributedGetSomethingWithCustomRoute) &&
        //        a.Route == "my/custom/route" &&
        //        a.Group == "");

        //    registrations.ShouldContain(a =>
        //    a.OperationRequestType == typeof(AttributedGetSomethingElseWithCustomRoute) &&
        //        a.Route == "my/other/custom/route" &&
        //        a.Group == "");

        //    if (registerFromAssembly)
        //    {
        //        registrations.ShouldContain(a =>
        //        a.OperationRequestType == typeof(GetSomething) &&
        //        a.Route == "OperatR/Tests/GetSomething" &&
        //        a.Group == "");

        //        registrations.ShouldContain(a =>
        //        a.OperationRequestType == typeof(GetSomethingElse) &&
        //            a.Route == "OperatR/Tests/GetSomethingElse" &&
        //            a.Group == "");
        //    }
        //}

        //[Fact]
        //public void Should_register_manually()
        //{
        //    var config = new OperatRConfiguration();

        //    config.MapGet<GetSomething>();
        //    config.MapGet<GetSomethingElse>();

        //    var registrar = new Registrar(config, GetMockAssembly(Array.Empty<Type>()));

        //    var registrations = registrar.OperationRegistrations.ToList();

        //    registrations.Count.ShouldBe(2);
        //}

        //[Theory]
        //[InlineData(true)]
        //[InlineData(false)]
        //public void Should_register_from_various_sources(bool registerFromAssembly)
        //{
        //    var config = new OperatRConfiguration
        //    {
        //        RegisterFromAssembly = registerFromAssembly
        //    };

        //    var typesInMockAssembly = new Type[]
        //    {
        //        typeof(GetSomething),
        //        typeof(AttributedGetSomethingWithCustomRoute)
        //    };
        //    config.MapGet<GetSomethingElse>();

        //    var registrar = new Registrar(config, GetMockAssembly(typesInMockAssembly));

        //    var registrations = registrar.OperationRegistrations.ToList();

        //    registrations.Count.ShouldBe(registerFromAssembly ? 3 : 2);

        //    registrations.ShouldContain(a => a.OperationRequestType == typeof(GetSomethingElse));
        //    registrations.ShouldContain(a => a.OperationRequestType == typeof(AttributedGetSomethingWithCustomRoute));

        //    if (registerFromAssembly)
        //    {
        //        registrations.ShouldContain(a => a.OperationRequestType == typeof(GetSomething));
        //    }

        //}

        //[Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //public void Should_consolidate_actions(int numberOfActions)
        //{
        //    var config = new OperatRConfiguration();

        //    if (numberOfActions > 0)
        //        config.MapGet<GetSomething>(a => a.WithDescription("with description"));

        //    if (numberOfActions > 1)
        //        config.MapGet<GetSomething>(a => a.WithName("with name"));

        //    if (numberOfActions > 2)
        //        config.MapGet<GetSomething>(a => a.WithDisplayName("with display name"));

        //    var registrar = new Registrar(config, GetMockAssembly(typeof(GetSomething)));

        //    var registrations = registrar.OperationRegistrations.ToList();

        //    registrations.Count.ShouldBe(1);

        //    var action = registrations.Single().RouteHandlerBuilder;
        //    if (numberOfActions == 0)
        //    {
        //        action.ShouldBeNull();
        //    }
        //    else
        //    {
        //        action.ShouldNotBeNull();
        //        action.GetInvocationList().Length.ShouldBe(numberOfActions);
        //    }
        //}
    }
}