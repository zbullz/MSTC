Write-Host 'Hey, this is my before packaging script'

#$root = 'D:\dev\MSTC\website'
#$root = 'C:\projects\mstc-f3l95\website'
$root =  $env:APPVEYOR_BUILD_FOLDER + '\website'
Write-Host $root

$webconfig = get-content $root\Web.config | select-object -skip 1
#Write-Host $webconfig

$webconfig = $webconfig -replace "<add key=`"gmailUserName`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailUserName`" value=`"$Env:gmailUserName"
$webconfig = $webconfig -replace "<add key=`"gmailPassword`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailPassword`" value=`"Jim"
$webconfig | out-file $root\Web.config