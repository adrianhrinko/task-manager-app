using Asp.Versioning;
using Shared.API.ExceptionHandling;
using Shared.API.OpenApi;
using UserService.API.Endpoints;
using UserService.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables(); 

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options => 
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
} else {
    app.UseMiddleware<CustomExceptionMiddleware>();
}

app.UseHttpsRedirection();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

var versionedGroup = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionedGroup.MapAuthEndpoints();
versionedGroup.MapUserEndpoints();
versionedGroup.MapTeamEndpoints();

app.Run();