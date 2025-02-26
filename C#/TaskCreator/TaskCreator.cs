using Nure.NET.Types;
using Microsoft.Win32.TaskScheduler;
using Utils;
using Serilog;

namespace Automation
{
    public class TaskCreator
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string GroupName { get; init; }
        public required int StartOffsetMinutes { get; init; }

        private string MarkerScriptPath { get; } = DataUtils.GetExecutablePath("script.exe");
        private string PowerActionScriptPath { get; } = DataUtils.GetExecutablePath("script.ps1");

        public void CreateTasks()
        {
            Log.Information("<<<<Application started>>>>");
            //var eventItem = new Event
            //{
            //    Subject = new Subject { Title = "Philosophy", Id = 123 },
            //    StartTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 60 - 3600
            //};

            IEnumerable<Event> events = GetEvents().Take(1);
            Log.Information("The groups were successfully retrieved from CIST");
            foreach (Event eventItem in events)
            {
                if (eventItem.StartTime.HasValue)
                {
                    CreateTaskForEvent(eventItem);
                    Log.Information("Task for {Title} on {StartTime} was created", eventItem.Subject.Title,
                        DateTimeOffset.FromUnixTimeSeconds((long)eventItem.StartTime).DateTime.ToLongDateString);
                }
                Log.Information("All events were created.\n\n");
            }
        }

        public void CreateTaskForEvent(Event ev)
        {
            if (!ev.StartTime.HasValue)
            {
                // If event does not have a start time neither create a task nor notify use about it
                return;
            }

            // Get all required data
            string subjectTitle = ev.Subject?.Title ?? "";

            // Vars for task settings
            DateTime taskStartTimeLocal = DateTime
                .SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(ev.StartTime.Value).DateTime, DateTimeKind.Utc)
                .ToLocalTime().AddMinutes(StartOffsetMinutes);
            long taskStartTimeUnix = (long)(taskStartTimeLocal - DateTime.UnixEpoch).TotalSeconds ;
            string taskName = $"{subjectTitle}_Attendance_At_{taskStartTimeLocal:yyyyMMdd_HHmm}";

            using TaskService ts = new();
            TaskDefinition td = ts.NewTask();

            // Task settings
            td.RegistrationInfo.Description = $"Automatically marks the attendance for {subjectTitle} at {taskStartTimeLocal:yyyyMMdd_HHmm}";
            td.Settings.AllowDemandStart = true;
            td.Settings.Enabled = true;
            td.Settings.WakeToRun = true;
            td.Settings.StopIfGoingOnBatteries = false;
            td.Settings.DisallowStartIfOnBatteries = false;

            // Time when task is triggered
            td.Triggers.Add(new TimeTrigger { StartBoundary = taskStartTimeLocal });

            // Action to execute
            td.Actions.Add(new ExecAction(MarkerScriptPath, $"{Username} {Password}", null));
            AddActionToSleep(td, taskStartTimeUnix);

            ts.RootFolder.RegisterTaskDefinition(taskName, td);
        }

        private void AddActionToSleep(TaskDefinition td, long taskTimeStart)
        {
            string powershellExePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                @"WindowsPowerShell\v1.0\powershell.exe"
            );
            string scriptParams = $"-inputTime {taskTimeStart}";

            td.Actions.Add(new ExecAction(
                powershellExePath,
                $"-ExecutionPolicy Bypass -File \"{PowerActionScriptPath}\" {scriptParams}",
                null
            ));
        }

        private List<Event> GetEvents()
        {
            Console.WriteLine($"Fetching schedule for group: {GroupName}");

            long currentUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return ScheduleFetcher.GetClassesForGroup(GroupName)
                .Where(eventItem => eventItem.StartTime.HasValue && eventItem.StartTime.Value > currentUnixTimestamp)
                .ToList();
        }
    }
}
