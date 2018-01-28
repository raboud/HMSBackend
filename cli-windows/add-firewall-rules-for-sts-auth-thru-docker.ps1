param([switch]$Elevated)
function Check-Admin {
$currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
$currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}
if ((Check-Admin) -eq $false)  {
if ($elevated)
{
# could not elevate, quit
}
 
else {
 
Start-Process powershell.exe -Verb RunAs -ArgumentList ('-noprofile -noexit -file "{0}" -elevated' -f ($myinvocation.MyCommand.Definition))
}
exit
}

try {
  Get-NetFirewallRule -DisplayName EshopDocker -ErrorAction Stop
  Write-Host "Rule found"
}
  catch [Exception] {
  New-NetFirewallRule -DisplayName HMS-Backend-Inbound -Confirm -Description "HMS Backend-Inbound Rule for port range 5100-5150" -LocalAddress Any -LocalPort 5100-5150 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Inbound
  New-NetFirewallRule -DisplayName HMS-Backend-Outbound -Confirm -Description "HMS Backend-Outbound Rule for port range 5100-5150" -LocalAddress Any -LocalPort 5100-5150 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Outbound
  New-NetFirewallRule -DisplayName HMS-SQL-Inbound -Confirm -Description "HMS SQL Inbound Rule for port-5433" -LocalAddress Any -LocalPort 5433 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Inbound
  New-NetFirewallRule -DisplayName HMS-SQL-Outbound -Confirm -Description "HMS SQL Outbound Rule for 5433" -LocalAddress Any -LocalPort 5433 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Outbound
  New-NetFirewallRule -DisplayName HMS-Inbound -Confirm -Description "HMS Inbound Rule for port range 4200" -LocalAddress Any -LocalPort 4200 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Inbound
  New-NetFirewallRule -DisplayName HMS-Outbound -Confirm -Description "HMS Outbound Rule for port range 4200" -LocalAddress Any -LocalPort 4200 -Protocol tcp -RemoteAddress Any -RemotePort Any -Direction Outbound
}