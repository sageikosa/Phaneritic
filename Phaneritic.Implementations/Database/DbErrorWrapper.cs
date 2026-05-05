using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public static class DbErrorWrapper
{
    public static async Task DoErrorWrap(this IList<IDbErrorWrap> wrappers, Func<Task> action, CancellationToken cancellationToken)
    {
        // forward declare
        Func<int, Task> _invoker = async (depth) => { await Task.CompletedTask; };

        // self-referential call needed forward declare
        _invoker = async (depth) =>
        {
            if (depth < 0)
            {
                // if index goes below available, execute the underlying action
                action?.Invoke();
            }
            else
            {
                var _wrap = wrappers[depth];
                if (_wrap != null)
                {
                    await _wrap.ErrorWrap(async () => await _invoker(depth - 1), cancellationToken);
                }
            }
        };

        // kick off recursioning
        await _invoker((wrappers?.Count ?? 0) - 1);
    }
}
