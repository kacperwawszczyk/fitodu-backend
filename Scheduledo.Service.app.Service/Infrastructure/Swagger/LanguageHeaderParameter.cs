using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Scheduledo.Service.Infrastructure.Swagger
{
    public class LanugageHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "Language",
                In = "header",
                Description = "User language",
                Required = false,
                Type = "string"
            });
        }
    }
}
