using System.Transactions;

namespace Phaneritic.Interfaces.Database;
public class DatabaseOptions
{
    public string StandardConnectString { get; set; } = @"Server=.;Database=Phaneritic;User ID=Phaneritic;Password=Phaneritic;TrustServerCertificate=true;";
    public string LoggingConnectString { get; set; } = @"Server=.;Database=Phaneritic;User ID=Phaneritic;Password=Phaneritic;TrustServerCertificate=true;";

    /// <summary>Milliseconds between retries, Default = 10</summary>
    public int DbRetryDelayMilliseconds { get; set; } = 10;

    /// <summary>Max retries on failure, Default = 3</summary>
    public int DbRetryMax { get; set; } = 3;

    /// <summary>Default = IsolationLevel.ReadCommitted</summary>
    public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
}
