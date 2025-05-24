#r "nuget: NSwag.Core, 14.2.0"
#r "nuget: Microsoft.OpenApi, 1.6.23"

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using NSwag;

string solutionRoot = Directory.GetCurrentDirectory();
string outputFilePath = Path.Combine(solutionRoot, "task-manager-swagger.json");

// First, build the solution
Console.WriteLine("🔨 Building solution...");
var buildProcess = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "build",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        WorkingDirectory = solutionRoot
    }
};

buildProcess.Start();
string output = buildProcess.StandardOutput.ReadToEnd();
string error = buildProcess.StandardError.ReadToEnd();
buildProcess.WaitForExit();

if (buildProcess.ExitCode != 0)
{
    Console.WriteLine("❌ Build failed!");
    Console.WriteLine(error);
    return;
}

Console.WriteLine("✅ Build completed successfully!");

Console.WriteLine($"📂 Scanning entire solution for Swagger JSON files...");

// Recursively find all `swagger.json` files in the solution
var swaggerFiles = Directory.GetFiles(solutionRoot, "swagger.json", SearchOption.AllDirectories)
                            .Where(f => !f.Contains("bin") && !f.Contains("obj")) 
                            .ToList();

if (swaggerFiles.Count == 0)
{
    Console.WriteLine("❌ No Swagger files found in the solution!");
    return;
}

var openApiDocs = new List<NSwag.OpenApiDocument>();

foreach (var file in swaggerFiles)
{
    try
    {
        Console.WriteLine($"🔍 Processing {file}");
        var json = await File.ReadAllTextAsync(file);
        var document = await NSwag.OpenApiDocument.FromJsonAsync(json);
        openApiDocs.Add(document);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Error processing {file}: {ex.Message}");
    }
}

if (openApiDocs.Count == 0)
{
    Console.WriteLine("❌ No valid Swagger documents found!");
    return;
}

// Merge all Swagger documents into a single file
var mergedDoc = new NSwag.OpenApiDocument() {
    Info = new NSwag.OpenApiInfo
    {
        Title = "Task Manager API",
    },
};

foreach (var doc in openApiDocs)
{
    var serviceName = doc.Info.ExtensionData?.ContainsKey("x-service-path") == true
            ? doc.Info.ExtensionData["x-service-path"].ToString()
            : throw new InvalidOperationException($"No 'x-service-path' found in '{doc.Info.Title}'.");
                            
    // Merge Paths
    foreach (var path in doc.Paths)
    {
        if (!mergedDoc.Paths.ContainsKey(path.Key))
        {
            string newPath = $"/{serviceName}{path.Key}";
            mergedDoc.Paths.Add(newPath, path.Value);
        }
        else
        {
            throw new InvalidOperationException($"Duplicate path '{path.Key}' found in '{doc.Info.Title}'.");
        }
    }

    // Merge Components (Schemas)
    foreach (var schema in doc.Components.Schemas)
    {
        if (!mergedDoc.Components.Schemas.ContainsKey(schema.Key))
        {
            mergedDoc.Components.Schemas.Add(schema.Key, schema.Value);
        }
        else
        {
            throw new InvalidOperationException($"Duplicate schema '{schema.Key}' found in '{doc.Info.Title}'.");
        }
    }

    // Merge other components as necessary (e.g., Responses, Parameters, etc.)
}

await File.WriteAllTextAsync(outputFilePath, mergedDoc.ToJson());
Console.WriteLine($"✅ Swagger merged successfully into {outputFilePath}");