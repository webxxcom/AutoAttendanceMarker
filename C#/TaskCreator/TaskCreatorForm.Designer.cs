namespace TaskCreatorUI
{
    partial class AutoMarker
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            usernameField = new TextBox();
            passwordField = new TextBox();
            label3 = new Label();
            groupsComboBox = new ComboBox();
            testLoginButton = new Button();
            runButton = new Button();
            saveButton = new Button();
            label4 = new Label();
            timerOffsetNumeric = new NumericUpDown();
            sourceCodeLinkLabel = new LinkLabel();
            reportIssueLinkLabel = new LinkLabel();
            testWakeButton = new Button();
            label5 = new Label();
            powerActionComboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)timerOffsetNumeric).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 26);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 0;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 83);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // usernameField
            // 
            usernameField.Location = new Point(99, 23);
            usernameField.Name = "usernameField";
            usernameField.Size = new Size(223, 23);
            usernameField.TabIndex = 2;
            // 
            // passwordField
            // 
            passwordField.Location = new Point(99, 80);
            passwordField.Name = "passwordField";
            passwordField.Size = new Size(223, 23);
            passwordField.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 138);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 4;
            label3.Text = "Your group";
            // 
            // groupsComboBox
            // 
            groupsComboBox.FormattingEnabled = true;
            groupsComboBox.Location = new Point(99, 135);
            groupsComboBox.Name = "groupsComboBox";
            groupsComboBox.Size = new Size(121, 23);
            groupsComboBox.TabIndex = 5;
            // 
            // testLoginButton
            // 
            testLoginButton.Location = new Point(25, 328);
            testLoginButton.Name = "testLoginButton";
            testLoginButton.Size = new Size(121, 40);
            testLoginButton.TabIndex = 6;
            testLoginButton.Text = "Test dl login";
            testLoginButton.UseVisualStyleBackColor = true;
            testLoginButton.Click += TestButton_Click;
            // 
            // runButton
            // 
            runButton.Location = new Point(90, 462);
            runButton.Name = "runButton";
            runButton.Size = new Size(201, 104);
            runButton.TabIndex = 7;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = true;
            runButton.Click += RunButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(135, 397);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(121, 40);
            saveButton.TabIndex = 8;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 188);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 9;
            label4.Text = "Offset timer";
            // 
            // timerOffsetNumeric
            // 
            timerOffsetNumeric.Location = new Point(100, 186);
            timerOffsetNumeric.Name = "timerOffsetNumeric";
            timerOffsetNumeric.Size = new Size(57, 23);
            timerOffsetNumeric.TabIndex = 10;
            timerOffsetNumeric.TextAlign = HorizontalAlignment.Center;
            // 
            // sourceCodeLinkLabel
            // 
            sourceCodeLinkLabel.AutoSize = true;
            sourceCodeLinkLabel.Location = new Point(316, 615);
            sourceCodeLinkLabel.Name = "sourceCodeLinkLabel";
            sourceCodeLinkLabel.Size = new Size(74, 15);
            sourceCodeLinkLabel.TabIndex = 11;
            sourceCodeLinkLabel.TabStop = true;
            sourceCodeLinkLabel.Text = "Source Code";
            sourceCodeLinkLabel.LinkClicked += LinkLabel1_LinkClicked;
            // 
            // reportIssueLinkLabel
            // 
            reportIssueLinkLabel.AutoSize = true;
            reportIssueLinkLabel.Location = new Point(316, 590);
            reportIssueLinkLabel.Name = "reportIssueLinkLabel";
            reportIssueLinkLabel.Size = new Size(71, 15);
            reportIssueLinkLabel.TabIndex = 12;
            reportIssueLinkLabel.TabStop = true;
            reportIssueLinkLabel.Text = "Report issue";
            reportIssueLinkLabel.LinkClicked += ReportIssueLinkLabel_LinkClicked;
            // 
            // testWakeButton
            // 
            testWakeButton.Location = new Point(246, 328);
            testWakeButton.Name = "testWakeButton";
            testWakeButton.Size = new Size(121, 40);
            testWakeButton.TabIndex = 13;
            testWakeButton.Text = "Test wake timer";
            testWakeButton.UseVisualStyleBackColor = true;
            testWakeButton.Click += TestWakeButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 238);
            label5.Name = "label5";
            label5.Size = new Size(76, 15);
            label5.TabIndex = 14;
            label5.Text = "Power action";
            // 
            // powerActionComboBox
            // 
            powerActionComboBox.FormattingEnabled = true;
            powerActionComboBox.Location = new Point(100, 235);
            powerActionComboBox.Name = "powerActionComboBox";
            powerActionComboBox.Size = new Size(121, 23);
            powerActionComboBox.TabIndex = 15;
            // 
            // AutoMarker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 637);
            Controls.Add(powerActionComboBox);
            Controls.Add(label5);
            Controls.Add(testWakeButton);
            Controls.Add(reportIssueLinkLabel);
            Controls.Add(sourceCodeLinkLabel);
            Controls.Add(timerOffsetNumeric);
            Controls.Add(label4);
            Controls.Add(saveButton);
            Controls.Add(runButton);
            Controls.Add(testLoginButton);
            Controls.Add(groupsComboBox);
            Controls.Add(label3);
            Controls.Add(passwordField);
            Controls.Add(usernameField);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AutoMarker";
            Text = "AutoMarker";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)timerOffsetNumeric).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox usernameField;
        private TextBox passwordField;
        private Label label3;
        private ComboBox groupsComboBox;
        private Button testLoginButton;
        private Button runButton;
        private Button saveButton;
        private Label label4;
        private NumericUpDown timerOffsetNumeric;
        private LinkLabel sourceCodeLinkLabel;
        private LinkLabel reportIssueLinkLabel;
        private Button testWakeButton;
        private Label label5;
        private ComboBox powerActionComboBox;
    }
}
