Write-Host 'Running before packaging script'

#$root = 'D:\dev\MSTC\website'
#$root = 'C:\projects\mstc-f3l95\website'
$root =  $env:APPVEYOR_BUILD_FOLDER + '\website'
Write-Host $root

$webconfig = get-content $root\Web.config | select-object -skip 1
#Write-Host $webconfig

$webconfig = $webconfig -replace "connectionString=`"([^`"]+)`"","connectionString=`"$env:databaseConnectionString`""
$webconfig = $webconfig -replace "<add key=`"umbracoDbDSN`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"umbracoDbDSN`" value=`"$env:databaseDsn"
$webconfig = $webconfig -replace "<add key=`"triclubDSN`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"triclubDSN`" value=`"$env:databaseDsn"
$webconfig = $webconfig -replace "<add key=`"gmailUserName`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailUserName`" value=`"$env:gmailUserName"
$webconfig = $webconfig -replace "<add key=`"gmailPassword`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailPassword`" value=`"$env:gmailPassword"
if ($env:gotriEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"gotriEntryEmailTo`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gotriEntryEmailTo`" value=`"$env:gotriEntryEmailTo"
}
if ($env:juniorEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"juniorEntryEmailTo`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"juniorEntryEmailTo`" value=`"$env:juniorEntryEmailTo"
}
if ($env:midSussexTriEntryEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"midSussexTriEntryEmailTo`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"midSussexTriEntryEmailTo`" value=`"$env:midSussexTriEntryEmailTo"
}
if ($env:newRegistrationEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"newRegistrationEmailTo`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"newRegistrationEmailTo`" value=`"$env:newRegistrationEmailTo"
}
if ($env:contactFormEmailTo)
{
	$webconfig = $webconfig -replace "<add key=`"contactFormEmailTo`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"contactFormEmailTo`" value=`"$env:contactFormEmailTo"
}

$webconfig = $webconfig -replace "<add key=`"gocardlessEnvironment`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gocardlessEnvironment`" value=`"$env:gocardlessEnvironment"
$webconfig = $webconfig -replace "<add key=`"gocardlessAppId`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gocardlessAppId`" value=`"$env:gocardlessAppId"
$webconfig = $webconfig -replace "<add key=`"gocardlessAppSecret`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gocardlessAppSecret`" value=`"$env:gocardlessAppSecret"
$webconfig = $webconfig -replace "<add key=`"gocardlessToken`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gocardlessToken`" value=`"$env:gocardlessToken"
$webconfig = $webconfig -replace "<add key=`"gocardlessMerchantId`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gocardlessMerchantId`" value=`"$env:gocardlessMerchantId"

if ($env:renewalsEnabled)
{
	$webconfig = $webconfig -replace "<add key=`"renewalsEnabled`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"renewalsEnabled`" value=`"$env:renewalsEnabled"
}
if ($env:openWaterEnabled)
{
	$webconfig = $webconfig -replace "<add key=`"openWaterEnabled`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"openWaterEnabled`" value=`"$env:openWaterEnabled"
}

$webconfig | out-file $root\Web.config

Write-Host 'Finished before packaging script'