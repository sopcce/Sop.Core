using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class AssignOperationVendorExtensions : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //operation.Extensions.Add("x-purpose", new OpenApiString("test"));
        }
    }
}