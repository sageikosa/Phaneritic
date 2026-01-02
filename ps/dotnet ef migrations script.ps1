# Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
# &"ps\dotnet ef migrations script.ps1"

$DeployInstance = $DeployInstance ? $DeployInstance : "xpstower,55187"
$DeployDB = $DeployDB ? $DeployDB : "GyroLedger"
$DeployUser = $DeployUser ? $DeployUser : "GyroLedger"
$DeployPassword = $DeployPassword ? $DeployPassword : "GyroLedger3360"
$ConnStr = "Server=$DeployInstance;Database=$DeployDB;User ID=$DeployUser;Password=$DeployPassword;TrustServerCertificate=true;"

$proj = "GyroLedger.Kernel"
$proj
dotnet ef migrations script 0 -p $proj -o "SQL\krnlinit.sql" --context TableFreshnessContext -- krnl $ConnStr

$proj = "GyroLedger.Intralogistics.Implementations"
$proj
write-output "----- BaseItemMasterContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\itminit.sql" --context BaseItemMasterContext -- itm $ConnStr
write-output "----- OperationalContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\opinit.sql" --context OperationalContext -- op $ConnStr
write-output "----- BaseInventoryContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\invinit.sql" --context BaseInventoryContext -- inv $ConnStr
write-output "----- BaseDemandContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\dmndinit.sql" --context BaseDemandContext -- dmnd $ConnStr
write-output "----- FulfillSourceContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\ffsrcinit.sql" --context FulfillSourceContext -- ffsrc $ConnStr
write-output "----- FulfillDestinationContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\ffdestinit.sql" --context FulfillDestinationContext -- ffdest $ConnStr
write-output "----- FulfillFlowContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\ffflowinit.sql" --context FulfillFlowContext -- ffflow $ConnStr
write-output "----- BaseMovementContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\mvinit.sql" --context BaseMovementContext -- mv $ConnStr
write-output "----- LedgeringContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\ldgrinit.sql" --context LedgeringContext -- ldgr $ConnStr
write-output "----- RateContext -----"
dotnet ef migrations script 0 -p $proj -o "SQL\rateinit.sql" --context RateContext -- rate $ConnStr
