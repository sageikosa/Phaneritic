# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations script.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "Phaneritic,55187"
$DeployDB = $DeployDB ? $DeployDB : "Phaneritic"
$DeployUser = $DeployUser ? $DeployUser : "Phaneritic"
$DeployPassword = $DeployPassword ? $DeployPassword : "Phaneritic"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"
$CaseInsensitive = "SQL_Latin1_General_CP1_CI_AS"
$CaseSensitive = "SQL_Latin1_General_CP1_CS_AS"

$proj = "Phaneritic.Implementations"
$proj
dotnet ef migrations script 0 -p $proj -o "SQL\krnlinit.sql" --context TableFreshnessContext -- krnl $ConnStr $CaseInsensitive $CaseSensitive
write-output "----- OperationalContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\opinit.sql" --context OperationalContext -- op $ConnStr $CaseInsensitive $CaseSensitive
write-output "----- LedgeringContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\ldgrinit.sql" --context LedgeringContext -- ldgr $ConnStr $CaseInsensitive $CaseSensitive
