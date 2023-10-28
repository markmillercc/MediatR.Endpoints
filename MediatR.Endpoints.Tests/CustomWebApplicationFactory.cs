//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//namespace MediatR.Endpoints.Tests
//{
//    public class CustomWebApplicationFactory<TProgram>
//    : WebApplicationFactory<TProgram> where TProgram : class
//    {

//        protected override void ConfigureWebHost(IWebHostBuilder builder)
//        {
//            var typesInMockAssembly = new Type[]
//            {
//                typeof(GetSomething), typeof(GetSomethingElse),
//                typeof(DoSomething), typeof(DoSomethingElse)
//            };

//            builder.ConfigureServices(services =>
//            {
//                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(GetMock.Assembly(typesInMockAssembly)));
//                services.AddMediatREndoints(cfg =>
//                {
//                    cfg.MapGet<GetSomething>();
//                    cfg.MapGet<GetSomethingElse>();
//                    cfg.MapPost<DoSomething>();
//                    cfg.MapPost<DoSomethingElse>();
//                });
//            });

//            //var app = builder.Build();
//            // app.Run();
//        }
//    }
//}