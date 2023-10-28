using Moq;
using System.Reflection;

namespace MediatR.Endpoints.Tests
{
    public static class GetMock 
    {
        public static Assembly Assembly(params Type[] types)
        {
            var mock = new Mock<Assembly>();
            //var t = new List<Type>();
            //foreach (var type in types) 
            //{                
            //    var mt = new Mock<Type>();
            //    mt.Setup(a => a.)
            //    mt.Setup(a => a.Assembly).Returns(mock.Object);
            //    t.Add(mt.Object);
            //}
            
            mock.Setup(a => a.GetTypes()).Returns(types);
            mock.Setup(a => a.DefinedTypes).Returns(types.Select(t => t.GetTypeInfo()));
            return mock.Object;
        }

    }
}