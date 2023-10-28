namespace MediatR.Endpoints.App.Advanced;

public class GetRequestInGroup1
{
    [Endpoint(Method.Get, "first", Group = "testGroup")]
    public class Command : IRequest<string> { }
    public class Handler : IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
           => Task.FromResult(typeof(Command).FullName + " handled");
    }
}



