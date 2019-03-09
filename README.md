uDebug-API
[![AppVeyor](https://img.shields.io/appveyor/ci/RyuzakiH/uDebug-API/master.svg?maxAge=60)](https://ci.appveyor.com/project/RyuzakiH/uDebug-API)
[![NuGet](https://img.shields.io/nuget/v/uDebug.API.svg?maxAge=60)](https://www.nuget.org/packages/uDebug.API)
===============

Unofficial API for [uDebug](https://udebug.com) in .NET Standard

**NuGet**: https://www.nuget.org/packages/uDebug.API


# Usage

This API provides synchronous and asynchronous methods

```csharp
var client = new Client();
```
Search problems by category
```csharp
var problems = client.Search("*", SearchCategory.All);
var problems = await client.SearchAsync("*", SearchCategory.URI);
```
Get User Info
```csharp
var user = client.GetUser("dev-skill");
var user = await client.GetUserAsync("dev-skill");
```
Get Problem Info
```csharp
var problem = client.GetProblem(Judge.URI, 1001);
var problem = await client.GetProblemAsync(Judge.URI, 1001);

// By url
var problem1002 = client.GetProblem("https://udebug.com/URI/1002");
var problem1002 = await client.GetProblemAsync("https://udebug.com/URI/1002");
```
Get Input
```csharp
var inputs = new string[3];

// Gets input data
inputs[0] = problem.GetInput(problem.Inputs[0]);
inputs[0] = await problem.GetInputAsync(problem.Inputs[0]);

// Another way to get input data
inputs[1] = client.GetInput(problem.Inputs[1]);
inputs[1] = await client.GetInputAsync(problem.Inputs[1]);

// Get input data by input id
inputs[2] = client.GetInput(818703);
inputs[2] = await client.GetInputAsync(818703);
```
Get Output
```csharp
var outputs = new string[8];

// Gets the output of the problem with an input string
outputs[0] = problem.GetOutput(inputs[0]);
outputs[0] = await problem.GetOutputAsync(inputs[0]);

// Another way to get the output of the problem with a custom input string
outputs[1] = problem.GetOutput("10 6");
outputs[1] = await problem.GetOutputAsync("10 6");

// Gets the output of the problem with an input
outputs[2] = client.GetOutput(problem.Inputs[3]);
outputs[2] = await client.GetOutputAsync(problem.Inputs[3]);

// Another way to get the output of the problem with an input
outputs[3] = problem.GetOutput(problem.Inputs[4]);
outputs[3] = await problem.GetOutputAsync(problem.Inputs[4]);

// Another way to get the output of the problem with an input
outputs[4] = client.GetOutput(problem, problem.Inputs[5]);
outputs[4] = await client.GetOutputAsync(problem, problem.Inputs[5]);

// Gets the output of a problem identified by url with a custom input string
outputs[5] = client.GetOutput("https://udebug.com/URI/1001", "200 5");
outputs[5] = await client.GetOutputAsync("https://udebug.com/URI/1001", "200 5");

// Gets the output of a problem identified by url with an input
outputs[6] = client.GetOutput("https://udebug.com/URI/1001", problem.Inputs[6]);
outputs[6] = await client.GetOutputAsync("https://udebug.com/URI/1001", problem.Inputs[6]);

// Gets the output of a problem with a custom input string
outputs[7] = client.GetOutput(Judge.URI, 1001, "400 5");
outputs[7] = await client.GetOutputAsync(Judge.URI, 1001, "400 5");
```


Full Test Example [Here](https://github.com/RyuzakiH/uDebug-API/blob/master/src/uDebug.Example/Program.cs)

# Supported Platforms
[.NET Standard 1.3](https://github.com/dotnet/standard/blob/master/docs/versions.md)

# Dependencies
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack)
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
