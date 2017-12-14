Write-Host 'Running before packaging script'

#$root = 'D:\dev\MSTC\website'
#$root = 'C:\projects\mstc-f3l95\website'
$root =  $env:APPVEYOR_BUILD_FOLDER + '\website'
Write-Host $root

$configPath = $root + '\Web.config'
$webconfig = get-content $configPath
#Write-Host $webconfig

$webconfig = $webconfig -replace "connectionString=`"([^`"]+)`"","connectionString=`"$env:databaseConnectionString`""
$webconfig = $webconfig -replace "<add key=`"umbracoDbDSN`" value=`"([^`"]+)`"","<add key=`"umbracoDbDSN`" value=`"$env:databaseDsn`""
$webconfig = $webconfig -replace "<add key=`"triclubDSN`" value=`"([^`"]+)`"","<add key=`"triclubDSN`" value=`"$env:databaseDsn`""
$webconfig = $webconfig -replace "<add key=`"gmailUserName`" value=`"([^`"]+)`"","<add key=`"gmailUserName`" value=`"$env:gmailUserName`""
$webconfig = $webconfig -replace "<add key=`"gmailPassword`" value=`"([^`"]+)`"","<add key=`"gmailPassword`" value=`"$env:gmailPassword`""
if ($env:gotriEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"gotriEntryEmailTo`" value=`"([^`"]+)`"","<add key=`"gotriEntryEmailTo`" value=`"$env:gotriEntryEmailTo`""
}
if ($env:juniorEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"juniorEntryEmailTo`" value=`"([^`"]+)`"","<add key=`"juniorEntryEmailTo`" value=`"$env:juniorEntryEmailTo`""
}
if ($env:midSussexTriEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"midSussexTriEntryEmailTo`" value=`"([^`"]+)`"","<add key=`"midSussexTriEntryEmailTo`" value=`"$env:midSussexTriEntryEmailTo`""
}
if ($env:newRegistrationEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"newRegistrationEmailTo`" value=`"([^`"]+)`"","<add key=`"newRegistrationEmailTo`" value=`"$env:newRegistrationEmailTo`""
}
if ($env:contactFormEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"contactFormEmailTo`" value=`"([^`"]+)`"","<add key=`"contactFormEmailTo`" value=`"$env:contactFormEmailTo`""
}

$webconfig = $webconfig -replace "<add key=`"gocardlessEnvironment`" value=`"([^`"]+)`"","<add key=`"gocardlessEnvironment`" value=`"$env:gocardlessEnvironment`""
$webconfig = $webconfig -replace "<add key=`"gocardlessAppId`" value=`"([^`"]+)`"","<add key=`"gocardlessAppId`" value=`"$env:gocardlessAppId`""
$webconfig = $webconfig -replace "<add key=`"gocardlessAppSecret`" value=`"([^`"]+)`"","<add key=`"gocardlessAppSecret`" value=`"$env:gocardlessAppSecret`""
$webconfig = $webconfig -replace "<add key=`"gocardlessToken`" value=`"([^`"]+)`"","<add key=`"gocardlessToken`" value=`"$env:gocardlessToken`""
$webconfig = $webconfig -replace "<add key=`"gocardlessMerchantId`" value=`"([^`"]+)`"","<add key=`"gocardlessMerchantId`" value=`"$env:gocardlessMerchantId`""
$webconfig = $webconfig -replace "<add key=`"gocardlessAccessToken`" value=`"([^`"]+)`"","<add key=`"gocardlessAccessToken`" value=`"$env:gocardlessAccessToken`""


if ($env:renewalsEnabled)
{
	$webconfig = $webconfig -replace "<add key=`"renewalsEnabled`" value=`"([^`"]+)`"","<add key=`"renewalsEnabled`" value=`"$env:renewalsEnabled`""
}
if ($env:openWaterEnabled)
{
	$webconfig = $webconfig -replace "<add key=`"openWaterEnabled`" value=`"([^`"]+)`"","<add key=`"openWaterEnabled`" value=`"$env:openWaterEnabled`""
}

[System.IO.File]::WriteAllLines($configPath, $webconfig)

Write-Host 'Finished before packaging script'