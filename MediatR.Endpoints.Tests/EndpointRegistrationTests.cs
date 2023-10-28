using Shouldly;
using System.Text.RegularExpressions;

namespace MediatR.Endpoints.Tests
{
    public class EndpointRegistrationTests
    {
        public class GetSomething : IRequest<string> { }
        public class GetSomethingHandler : IRequestHandler<GetSomething, string>
        {
            public Task<string> Handle(GetSomething request, CancellationToken cancellationToken) => Task.FromResult("Handled");
        }

        public class PutSomething : IRequest { }
        public class PutSomethingHandler : IRequestHandler<PutSomething>
        {
            public Task Handle(PutSomething request, CancellationToken cancellationToken) => Task.CompletedTask;
        }

        [Theory]
        [InlineData(typeof(GetSomething), typeof(string))]
        [InlineData(typeof(PutSomething), null)]  
        public void Should_create_endpoint_registration_from_type(Type requestType, Type responseType)
        {
            var get = new EndpointRegistration(requestType);

            get.RequestType.ShouldBe(requestType);
            get.ResponseType.ShouldBe(responseType);
        }

        [Fact]
        public void Should_create_endpoint_registration_from_type_and_attribute()
        {
            var getAttribute = new EndpointAttribute(Method.Get, "the/get") { Group = "TheGet" };
            var get = new EndpointRegistration(typeof(GetSomething), getAttribute);

            get.RequestType.ShouldBe(typeof(GetSomething));
            get.ResponseType.ShouldBe(typeof(string));
            get.HttpMethod.ShouldBe(Method.Get);
            get.Route.ShouldBe("the/get");
            get.Group.ShouldBe("TheGet");

            var putAttribute = new EndpointAttribute(Method.Put, "the/put") { Group = "ThePut" };
            var put = new EndpointRegistration(typeof(PutSomething), putAttribute);

            put.RequestType.ShouldBe(typeof(PutSomething));
            put.ResponseType.ShouldBeNull();
            put.HttpMethod.ShouldBe(Method.Put);
            put.Route.ShouldBe("the/put");
            put.Group.ShouldBe("ThePut");
        }

    }
}