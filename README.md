# **Automated Attendance Marking System**

## **Navigate**
- [Project Overview](#project-overview)  
- [Project Components](#project-components)
- [System Configuration](#how-to-enable-wake-timers-in-windows)  
- [System Workflow](#system-workflow)  
- [How to Use](#how-to-use)
- [How to Install](#how-to-install)  

## **Project Overview**
This project automates attendance marking for online lectures. The app can wake the computer to mark the attendance and go to sleep. It provides a simple UI where users can enter their credentials and configure the system to mark attendance at a scheduled time.

## **Project Components**
The project consists of two main components:

### **1. Task Scheduler Integration (C#)**
- Creates and schedules tasks in **Windows Task Scheduler**.
- Configures the task to execute a **Python script** for browser automation.
- Allows users to:
  - Enter **username** and **password** for authentication.
  - Specify the **group name** to identify the correct session.
  - Set an **offset timer** to delay marking attendance after the class starts.

  ![Screenshot 2025-02-27 193157](https://github.com/user-attachments/assets/74c6a849-4e57-498b-85ac-d25c95919914)

### **2. Browser Automation Script (Python + Selenium)**
- Launches a web browser to log attendance.
- Handles authentication using the provided credentials.
- Finds the current available attendance and marks it.

## **How to Enable Wake Timers in Windows**

IF you want **Task Creator** to work even when you're not home you should allow it to wake your PC. Task Scheduler is unable to turn on your computer itself but it can wake it up from sleep. Hence you should configure your system for Task Scheduler to be able to wake your PC. Follow these steps to enable them:

1. **Open Power Options**  
   - Press `Win + R`, type `control powercfg.cpl`, and press `Enter`.

2. **Select Your Active Power Plan**  
   - Click **Change plan settings** next to your active power plan.

3. **Modify Advanced Power Settings**  
   - Click **Change advanced power settings**.
   - In the new window, scroll down to **Sleep > Allow wake timers**.
   - Expand the option.

4. **Enable Wake Timers**
   - **On battery:** Set to **Enable** (if using a laptop).  
   - **Plugged in:** Set to **Enable**.  

5. **Apply Changes**
   - Click **Apply** and **OK** to save the settings.

**Note**: there is no 100% assurance that your laptop will be turned on because some laptop may forbid the wake timers on BIOS level and the configuration depends on each model, you can browser the internet to find out.

## **System Workflow**
1. **User Input (C# UI)**
   - The user enters:
     - **Username**
     - **Password**
     - **Group Name**
     - **Offset Timer (in minutes)**
   - The application schedules a task in **Windows Task Scheduler**.

2. **Task Execution**
   - At the scheduled time, the Python script runs automatically.
   - Logs in, selects the session, and marks attendance.

3. **Completion**
   - The script validates successful execution and logs results.
   - If an error occurs, it is logged for troubleshooting.

## **How to Use**
1. **Modify windows settings and send your PC to sleep**
   - See the section [System Configuration](#how-to-enable-wake-timers-in-windows).
  
2. **Run the C# Application**  
   - Open the application and enter your **username**, **password**, **group name**, and **offset time**.
   - Click **"Save"** button.
   - It's recommended to press **"Test"** button to check whether the entered data is correct.
   - Click the **"Schedule Task"** button.
   - Wait until the tasks are created and see the result.

3. **Verify Task in Windows Task Scheduler (Optionally)** 
   - Open **Task Scheduler** (`taskschd.msc`).
   - Navigate to **Task Scheduler Library** and locate the created task.
   - Ensure it is set to run at the specified time.

4. **Let the Automation Work**  
   - At the scheduled time, the Python script will execute.
   - It will log in, navigate to the attendance page, and mark attendance.

5. **Check Logs for Errors (If Needed)**  
   - If attendance is not marked, check the logs in the Python script folder.
   - Errors such as **invalid login credentials** or **network issues** will be recorded.
   - If other errors occured please submit an **issue** in the correspondent section with the logs file.
  
## **How to Install**

To install and use the **Automated Attendance Marking System**, follow these steps:

1. **Download the Release**
   - Go to the [Release zip file](Release.zip/) of the project.
   - Download the release, which contains the necessary files to run the system.

2. **Extract the Files**
   - Extract the contents of the ZIP to a location of your choice on your PC.

3. **Running the Application**
   - Locate the executable file **TaskCreator.exe**.
   - Double-click to run the application.

If you run into any issues, review the logs for more details or create an **issue** in the correspondent section.

