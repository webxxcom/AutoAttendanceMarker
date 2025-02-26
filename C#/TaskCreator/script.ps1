param (
    [string]$inputTime,  # Input time in format 'yyyy-MM-ddTHH:mm:ss'
    [string]$powerAction  # Power action (e.g., "sleep", "shutdown")
)

# Wait for 1 minute before executing further logic
Start-Sleep -Seconds 60

# Get the last wake time from the system logs
$event = wevtutil qe System /rd:true /f:Text /c:1 /q:"<QueryList><Query Id='0' Path='System'><Select Path='System'>*[System[Provider[@Name='Microsoft-Windows-Kernel-Power']]]</Select></Query></QueryList>"
# Extract the TimeCreated field and convert it to a DateTime object
$lastWakeTime = $event | Select-String -Pattern "Date" | ForEach-Object {
    $line = $_.Line
    $timestamp = $line -replace "Date:", ""
    $timestamp.Trim()
}

# Convert the strings to DateTime objects
$lastWakeDateTime = ([datetime]$lastWakeTime).ToUniversalTime()
# Convert Unix time to DateTime
$inputDateTime = (Get-Date "1970-01-01 00:00:00").AddSeconds($inputTime)

Write-Output $lastWakeDateTime
Write-Output $inputDateTime

# Compare the times
if ($inputDateTime -gt $lastWakeDateTime) {
    Write-Output "Input time is greater than the last wake time. No action will be taken."
} else {
    Write-Output "Input time is earlier than the last wake time. Executing power action."

    Add-Type -AssemblyName System.Windows.Forms
    $PowerState = [System.Windows.Forms.PowerState]::Suspend;
    $Force = $true;
    $DisableWake = $false;
    [System.Windows.Forms.Application]::SetSuspendState($PowerState, $Force, $DisableWake);
    }
}

