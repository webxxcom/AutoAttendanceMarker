using System.Text;
using System.Configuration;
using Microsoft.Win32.TaskScheduler;
using Nure.NET.Types;
using RunTaskScript;

namespace Automation
{
    internal class TaskCreator
    {
        private static readonly DateTime epoch = DateTime.UnixEpoch;

        private static readonly string username = GetFromConfig("Username");
        private static readonly string password = GetFromConfig("Password");
        private static readonly string powerAction = GetFromConfig("Power Action");
        private static readonly int startOffsetMinutes = int.Parse(GetFromConfig("Start Offset"));

        static void Main()
        {
            new TaskCreator().CreateTasks();
        }

        public TaskCreator()
        {
            Console.WriteLine("Username: " + username);
            Console.WriteLine("Password: " + password);
            Console.WriteLine("Power Action: " + powerAction);
            Console.WriteLine("Start offset: " + startOffsetMinutes + "\n\n");
        }

        public void CreateTasks()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var eventItem = new Event
            {
                Subject = new Subject { Title = "Philosophy", Id = 123 },
                StartTime = DateTimeOffset.Now.ToUnixTimeSeconds() + 60 - 3600
            };

            Console.WriteLine(DateTimeOffset.Now.ToLocalTime());

            //foreach (var eventItem in GetEvents().Take(1))
            //{
            //    //var eventItem = GetEvents().First();
            //    if (eventItem.StartTime.HasValue)
            //    {
                    CreateTaskForEvent(eventItem);
            //        Console.WriteLine($"Task for {eventItem.Subject.Title} at {DateTimeOffset.FromUnixTimeSeconds((long)eventItem.StartTime).DateTime.ToLongDateString()} was created\n");
            //    }
            //}
        }

        public static void CreateTaskForEvent(Event ev)
        {
            if (!ev.StartTime.HasValue)
            {
                throw new InvalidDataException();
            }

            // Get all required data
            string autoMarkScriptPath = GetExecutablePath("script.exe");
            string? subjectTitle = ev.Subject?.Title ?? "";

            // Vars for task settings
            DateTime taskStartTimeLocal = DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(ev.StartTime.Value).DateTime, DateTimeKind.Utc).ToLocalTime().AddMinutes(startOffsetMinutes);
            long taskStartTimeUnix = (long)(taskStartTimeLocal - epoch).TotalSeconds ;
            string taskName = $"{subjectTitle}_Attendance_At_{taskStartTimeLocal:yyyyMMdd_HHmm}";

            Console.WriteLine(taskStartTimeLocal);

            using (TaskService ts = new())
            {
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
                td.Actions.Add(new ExecAction(autoMarkScriptPath, $"{username} {password}", null));
                AddPowerAction(td, taskStartTimeUnix, powerAction);

                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }

            Console.WriteLine($"Task created for event starting at {taskStartTimeLocal}.");
        }

        private static void AddPowerAction(TaskDefinition td, long taskTimeStart, string powerAction)
        {
            string powerShellScriptPath = GetExecutablePath("script.ps1");
            string powershellExePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                @"WindowsPowerShell\v1.0\powershell.exe"
            );
            string scriptParams = $"-inputTime {taskTimeStart} -powerAction \"{powerAction}\"";

            td.Actions.Add(new ExecAction(
                powershellExePath,
                $"-ExecutionPolicy Bypass -File \"{powerShellScriptPath}\" {scriptParams}",
                null
            ));
        }

        private static List<Event> GetEvents()
        {
            string groupName = GetFromConfig("GroupName");
            Console.WriteLine($"Fetching schedule for group: {groupName}");

            long currentUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return ScheduleFetcher.GetClassesForGroup(groupName)
                .Where(eventItem => eventItem.StartTime.HasValue && eventItem.StartTime.Value > currentUnixTimestamp)
                .ToList();
        }

        private static string GetFromConfig(string varName)
        {
            string? var = ConfigurationManager.AppSettings[varName];
            return string.IsNullOrWhiteSpace(var) 
                ? throw new KeyNotFoundException($"{varName} is missing or empty in a config file")
                : var;
        }

        private static string GetExecutablePath(string fileName)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            return File.Exists(fullPath) ?
                fullPath 
                : throw new FileNotFoundException($"The file '{fileName}' was not found at '{fullPath}'");
        }
    }
}
