using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phaneritic.Interfaces.Startup;

namespace Phaneritic.Implementations.Startup;

public static class StartupServiceHost
{
    public static IHost StartUp(this IHost host)
    {
        // create service scope for running startups
        using (var _scope = host.Services.CreateScope())
        {
            // get all kick startable services and cycle through them
            var _services = _scope.ServiceProvider;
            var _burnDown = _services.GetServices<IKickStart>().ToList();
            var _cycle = 0;

            var _processing = true;
            while (_burnDown.Count != 0 && _processing)
            {
                // flip at start of loop
                _processing = false;

                // snapshot list
                foreach (var _i in _burnDown.ToList())
                {
                    // process
                    if (_i.Startup())
                    {
                        // remove from list if successful
                        _burnDown.Remove(_i);
                        _processing = true;
                    }
                }
                _cycle++;
            }

            // if nothing got processed in the last cycle, we have a problem
            if (!_processing)
            {
                throw new StartupException(@"startup stalled", _cycle, _burnDown);
            }
        }

        return host;
    }
}
