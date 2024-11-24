using Microsoft.OpenApi.Models;

namespace DAMProject.Server.Configuration
{
    public static class SwaggerConfiguration
    {
        public static WebApplicationBuilder ConfigureCustomSwagger(this WebApplicationBuilder builder)
        {            
            builder.Services.AddSwaggerGen(c =>
            {
                c.OperationFilter<FileUploadOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlazorWebAssemblyTemplate.Server", Version = "v1" });
            });

            return builder;
        }
    }
}
