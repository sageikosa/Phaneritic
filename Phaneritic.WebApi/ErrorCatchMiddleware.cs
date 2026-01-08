using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.WebApi;

public class ErrorCatchMiddleware(
    RequestDelegate next,
    IActionResultExecutor<JsonResult> executor
    )
{
    public async Task Invoke(
        HttpContext context,
        IAccessSessionReader accessSessionReader,
        ILogger<ErrorCatchMiddleware> logger,
        ILedgerScribbler ledgerScribbler,
        ProblemDetailsFactory factory
        )
    {
        try
        {
            await next(context);
        }
        catch (Exception _except)
        {
            var _activity = ledgerScribbler.ActiveActivity;
            if (logger.IsEnabled(LogLevel.Error))
            {
                var _session = accessSessionReader.GetScopedAccessSession();
                var _logEntry = new
                {
                    SessionID = _session?.AccessSessionID ?? default,
                    MechanismID = _session?.AccessMechanism?.AccessMechanismID ?? default,
                    MechanismKey = _session?.AccessMechanism?.AccessMechanismKey ?? default,
                    AccessorID = _session?.Accessor?.AccessorID ?? default,
                    AccessorKey = _session?.Accessor?.AccessorKey ?? default,
                    ActivityID = _activity?.ActivityID ?? default,
                    ActivityTypeKey = _activity?.ActivityTypeKey ?? default,
                    OperationID = _activity?.OperationID ?? default,
                    MethodKey = _activity?.MethodKey ?? default
                };
                logger.Log(LogLevel.Error, _except, "Session: {@LogData}", _logEntry);
            }

            // log to scribbler if tracking an activity
            if (_activity != null)
            {
                ledgerScribbler.AddException(_except);
                ledgerScribbler.CompleteActivity(isSuccessful: false);
            }

            if (context.Response.HasStarted)
            {
                throw;
            }

            await HandleException(context, _except, factory);
        }
    }

    private Task HandleException(HttpContext context, Exception except, ProblemDetailsFactory factory)
    {
        var _route = context.GetRouteData() ?? new RouteData();
        ClearCacheHeaders(context.Response);

        var _actionContext = new ActionContext(context, _route, new ActionDescriptor());
        var _result = new JsonResult(
            factory.CreateProblemDetails(context, StatusCodes.Status500InternalServerError, @"GL Error", except.GetType().Name,
            $@"{except.Message}{(except.InnerException != null ? $@"[{except.InnerException?.Message}]" : string.Empty)}"))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        return executor.ExecuteAsync(_actionContext, _result);
    }

    private static void ClearCacheHeaders(HttpResponse response)
    {
        response.Headers[HeaderNames.CacheControl] = @"no-cache";
        response.Headers[HeaderNames.Pragma] = @"no-cache";
        response.Headers[HeaderNames.Expires] = @"-1";
        response.Headers.Remove(HeaderNames.ETag);
    }
}
