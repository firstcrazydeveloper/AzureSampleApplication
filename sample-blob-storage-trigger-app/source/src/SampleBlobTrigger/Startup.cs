using FunctionAppBlobStorageTrigger;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]

namespace FunctionAppBlobStorageTrigger
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.AzureFunctions.Extensions;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            //throw new System.NotImplementedException();
        }

        private void RegisterServices(IServiceCollection services)
        {
            
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            var functionAssembly = typeof(Startup).Assembly;
            services.AddAzureFunctionsApiProvider(functionAssembly);

            var info = new Info
            {
                Title = "DeviceManagement",
                Version = "1.0.0",
                Description = "Device Management API",
                Contact = new Contact
                {
                    Name = "Team Avengers",
                },
            };

            // Add Swagger Configuration
            services.AddSwaggerGen(
                options =>
                {
                    // SwaggerDoc - API
                    options.SwaggerDoc("v1.0.0", info);

                    // Add Enums to Swagger as String
                    options.DescribeAllEnumsAsStrings();
                    options.EnableAnnotations();

                    // Swagger 2.+ support
                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer", Array.Empty<string>() },
                    };

                    options.AddSecurityDefinition(
                        "Bearer",
                        new ApiKeyScheme { Description = "Add authorization header using the JWT-Token (Bearer)", Name = "Authorization", In = "header", Type = "apiKey" });

                    options.AddSecurityRequirement(security);
                });
        }
    }
}
