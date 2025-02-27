# **Automated Attendance Marking System**

## **Navigate**
- [Project Overview](#project-overview)  
- [Project Components](#project-components)
- [System Configuration](#how-to-enable-wake-timers-in-windows)  
- [System Workflow](#system-workflow)  
- [How to Use](#how-to-use)  
- [Requirements](#requirements)  

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

  ![Screenshot 2025-02-27 190246](https://github.com/user-attachments/assets/1a0fad02-d619-4401-a7fa-70d5e7e36edf)

### **2. Browser Automation Script (Python + Selenium)**
- Launches a web browser to log attendance.
- Handles authentication using the provided credentials.
- Finds the current available attendance and marks it.

## **How to Enable Wake Timers in Windows**

Task Scheduler is unable to turn on your computer itself but it can wake it up from sleep. Hence you should configure your system for Task Scheduler to be able to wake your PC. Follow these steps to enable them:

### **Method 1: Using Power Options (Recommended)**
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

---

### **Method 2: Using Command Prompt (For Advanced Users)**
You can enable wake timers via the command line:

1. **Open Command Prompt as Administrator**  
   - Press `Win + S`, type **cmd**, and select **Run as administrator**.

2. **Run the Following Command:**  
   ```sh
   powercfg -change -waketimers enable

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
   - It waits for the specified **offset time** before proceeding.
   - Logs in, selects the session, and marks attendance.

3. **Completion**
   - The script validates successful execution and logs results.
   - If an error occurs, it is logged for troubleshooting.

## **How to Use**
1. **Modify windows settings and send your PC to sleep**
   - Task scheduler is unable to turn on your PC but it can wake it if configured properly.
   - Method 1: Using Power Options (Recommended)
Open Power Options

Press Win + R, type control powercfg.cpl, and press Enter.
Select Your Active Power Plan

Click Change plan settings next to your active power plan.
Modify Advanced Power Settings

Click Change advanced power settings.
In the new window, scroll down to Sleep > Allow wake timers.
Expand the option.
Enable Wake Timers

On battery: Set to Enable (if using a laptop).
Plugged in: Set to Enable.
Apply Changes

Click Apply and OK to save the settings.
   - 
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
