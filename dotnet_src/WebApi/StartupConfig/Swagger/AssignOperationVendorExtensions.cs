using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.StartupConfig.Swagger
{
    public class AssignOperationVendorExtensions : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Extensions.Add("x-purpose", new OpenApiString("test"));
        }
    }
}
