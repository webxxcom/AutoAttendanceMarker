using Nure.NET.Types;
using Microsoft.Win32.TaskScheduler;
using Utils;
using Serilog;

namespace Automation
{
    public class CistTaskCreator
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string GroupName { get; init; }
        public required int StartOffsetMinutes { get; init; }

        private static string MarkerScriptPath { get; } = DataUtils.GetExecutablePath("script.exe");

        public void CreateTasks()
        {
            Log.Information("<<<<Application started>>>>");

            IEnumerable<Event> events = GetEvents();
            Log.Information("The groups were successfully retrieved from CIST");
            foreach (Event eventItem in events.Where(ev => ev.StartTime.HasValue))
            {
                CreateCistTaskForEvent(eventItem);
            }
            Log.Information("All events were created.\n\n");
        }

        public void CreateCistTaskForEvent(Event ev)
        {
            if (!ev.StartTime.HasValue)
            {
                // If event does not have a start time neither create a task nor notify use about it
                return;
            }

            // Vars for task settings
            DateTime taskStartTimeLocal = DateTime
                .SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(ev.StartTime.Value).DateTime, DateTimeKind.Utc)
                .ToLocalTime().AddMinutes(StartOffsetMinutes);
            long taskStartTimeUnix = (long)(taskStartTimeLocal - DateTime.UnixEpoch).TotalSeconds ;
            bool? hibernate = DataUtils.GetNullableFromConfig("power action")?.Equals("Hibernate");
            string subjectTitle = ev.Subject?.Title ?? "SomeSubject";
            string taskName = $"{subjectTitle}_Attendance_At_{taskStartTimeLocal:yyyyMMdd_HHmm}";

            TaskCreator.CreateTask(
                taskName,
                $"Automatically marks the attendance for {subjectTitle} at {taskStartTimeLocal:yyyyMMdd_HHmm}",
                taskStartTimeLocal,
                [
                    new ExecAction(MarkerScriptPath, $"{Username} {Password}", null),
                    TaskCreator.GetActionToSleep(taskStartTimeUnix, hibernate != null && ((bool)hibernate))
                ]
            );
            Log.Information("Task for {Title} at {StartTime} was created", subjectTitle,
                        taskStartTimeLocal.ToString());
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
