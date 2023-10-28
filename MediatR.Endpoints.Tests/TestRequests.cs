using Microsoft.AspNetCore.Http;

namespace MediatR.Endpoints.Tests;

////public class GetSomething : IRequest<string> { }
////public class GetSomethingHandler : IRequestHandler<GetSomething, string>
////{
////    public Task<string> Handle(GetSomething request, CancellationToken cancellationToken) => Task.FromResult("Handled");
////}

//public class DoSomething : IRequest { }
//public class DoSomethingHandler : IRequestHandler<DoSomething>
//{
//    public Task Handle(DoSomething request, CancellationToken cancellationToken) => Task.CompletedTask;
//}

//[Endpoint(Method.Get)]
//public class AttributedGetSomething : IRequest<string> { }
//public class AttributedGetSomethingHandler : IRequestHandler<AttributedGetSomething, string>
//{
//    public Task<string> Handle(AttributedGetSomething request, CancellationToken cancellationToken) => Task.FromResult("Handled");
//}

//[Endpoint(Method.Post)]
//public class AttributedDoSomething : IRequest { }
//public class AttributedDoSomethingHandler : IRequestHandler<AttributedDoSomething>
//{
//    public Task Handle(AttributedDoSomething request, CancellationToken cancellationToken) => Task.CompletedTask;
//}


//public class GetSomethingElse : IRequest<string> { }
//public class GetSomethingElseRequestHandler : TestRequestHandlerStringResult<GetSomethingElse> { }

//public class DoSomething : IRequest<string> { }
//public class DoSomethingRequestHandler : TestRequestHandlerStringResult<DoSomething> { }

//public class DoSomethingElse : IRequest<string> { }
//public class DoSomethingElseRequestHandler : TestRequestHandlerStringResult<DoSomethingElse> { }


//[Endpoint(Method.Get, "my/custom/route")]
//public class AttributedGetSomethingWithCustomRoute : IRequest<string> { }
//public class AttributedGetSomethingRequestHandler : IRequestHandler<AttributedGetSomethingWithCustomRoute, string>
//{
//    public Task<string> Handle(AttributedGetSomethingWithCustomRoute request, CancellationToken cancellationToken)
//    => Task.FromResult("Handled");
//}


//[Endpoint(Method.Get, "my/custom/route", Group = "MyGroup")]
//public class AttributedGetSomethingWithCustomRouteAndGroup : IRequest<string> { }
//public class AttributedGetSomethingWithCustomRouteAndGroupRequestHandler : IRequestHandler<AttributedGetSomethingWithCustomRouteAndGroup, string> 
//{
//    public Task<string> Handle(AttributedGetSomethingWithCustomRouteAndGroup request, CancellationToken cancellationToken)
//    => Task.FromResult($"{typeof(AttributedGetSomethingWithCustomRouteAndGroup).Name} Handled");
//}


//[Endpoint(Method.Get, "my/other/custom/route")]
//public class AttributedGetSomethingElseWithCustomRoute : IRequest<string> { }
//public class AttributedGetSomethingElseWithCustomRouteRequestHandler : TestRequestHandlerStringResult<AttributedGetSomethingElseWithCustomRoute> { }


//[Endpoint(Method.Get, "my/other/custom/route", Group = "MyOtherGroup")]
//public class AttributedGetSomethingElseWithCustomRouteAndGroup : IRequest<string> { }
//public class AttributedGetSomethingElseWithCustomRouteAndGroupRequestHandler : TestRequestHandlerStringResult<AttributedGetSomethingElseWithCustomRouteAndGroup> { }

//public class OperationWithoutHandler : IRequest<string> { }

//public class OperationWithMultipleHandlers : IRequest<string> { }
//public class OperationWithMultipleHandlersRequestHandler1 : TestRequestHandlerStringResult<OperationWithMultipleHandlers> { }
//public class OperationWithMultipleHandlersRequestHandler2 : TestRequestHandlerStringResult<OperationWithMultipleHandlers> { }


//public abstract class TestRequestHandlerStringResult<TRequest> : IRequestHandler<TRequest, string>
//    where TRequest : IRequest<string>
//{
//    public Task<string> Handle(TRequest request, CancellationToken cancellationToken)
//    => Task.FromResult($"{typeof(TRequest).Name} Handled");
//}

//public class UselessEndpointFilter : IEndpointFilter
//{
//    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
//    {
//        return await next(context);
//    }
//}
