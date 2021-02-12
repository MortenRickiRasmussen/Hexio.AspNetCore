# Hexio.AspNetCore Package

A package for adding simple but powerful logging to AspNetCore applications with Serilog to Elasticsearch

This package also includes simple healthchecks

## How To Use

Make your Program.cs Main method look like this

``` cs
public static void Main(string[] args)
{
    Host.CreateDefaultBuilder(args).ConfigureWebHostHexioDefaults<Startup>()
        .Build()
        .Run();
}
```

Then you need to setup your environment variables, e.g inside appsettings

```
{
  "Elasticsearch": {
    "Url": "http://localhost:9200",
    "Username": "CHANGEME", 
    "Password": "CHANGEME"
  }
}
```

### Root endpoint 

If you want something on your applications root endpoint add the following inside your Configure method in Startup.cs
Then your application will return the Full namespace of the package e.g. `Hexio.AspNetCore.Demo`

``` cs
app.AddRootEndpoint();
```

### Healthchecks

If you want to enable healthchecks add the following to your Configure method inside Startup.cs

``` cs
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
});
```

And also include the following inside your ConfigureServices method of your Startup class

``` cs
services.AddHexioHealthChecks();
```

