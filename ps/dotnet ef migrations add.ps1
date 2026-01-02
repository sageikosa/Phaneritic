# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations add.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "Phaneritic,55187"
$DeployDB = $DeployDB ? $DeployDB : "Phaneritic"
$DeployUser = $DeployUser ? $DeployUser : "Phaneritic"
$DeployPassword = $DeployPassword ? $DeployPassword : "Phaneritic"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"

$proj = "GyroLedger.Kernel"
$proj
dotnet ef migrations add Initialize -p $proj --context TableFreshnessContext -o "EF/Migrations/Kernel" -- krnl $ConnStr


$proj = "GyroLedger.Intralogistics.Implementations"
$proj
write-output "----- BaseItemMasterContext -----"
dotnet ef migrations add Initialize -p $proj --context BaseItemMasterContext -o "Migrations/ItemMasters" -- itm $ConnStr
write-output "----- OperationalContext -----"
dotnet ef migrations add Initialize -p $proj --context OperationalContext -o "Migrations/Operational" -- op $ConnStr
write-output "----- BaseInventoryContext -----"
dotnet ef migrations add Initialize -p $proj --context BaseInventoryContext -o "Migrations/Inventory" -- inv $ConnStr
write-output "----- BaseDemandContext -----"
dotnet ef migrations add Initialize -p $proj --context BaseDemandContext -o "Migrations/Demands" -- dmnd $ConnStr
write-output "----- FulfillSourceContext -----"
dotnet ef migrations add Initialize -p $proj --context FulfillSourceContext -o "Migrations/FulfillSource" -- ffsrc $ConnStr
write-output "----- FulfillDestinationContext -----"
dotnet ef migrations add Initialize -p $proj --context FulfillDestinationContext -o "Migrations/FulfillDestination" -- ffdest $ConnStr
write-output "----- FulfillFlowContext -----"
dotnet ef migrations add Initialize -p $proj --context FulfillFlowContext -o "Migrations/FulfillFlow" -- ffflow $ConnStr
write-output "----- BaseMovementContext -----"
dotnet ef migrations add Initialize -p $proj --context BaseMovementContext -o "Migrations/Movement" -- mv $ConnStr
write-output "----- LedgeringContext -----"
dotnet ef migrations add Initialize -p $proj --context LedgeringContext -o "Migrations/Ledgering" -- ldgr $ConnStr
write-output "----- RateContext -----"
dotnet ef migrations add Initialize -p $proj --context RateContext -o "Migrations/Rate" -- rate $ConnStr
