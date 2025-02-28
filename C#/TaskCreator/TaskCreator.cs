using Microsoft.Win32.TaskScheduler;
using Utils;

namespace Automation
{
    public static class TaskCreator
    {
        private static string PowerActionScriptPath { get; } = DataUtils.GetExecutablePath("script.ps1");

        public static void CreateTask(string name, string description, DateTime startTime, ICollection<ExecAction> actions)
        {
            using TaskService ts = new();
            TaskDefinition td = ts.NewTask();

            // Task settings
            td.RegistrationInfo.Description = description;
            td.Settings.AllowDemandStart = true;
            td.Settings.Enabled = true;
            td.Settings.WakeToRun = true;
            td.Settings.StopIfGoingOnBatteries = false;
            td.Settings.DisallowStartIfOnBatteries = false;

            // Time when task is triggered
            td.Triggers.Add(new TimeTrigger { StartBoundary = startTime });

            //Actions to trigger
            td.Actions.AddRange(actions);

            ts.RootFolder.RegisterTaskDefinition(name, td);
        }

        public static ExecAction GetActionToSleep(long taskTimeStart, bool hibernate)
        {
            string powershellExePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                @"WindowsPowerShell\v1.0\powershell.exe"
            );
            string scriptParams = $"-inputTime {taskTimeStart} -hibernate {(hibernate ? 1 : 0)}";

            return new ExecAction(
                powershellExePath,
                $"-ExecutionPolicy Bypass -File \"{PowerActionScriptPath}\" {scriptParams}",
                null
            );
        }
    }
}
