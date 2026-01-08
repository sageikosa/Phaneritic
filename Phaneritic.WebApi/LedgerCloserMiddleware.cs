using Phaneritic.Interfaces.Ledgering;

namespace Phaneritic.WebApi;

public class LedgerCloserMiddleware(
    RequestDelegate request
    )
{
    public async Task InvokeAsync(HttpContext context, ILedgerScribbler ledgerScribbler)
    {
        try
        {
            await request(context);
        }
        finally
        {
            ledgerScribbler.CompleteActivity();
        }
    }
}
