using Microsoft.AspNet.OData;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace TestOData.Api
{
    // <summary>
    ///     Add the supported odata parameters for IQueryable endpoints.
    /// </summary>
    public class ODataParametersSwaggerDefinition : IOperationFilter
    {
        /// <summary>
        ///     Apply the filter to the operation.
        /// </summary>
        /// <param name="operation">The API operation to check.</param>
        /// <param name="context">The context of the method assoicated with the API.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.CustomAttributes.Any(ca => ca.AttributeType == typeof(EnableQueryAttribute)))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$filter",
                    Description = "Filter the results using OData syntax.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    In = ParameterLocation.Query
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$orderby",
                    Description = "Order the results using OData syntax.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    In = ParameterLocation.Query
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$skip",
                    Description = "The number of results to skip.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "integer"
                    },
                    In = ParameterLocation.Query
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$top",
                    Description = "The number of results to return.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "integer"
                    },
                    In = ParameterLocation.Query
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$select",
                    Description = "The fields of results to return.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    In = ParameterLocation.Query
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "$expand",
                    Description = "The expanded fields of results to return.",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    In = ParameterLocation.Query
                });
            }
        }
    }
}
