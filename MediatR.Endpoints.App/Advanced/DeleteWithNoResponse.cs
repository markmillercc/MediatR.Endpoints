namespace MediatR.Endpoints.App.Advanced;

public class DeleteWithNoResponse
{
    [Endpoint(Method.Delete, "delete/{number}")]
    public class Command : IRequest
    {
        public int Number { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        public Task Handle(Command request, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}



