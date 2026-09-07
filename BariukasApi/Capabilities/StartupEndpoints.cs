using BariukasApi.Shared;

namespace BariukasApi.Capabilities;

public static class StartupEndpoints
{
    extension(WebApplication app)
    {
        public IApplicationBuilder MapEndpoints()
        {
            var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (var endpoint in endpoints) endpoint.Map(app);

            return app;
        }
    }

    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder RegisterEndpoints()
        {
            var endpointTypes = typeof(StartupEndpoints).Assembly
                .GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false }
                            && typeof(IEndpoint).IsAssignableFrom(t));

            foreach (var type in endpointTypes)
                builder.Services.AddTransient(typeof(IEndpoint), type);

            return builder;
        }
    }
}