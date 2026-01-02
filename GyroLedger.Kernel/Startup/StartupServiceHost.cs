using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GyroLedger.Kernel.Startup;

public static class StartupServiceHost
{
    public static IHost StartUp(this IHost host)
    {
        using (var _scope = host.Services.CreateScope())
        {
            var _services = _scope.ServiceProvider;
            var _burnDown = _services.GetServices<IKickStart>().ToList();
            var _cycle = 0;

            var _processing = true;
            while (_burnDown.Count != 0 && _processing)
            {
                // flip at start of loop
                _processing = false;

                foreach (var _i in _burnDown.ToList())
                {
                    if (_i.Startup())
                    {
                        _burnDown.Remove(_i);
                        _processing = true;
                    }
                }
                _cycle++;
            }

            if (!_processing)
            {
                throw new StartupException(@"startup stalled", _cycle, _burnDown);
            }
        }

        return host;
    }
}
