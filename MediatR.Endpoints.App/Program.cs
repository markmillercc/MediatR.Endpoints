using MediatR.Endpoints;
using MediatR.Endpoints.App;
using MediatR.Endpoints.App.Advanced;
using MediatR.Endpoints.App.Basic;
using System.Reflection;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => Regex.Replace(type.ToString(), "[^a-zA-Z0-9 -]", "")); });

builder.Services.AddScoped<SomeDependency>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatREndoints(cfg =>
{    
    cfg.UseEndpointHandlerDelegateFactory<MyHandlerFactory>();

    cfg.AddRouteGroupBuilder(a => a.AddEndpointFilter<GlobalEndpointFilter>());

    cfg.AddRouteGroupBuilder("testGroup", a => a.AddEndpointFilter<EndpointFilter3>().AddEndpointFilter<EndpointFilter2>());

    cfg.MapGet<GetRequestMappedByServicesAllDefaults>();
    cfg.MapGet<GetRequestMappedByServicesCustomRouteAndGroupAndBuilder>("cool/get/route", "coolGroup", a => a.AddEndpointFilter<EndpointFilter3>());

    cfg.MapPost<PostRequestMappedByServicesAllDefaults>();
    cfg.MapPost<PostRequestMappedByServicesCustomRouteAndGroupAndBuilder>("cool/post/route", "coolGroup", a => a.AddEndpointFilter<EndpointFilter3>());

    cfg.MapPatch<PatchRequestMappedByServicesAllDefaults>();
    cfg.MapPatch<PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder>("cool/patch/route", "coolGroup", a => a.AddEndpointFilter<EndpointFilter3>());

    cfg.MapPut<PutRequestMappedByServicesAllDefaults>();
    cfg.MapPut<PutRequestMappedByServicesCustomRouteAndGroupAndBuilder>("cool/put/route", "coolGroup", a => a.AddEndpointFilter<EndpointFilter3>());

    cfg.MapDelete<DeleteRequestMappedByServicesAllDefaults>();
    cfg.MapDelete<DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder>("cool/delete/route", "coolGroup", a => a.AddEndpointFilter<EndpointFilter3>());

    cfg.MapPatch<PatchWithCustomHandlerAndDependency.Query>("custom/handler/for/patch/{id}/{name}", handler:
        async (SomeDependency dependency, MediatR.IMediator mediator, int id, string name) =>
        {
            var request = new PatchWithCustomHandlerAndDependency.Query
            {
                Id = id,
                Name = dependency.DoSomething(name),
                Date = DateTime.Now.AddDays(3).Date
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapMediatREndpoints();

app.Run();



public partial class Program { }

