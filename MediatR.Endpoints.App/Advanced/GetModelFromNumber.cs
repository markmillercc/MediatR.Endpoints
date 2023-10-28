namespace MediatR.Endpoints.App.Advanced;

public class GetModelFromNumber
{
    [Endpoint(Method.Get, "models/get/{number}")]
    public class Query : IRequest<Model>
    {
        public int Number { get; set; }
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

    public class Handler : IRequestHandler<Query, Model>
    {
        public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            => Task.FromResult(new Model
            {
                Number = request.Number,
                Name = $"{request.Number}name",
                Description = $"{request.Number}description",
                Words = new string[] { "word1", "word2", "word3" },
                Submodels = new Submodel[]
                {
                    new Submodel {Code = 289128372, Name = "codename1", Date = DateTime.Now },
                    new Submodel {Code = 918289345, Name = "codename2", Date = DateTime.Now.AddDays(1) },
                    new Submodel {Code = 565746373, Name = "codename3", Date = DateTime.Now.AddDays(2) },
                }
            });
    }
}



