Write-Host 'Hey, this is my deploy.ps1'

#$root = 'D:\dev\MSTC\website'
$root =  $Env:APPLICATION_PATH

$webconfig = get-content $root\Web.config | select-object -skip 1
#Write-Host $webconfig

$webconfig = $webconfig -replace "<add key=`"gmailUserName`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailUserName`" value=`"$Env:gmailUserName"
$webconfig = $webconfig -replace "<add key=`"gmailPassword`" value=`"[a-zA-Z0-9-_.:/\\@]+","<add key=`"gmailUserName`" value=`"Jim"
$webconfig | out-file $root\Web.config