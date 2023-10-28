using Microsoft.AspNetCore.Mvc;

namespace MediatR.Endpoints.App.Advanced;

public class PutWithCustomHandler : IEndpointConfiguration<PutWithCustomHandler.Query>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Put;
        e.Route = "custom/handler/for/put/{number}";
        e.Handler = async (IMediator mediator, int number, [FromBody] ModelIn model) =>
        {
            var request = new Query { Number = number, Model = model };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>();
    }

    public class Query : IRequest<ModelOut>
    {
        public int Number { get; set; }
        public ModelIn Model { get; set; }
    }

    public class ModelIn
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ModelOut : ModelIn
    {
        public int Number { get; set; }
    }

    public class Handler : IRequestHandler<Query, ModelOut>
    {
        public Task<ModelOut> Handle(Query request, CancellationToken cancellationToken)
            => Task.FromResult(new ModelOut { Number = request.Number, Name = request.Model.Name, Description = request.Model.Description });
    }
}



