# OwinRequestScopeContext

With this Owin middleware, you can use OwinRequestScopeContext.Current like HttpContext.Current, but without a dependency to System.Web.

It uses CallContext internally. Which means that it is accessible when a request is handled on different threads, and using async/await.
More info in this article: http://odetocode.com/Articles/112.aspx

## Example usage
```cs
  public class Startup {
    public void Configuration(IAppBuilder app) {
      // Add it to the owin pipeline
      app.UseRequestScopeContext();
    }
  }
  
  public class RequestParametersFromOwinRequestConfigurator {
    public void Configure(IOwinRequest owinRequest) {
      // Set a value in the Items dictionary, available througout the request
      var requestContext = OwinRequestScopeContext.Current;
      requestContext.Items["MyCustomHeaderValue"] = owinRequest.Headers["MyCustomHeader"];
    }
  }
  
  public class RequestParametersProviderFromOwinRequestScope {
    public string ProvideMyCustomHeaderValue() {
      var requestContext = OwinRequestScopeContext.Current;
      if (requestContext == null) return null;
      if (!requestContext.Items.TryGetValue("MyCustomHeaderValue", out object myCustomHeaderValueObj)) return null;
      return myCustomHeaderValueObj as string;
    }
  }
```

## Disposable

You can also register IDisposable instances for disposal when the request is completed. 
```cs

  public class LocalStorageManager : IDisposable {
    ...
    public void Dispose() { ... }
  }

  public class RequestParametersFromOwinRequestConfigurator {
    public void Configure(IOwinRequest owinRequest) {
      var requestContext = OwinRequestScopeContext.Current;
      var localStorageManager = new LocalStorageManager();
      requestContext.Items["MyDisposableObject"] = localStorageManager;

      // Dispose the localStorageManager instance when the request is completed
      requestContext.RegisterForDisposal(localStorageManager);
    }
  }
```

Remark: When any .Dispose() call fails, the other registered instances are still disposed. Only afterwards, an AggregateException is thrown.

## NuGet

Binary package: [![NuGet Status](http://img.shields.io/nuget/v/DavidLievrouw.OwinRequestScopeContext.svg?style=flat-square)](https://www.nuget.org/packages/DavidLievrouw.OwinRequestScopeContext/)

Source-only package: [![NuGet Status](http://img.shields.io/nuget/v/DavidLievrouw.OwinRequestScopeContext.Sources.svg?style=flat-square)](https://www.nuget.org/packages/DavidLievrouw.OwinRequestScopeContext.Sources/)

## Change log

v1.0.0 - 2017-08-12
- Initial release (binary and sources).