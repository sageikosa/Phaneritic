$DeployInstance = $DeployInstance ? $DeployInstance : "xpstower,55187"
$DeployDB = $DeployDB ? $DeployDB : "GyroLedger"
$DeployUser = $DeployUser ? $DeployUser : "GyroLedger"
$DeployPassword = $DeployPassword ? $DeployPassword : "GyroLedger3360"
$DeploySeed = $DeploySeed ? $DeploySeed : ""
# NOTE set $DeploySeed = "custom\" before running to use custom seeddata

function DoSqlFile{
    param ($inputFileName)
    write-output $inputFileName
    invoke-sqlcmd -inputFile $inputFileName -ServerInstance $DeployInstance -Database $DeployDB -UserName $DeployUser -Password $DeployPassword -trustServerCertificate 
}

get-item ".\SQL\*.sql" | foreach-object { DoSqlFile($_.FullName) }
get-item ".\SQL\TableTypes\*.sql" | foreach-object { DoSqlFile($_.FullName) }
get-item ".\SQL\TableTypes\Rates\*.sql" | foreach-object { DoSqlFile($_.FullName) }
get-item ".\SQL\Sprocs\*.sql" | foreach-object { DoSqlFile($_.FullName) }
get-item ".\SQL\Sprocs\Rates\*.sql" | foreach-object { DoSqlFile($_.FullName) }
DoSqlFile(".\SQL\inserts\Inventory.sql")
DoSqlFile(".\SQL\inserts\Operational.sql")
DoSqlFile(".\SQL\inserts\RegionsZonesLocations.sql")