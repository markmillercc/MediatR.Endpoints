namespace MediatR.Endpoints;

public interface IEndpointConfiguration<TRequest> : IBaseEndpointConfiguration
    where TRequest : IBaseRequest
{ 
} 

public interface IBaseEndpointConfiguration
{
    public void Configure(IEndpointDefinition e);
}

