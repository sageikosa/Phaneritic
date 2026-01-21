# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations add.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "Phaneritic,55187"
$DeployDB = $DeployDB ? $DeployDB : "Phaneritic"
$DeployUser = $DeployUser ? $DeployUser : "Phaneritic"
$DeployPassword = $DeployPassword ? $DeployPassword : "Phaneritic"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"
$CaseInsensitive = "SQL_Latin1_General_CP1_CI_AS"
$CaseSensitive = "SQL_Latin1_General_CP1_CS_AS"

$proj = "Phaneritic.Implementations"
$proj
dotnet ef migrations add Initialize -p $proj --context TableFreshnessContext -o "EF/Migrations/Kernel" -- krnl $ConnStr $CaseInsensitive $CaseSensitive
write-output "----- OperationalContext -----"
dotnet ef migrations add Initialize -p $proj --context OperationalContext -o "Migrations/Operational" -- op $ConnStr $CaseInsensitive $CaseSensitive
write-output "----- LedgeringContext -----"
dotnet ef migrations add Initialize -p $proj --context LedgeringContext -o "Migrations/Ledgering" -- ldgr $ConnStr $CaseInsensitive $CaseSensitive
