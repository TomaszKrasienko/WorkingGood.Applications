using WorkingGood.WebApi.Middlewares;

namespace WorkingGood.WebApi.Common.Extensions.Configuration;

public static class MiddlewareConfiguration
{
    public static WebApplication AddCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}