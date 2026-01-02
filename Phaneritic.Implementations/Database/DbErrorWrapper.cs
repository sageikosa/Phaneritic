using GyroLedger.CodeInterface.Database;

namespace GyroLedger.Kernel.Database;

public static class DbErrorWrapper
{
    public static void DoErrorWrap(this IList<IDbErrorWrap> wrappers, Action action)
    {
        // forward declare
        Action<int> _invoker = (depth) => { };

        // self-referential call needed forward declare
        _invoker = (depth) =>
        {
            if (depth < 0)
            {
                // if index goes below available, execute the underlying action
                action?.Invoke();
            }
            else
            {
                wrappers[depth]?.ErrorWrap(() => _invoker(depth - 1));
            }
        };

        // kick off recursioning
        _invoker((wrappers?.Count ?? 0) - 1);
    }
}
