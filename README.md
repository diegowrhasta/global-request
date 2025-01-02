# Introduction

This is a small prototype ASP.NET Core project used in order to test out both framework 
specific injected properties (Request) and a static access property that is instantiated 
through a static class and configured on application startup.

## Request Property

May it be through Minimal APIs or through MVC Controllers, they can both get, at 
runtime, information in regard to the Request made to a specific endpoint through 
the access of the `Request` property for MVC, and through the declaration of a
`HttpRequest` parameter for Minimal APIs. This is how the framework works out of 
the box the developer does not need to do much on their side.

## HttpContextAccessor

However, there are some instances in which due to bad design choices, or just legacy 
code that's being ported, we can't rely on these properties, we might have coupled up 
a controller to another controller resulting in a clash or possible misinterpretation 
of what the `Request` instance actually holds in place. It's for these instances that 
we have access to a special static class that will always have fed onto it the information 
of the _request_ that kickstarted the whole flow.

## Solution

If we are working in a .NET Core project, we can configure DI so that we can access 
this specific static class if we want to: `builder.Services.AddHttpContextAccessor();`. 
And then, by coding a bit, we can end up with a simple call that can configure a static 
helper class on our side that will grab from that framework static place and expose 
that to any consumer:

````csharp
var (httpContextAccessor, webHostEnvironment) = (
    app.Services.GetRequiredService<IHttpContextAccessor>(),
    app.Services.GetRequiredService<IWebHostEnvironment>());

HttpContextHelper.Configure(httpContextAccessor, webHostEnvironment);
````
It is through the injection of both an `IHttpContextAccessor` and an `IWebHostEnvironment` 
instance that we can then leverage in order to reference in our static helper this 
_Request_ instance in which we can access many (if not all) the methods that we would 
have available through the injected properties mentioned in the previous sections.

## Information about project

There are 5 endpoints, 4 are Minimal APIs and 1 is a normal MVC controller. Each 
one tests out how we can access both the injected properties and the reference in 
the static helper. As a conclusion we see how through the usage of the `HttpContextAccesor` 
we get rid of possible confusions or errors when working with multiple controllers 
that could have null or other references in said conventional _Request_ properties.

## Accessing Server's Paths

