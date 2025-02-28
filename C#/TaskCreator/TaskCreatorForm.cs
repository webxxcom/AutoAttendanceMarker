using Automation;
using Microsoft.Win32.TaskScheduler;
using Nure.NET;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utils;

namespace TaskCreatorUI
{
    public partial class AutoMarker : Form
    {
        private const string usernameConfigName = "username";
        private const string passwordConfigName = "password";
        private const string timerOffsetConfigName = "timer offset";
        private const string powerActionConfigName = "power action";
        private const string groupConfigName = "group";


        public AutoMarker()
        {
            InitializeComponent();
        }

        private void SetCurrentUsernameField()
        {
            usernameField.Text = DataUtils.GetNullableFromConfig(usernameConfigName) ?? "";
        }

        private void SetCurrentPasswordField()
        {
            passwordField.Text = DataUtils.GetNullableFromConfig(passwordConfigName) ?? "";
        }

        private void SetComboGroups()
        {
            string[]? groups = Cist.GetGroups()?.Select(g => g.Name).ToArray();
            if (groups == null || groups.Length == 0)
            {
                MessageBox.Show("Sorry, the application was unable to retrieve groups " +
                    "from CIST site. You can write the data manually in the corresspondent field");
            }
            groupsComboBox.Items.AddRange(groups ?? []);
            groupsComboBox.SelectedItem = DataUtils.GetNullableFromConfig(groupConfigName) ?? "";
        }

        private void SetOffsetTimer()
        {
            timerOffsetNumeric.Value =
                long.TryParse(DataUtils.GetNullableFromConfig(timerOffsetConfigName), out long timerOffsetVal)
            ? timerOffsetVal
            : 0;
        }

        private void SetPowerAction()
        {
            string? powerActionValue = DataUtils.GetNullableFromConfig(powerActionConfigName);

            powerActionComboBox.DataSource = new List<string> { "Hibernate", "Sleep"};
            if (!string.IsNullOrEmpty(powerActionValue))
            {
                powerActionComboBox.SelectedIndex = powerActionComboBox.Items.IndexOf(powerActionValue);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetComboGroups();
            SetCurrentUsernameField();
            SetCurrentPasswordField();
            SetOffsetTimer();
            SetPowerAction();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            Automation.CistTaskCreator tc = new()
            {
                Username = usernameField.Text,
                Password = passwordField.Text,
                StartOffsetMinutes = ((int)timerOffsetNumeric.Value),
                GroupName = groupsComboBox.Text
            };
            tc.CreateTasks();
            MessageBox.Show("Tasks were successfully created and can be checked in Task Scheduler");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DataUtils.SaveDataToConfig(new Dictionary<string, string>{
                {usernameConfigName, usernameField.Text},
                {passwordConfigName, passwordField.Text},
                {timerOffsetConfigName, timerOffsetNumeric.Value.ToString() },
                {groupConfigName, groupsComboBox.Text },
                {powerActionConfigName, powerActionComboBox.Text }
            });
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            Func<string, string> error_message = txt => $"Unable to test dl.nure.ua login without {txt}. Please fill the correspondent field";

            string? username = DataUtils.GetNullableFromConfig(usernameConfigName);
            if (username == null)
            {
                MessageBox.Show(error_message("username"));
                return;
            }

            string? password = DataUtils.GetNullableFromConfig(passwordConfigName);
            if (password == null)
            {
                MessageBox.Show(error_message("password"));
                return;
            }

            ProcessStartInfo psi = new()
            {
                FileName = DataUtils.GetExecutablePath("login_script.exe"),
                Arguments = $"{username} {password}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new() { StartInfo = psi };
            process.Start();
            process.WaitForExit();

            string exitMessage = process.ExitCode == 1
                ? "The login data is correct and dl.nure.ua can be accessed using these credentials"
                : $"The application was unable to access dl.nure.ua using current credentials: " +
                $"{username}, {password}. Maybe you forgot to save your entered data?";
            MessageBox.Show(exitMessage);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = DataUtils.GetFromConfig("GithubSourceLink"),
                UseShellExecute = true
            });
        }

        private void ReportIssueLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = DataUtils.GetFromConfig("GithubSourceLink") + "/issues",
                UseShellExecute = true
            });
        }


        [LibraryImport("PowrProf.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetSuspendState([MarshalAs(UnmanagedType.Bool)] bool hibernate, [MarshalAs(UnmanagedType.Bool)] bool forceCritical, [MarshalAs(UnmanagedType.Bool)] bool disableWakeEvent);

        private void TestWakeButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Note that the testing requires that your PC will be" +
                " immediately sent to sleep, woken up after approximately 1 minute and send back to sleep",
                "Warning", MessageBoxButtons.OKCancel);
            if (dr.Equals(DialogResult.OK))
            {
                long startTime = DateTimeOffset.Now.AddMinutes(1).ToUnixTimeSeconds();
                bool hibernate = "Hibernate".Equals(powerActionComboBox.Text);
                TaskCreator.CreateTask(
                    "Test_Wake_Timer_Task",
                    "The task created by Task Creator to test whether wake timers are applicable in this system or not",
                    DateTime.Now.AddMinutes(1),
                    [
                        new ExecAction("powershell.exe", "-Command Start-Sleep -Seconds 5"),
                        TaskCreator.GetActionToSleep(startTime, hibernate)
                    ]
                );
                SetSuspendState(hibernate, true, false);
                MessageBox.Show($"Your PC must have been sent back to {powerActionComboBox.Text} " +
                    $"after several seconds... If it wasn't than there is some configuration problem. " +
                    $"Check the configuration section on github page");
            }
        }
    }
}
