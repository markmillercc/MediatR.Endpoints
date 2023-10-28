namespace MediatR.Endpoints.App.Advanced;

public class PatchWithCustomHandlerAndDependency
{
    public class Query : IRequest<string>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class Handler : IRequestHandler<Query, string>
    {
        public Task<string> Handle(Query request, CancellationToken cancellationToken)
            => Task.FromResult($"{request.Id}: {request.Name} on {request.Date}");
    }
}



