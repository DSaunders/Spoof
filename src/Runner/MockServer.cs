using System.Diagnostics.CodeAnalysis;
using Core;
using Core.Models;
using Core.RouteParser;
using Core.Validation;
using Runner.Database;
using Runner.HotReload;
using Runner.Logging;
using HttpRequest = Core.Models.HttpRequest;
using Route = Core.RouteModels.Route;

namespace Runner;

[ExcludeFromCodeCoverage]
public class MockServer
{
    private readonly ConsoleLogger _logger = new();
    private Handler _handler = null!;

    private List<Route> LoadRoutes()
    {
        var routeLoader = new RecursiveFileLoader(_logger, new RouteParser(_logger));
        
        _logger.Verbose("Loading routes from *.route.json files");
        var routes = routeLoader.LoadRoutes(Environment.CurrentDirectory);
        
        _logger.Verbose("Validating {0} routes", routes.Count);
        routes = new InvalidRouteFilter(_logger).Filter(routes);

        return routes;
    }
    
    public void Run(bool quietMode, int port, bool debugInternals, bool hotReload)
    {
        _logger.VerboseMode = !quietMode;

        if (hotReload)
        {
            _logger.Information("Hot reload enabled. Watching for changes to *.route.json files.");
            _ = new HotReloader(
                Environment.CurrentDirectory, () =>
                {
                    _logger.Information("File change detected. Reloading routes.");
                    _handler = new Handler(_logger, LoadRoutes());
                    _logger.Information("Routes reloaded.");
                }
            );
        }

        var routes = LoadRoutes();
        _handler = new Handler(_logger, routes);

        var builder = WebApplication.CreateBuilder();
        builder.Logging.SetMinimumLevel(debugInternals ? LogLevel.Debug : LogLevel.None);
        builder.WebHost.UseUrls($"http://*:{port.ToString()}");
        var app = builder.Build();

        MapRoutes(app);

        try
        {
            var routeText = routes.Count == 1 ? "route" : "routes";
            _logger.Information(
                "Starting mock API server for {0} " + routeText + " on port {1}. CTRL+C to stop.",
                routes.Count,
                port
            );
            
            app.Run();
        }
        catch (IOException ex) when (ex.Message.Contains("address already in use"))
        {
            _logger.Error("Port {0} is already in use. Please specify a different port.", port);
        }
    }

    private void MapRoutes(WebApplication app)
    {
        app.Map("/", async ctx => { await ctx.Response.WriteAsync("Spoof is running."); });

        app.Map("/{*route}", async ctx =>
        {
            var request = new HttpRequest(
                ctx.Request.Method,
                ctx.Request.Path,
                await new StreamReader(ctx.Request.Body).ReadToEndAsync(),
                ctx.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            );

            var response = _handler.HandleRequest(request);

            ctx.Response.StatusCode = response.StatusCode;
            foreach (var (key, value) in response.Headers)
            {
                ctx.Response.Headers.Append(key, value);
            }

            await ctx.Response.WriteAsync(response.Body);
        });
    }

    
}