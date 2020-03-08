using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.StartupConfig.Swagger
{
    public class AddCartsByIdGetExternalDocs : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.ExternalDocs = new OpenApiExternalDocs
            {
                Description = "External docs for CartsByIdGet",
                Url = new Uri("https://tempuri.org/carts-by-id-get")
            };
        }
    }
}
