param (
    [int]$inputTime,  # Input time in UnixSeconds
    [int]$hibernate  # Whether to hibernate or not, passed as 1 or 0
)

# Get the last wake time from the system logs
$event = wevtutil qe System /rd:true /f:Text /c:1 /q:"<QueryList><Query Id='0' Path='System'><Select Path='System'>*[System[Provider[@Name='Microsoft-Windows-Kernel-Power']]]</Select></Query></QueryList>"
# Extract the TimeCreated field and convert it to a DateTime object
$lastWakeTime = $event | Select-String -Pattern "Date" | ForEach-Object {
    $line = $_.Line
    $timestamp = $line -replace "Date:", ""
    $timestamp.Trim()
}

$lastWakeDateTime = ([datetime]$lastWakeTime).ToUniversalTime()
$inputDateTime = (Get-Date "1970-01-01 00:00:00").AddSeconds($inputTime)

if ($inputDateTime -gt $lastWakeDateTime) {
    Write-Output "Input time is greater than the last wake time. No action will be taken."
} else {
    Add-Type -AssemblyName System.Windows.Forms
    if ($hibernate -eq 1) {
        $PowerState = [System.Windows.Forms.PowerState]::Hibernate
    } else {
        $PowerState = [System.Windows.Forms.PowerState]::Suspend
    }
    
    $Force = $false
    $DisableWake = $false
    [System.Windows.Forms.Application]::SetSuspendState($PowerState, $Force, $DisableWake)
}
