# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations remove.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "xpstower,55187"
$DeployDB = $DeployDB ? $DeployDB : "GyroLedger"
$DeployUser = $DeployUser ? $DeployUser : "GyroLedger"
$DeployPassword = $DeployPassword ? $DeployPassword : "GyroLedger3360"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"

$proj = "GyroLedger.Kernel"
$proj
dotnet ef migrations remove -p $proj --context TableFreshnessContext -- krnl $ConnStr

$proj = "GyroLedger.Intralogistics.Implementations"
$proj
write-output "----- BaseItemMasterContext -----"
dotnet ef migrations remove -p $proj --context BaseItemMasterContext -- itm $ConnStr
write-output "----- OperationalContext -----"
dotnet ef migrations remove -p $proj --context OperationalContext -- op $ConnStr
write-output "----- BaseInventoryContext -----"
dotnet ef migrations remove -p $proj --context BaseInventoryContext -- inv $ConnStr
write-output "----- BaseDemandContext -----"
dotnet ef migrations remove -p $proj --context BaseDemandContext -- dmnd $ConnStr
write-output "----- FulfillSourceContext -----"
dotnet ef migrations remove -p $proj --context FulfillSourceContext -- ffsrc $ConnStr
write-output "----- FulfillDestinationContext -----"
dotnet ef migrations remove -p $proj --context FulfillDestinationContext -- ffdest $ConnStr
write-output "----- FulfillFlowContext -----"
dotnet ef migrations remove -p $proj --context FulfillFlowContext -- ffflow $ConnStr
write-output "----- BaseMovementContext -----"
dotnet ef migrations remove -p $proj --context BaseMovementContext -- mv $ConnStr
write-output "----- LedgeringContext -----"
dotnet ef migrations remove -p $proj --context LedgeringContext -- ldgr $ConnStr
write-output "----- RateContext -----"
dotnet ef migrations remove -p $proj --context RateContext -- rate $ConnStr
