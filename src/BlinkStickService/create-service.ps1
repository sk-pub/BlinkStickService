New-Service -Name BlinkStickService -BinaryPathName (Join-Path (Get-Location).Path BlinkStickService.exe) -StartupType Automatic
Start-Service -Name BlinkStickService