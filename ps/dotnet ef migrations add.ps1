# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations add.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "Phaneritic,55187"
$DeployDB = $DeployDB ? $DeployDB : "Phaneritic"
$DeployUser = $DeployUser ? $DeployUser : "Phaneritic"
$DeployPassword = $DeployPassword ? $DeployPassword : "Phaneritic"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"

$proj = "Phaneritic.Implementations"
$proj
dotnet ef migrations add Initialize -p $proj --context TableFreshnessContext -o "EF/Migrations/Kernel" -- krnl $ConnStr


# $proj = "Phaneritic.Operational.Implementations"
# $proj
# write-output "----- OperationalContext -----"
# dotnet ef migrations add Initialize -p $proj --context OperationalContext -o "Migrations/Operational" -- op $ConnStr
