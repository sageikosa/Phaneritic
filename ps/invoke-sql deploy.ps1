$DeployInstance = $DeployInstance ? $DeployInstance : "Phaneritic,55187"
$DeployDB = $DeployDB ? $DeployDB : "Phaneritic"
$DeployUser = $DeployUser ? $DeployUser : "Phaneritic"
$DeployPassword = $DeployPassword ? $DeployPassword : "Phaneritic"
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