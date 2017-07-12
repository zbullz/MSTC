Write-Host 'Hey, this is my before packaging script'

#$root = 'D:\dev\MSTC\website'
$root =  $env:APPVEYOR_BUILD_FOLDER
#$root = 'C:\projects\mstc-f3l95'
Write-Host $root

$webconfig = get-content $root\website\Web.config | select-object -skip 1
#Write-Host $webconfig

$webconfig = $webconfig -replace "<add key=`"gmailUserName`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailUserName`" value=`"$Env:gmailUserName"
$webconfig = $webconfig -replace "<add key=`"gmailPassword`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailPassword`" value=`"Jim"
$webconfig | out-file $root\Web.config