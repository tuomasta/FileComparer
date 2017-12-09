using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Swashbuckle.AspNetCore.Swagger;

namespace OrderApi
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Tags.Contains("FileUpload"))
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "id",
                    In = "path",
                    Description = "Id",
                    Required = true,
                    Type = "string"
                });
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "file",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("application/form-data");
            }
        }
    }
}
