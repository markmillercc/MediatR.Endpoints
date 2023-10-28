namespace MediatR.Endpoints.App.Advanced;

public class PostModelToNumber
{
    [Endpoint(Method.Post, "models/post")]
    public class Command : IRequest<int>
    {
        public Model Model { get; set; }
    }

    public class Model
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Words { get; set; }
        public Submodel[] Submodels { get; set; }
    }

    public class Submodel
    {
        public long Code { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly SomeDependency _dependency;
        public Handler(SomeDependency dependency) => _dependency = dependency;
        public Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Model.Number == 0 || _dependency == null)
                throw new Exception();

            return Task.FromResult(request.Model.Number);
        }
    }
}



