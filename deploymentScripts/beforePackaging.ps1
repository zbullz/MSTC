#Use this command to run the script
#&("D:\dev\MSTC\deploymentScripts\beforePackaging.ps1")

Write-Host 'Running before packaging script'

#$root = 'D:\Temp'
#$root = 'C:\projects\mstc-f3l95\website'
$root =  $env:APPVEYOR_BUILD_FOLDER + '\website'
Write-Host $root

$configPath = $root + '\Web.config'
$webconfig = get-content $configPath
#Write-Host $webconfig

function ReplaceAppSetting($sourceFile, $keyName, $replacementValue) {
    $sourceFile -replace "<add key=`"$keyName`" value=`".*`"","<add key=`"$keyName`" value=`"$replacementValue`""
}

$webconfig = $webconfig -replace "connectionString=`"([^`"]+)`"","connectionString=`"$env:databaseConnectionString`""
$webconfig = ReplaceAppSetting $webconfig "umbracoDbDSN" $env:databaseDsn
$webconfig = ReplaceAppSetting $webconfig "triclubDSN" $env:databaseDsn
$webconfig = ReplaceAppSetting $webconfig "gmailUserName" $env:gmailUserName
$webconfig = ReplaceAppSetting $webconfig "gmailPassword" $env:gmailPassword
if ($env:gotriEntryEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "gotriEntryEmailTo" $env:gotriEntryEmailTo
}
if ($env:juniorEntryEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "juniorEntryEmailTo" $env:juniorEntryEmailTo
}
if ($env:midSussexTriEntryEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "midSussexTriEntryEmailTo" $env:midSussexTriEntryEmailTo
}
if ($env:newRegistrationEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "newRegistrationEmailTo" $env:newRegistrationEmailTo
}
if ($env:contactFormEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "contactFormEmailTo" $env:contactFormEmailTo
}
if ($env:owsEmailTo)
{
	$webconfig = ReplaceAppSetting $webconfig "owsEmailTo" $env:owsEmailTo
}


$webconfig = ReplaceAppSetting $webconfig "gocardlessEnvironment" $env:gocardlessEnvironment
$webconfig = ReplaceAppSetting $webconfig "gocardlessAccessToken" $env:gocardlessAccessToken

if ($env:renewalsEnabled)
{
    $webconfig = ReplaceAppSetting $webconfig "renewalsEnabled" $env:renewalsEnabled
}
if ($env:openWaterEnabled)
{
    $webconfig = ReplaceAppSetting $webconfig "openWaterEnabled" $env:openWaterEnabled
}
if ($env:umbracoUseSSL)
{
    $webconfig = ReplaceAppSetting $webconfig "umbracoUseSSL" $env:umbracoUseSSL
}

[System.IO.File]::WriteAllLines($configPath, $webconfig)

Write-Host 'Finished before packaging script'