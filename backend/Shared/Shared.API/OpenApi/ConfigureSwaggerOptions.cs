using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.API.OpenApi;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private string _serviceName;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
        InitServiceName();
    }

    private void InitServiceName()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var microserviceAssembly = assemblies
            .FirstOrDefault(a => a.GetName().Name?.EndsWith("Service.API") == true);
        
        var assemblyName = microserviceAssembly?.GetName().Name;

        _serviceName = assemblyName?.Replace(".API", "").Replace("Service", "").Trim() ?? "Unknown";
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"{_serviceName} API",
                Version = description.ApiVersion.ToString(),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-service-path", new OpenApiString($"{_serviceName.ToLower()}-service") }
                }
            });
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}