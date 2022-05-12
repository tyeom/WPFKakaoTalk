using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Common.Helper;

public class ProcessChecker
{
    private static Mutex _mutex;

    /// <summary>
    /// Application의 Instance 실행 여부
    /// </summary>
    /// <param name="processName">
    /// 
    /// </param>
    /// <returns>
    /// 실행중 = true / 실행중인 프로세스 없음 = false
    /// </returns>
    public static bool Do(string processName)
    {
        try
        {
            _mutex = new Mutex(false, processName);
        }
        catch (Exception ex)
        {
            return true;
        }

        try
        {
            if (!_mutex.WaitOne(0, false))
            {
                return true;
            }
        }
        catch
        {
            return true;
        }

        return false;
    }

    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindowNative(string className, string windowName);

    //Import the SetForeground API to activate it
    [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
    private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    public static extern int ShowWindowNative(IntPtr hwnd, int iCmdShow);

    [DllImport("user32.dll", EntryPoint = "IsIconic")]
    public static extern bool IsIconicNative(IntPtr hwnd);

    /// <summary>
    /// 현재 활성화된 WindowHandle을 찾는다.
    /// </summary>
    /// <param name="className"></param>
    /// <param name="windowName">Window Title.</param>
    /// <returns></returns>
    public static IntPtr FindWindow(string className, string windowName)
    {
        return FindWindowNative(className, windowName);
    }

    /// <summary>
    /// 해당 핸들을 가진 Window를 Foreground로 Activate 시킨다.
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    public static IntPtr SetForegroundWindow(IntPtr hWnd)
    {
        return SetForegroundWindowNative(hWnd);
    }

    /// <summary>
    /// 해당 핸들의 Window가 minimize된 상태인지 확인한다.
    /// </summary>
    /// <param name="hwnd"></param>
    /// <returns></returns>
    public static bool IsIconic(IntPtr hwnd)
    {
        return IsIconicNative(hwnd);
    }

    /// <summary>
    /// 해당 핸들의 Window를 Command 상태로 전환한다.
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static int ShowWindow(IntPtr hwnd, WindowShowStyle command)
    {
        return ShowWindowNative(hwnd, (int)command);
    }
}

/// <summary>Enumeration of the different ways of showing a window using 
/// ShowWindow</summary>
public enum WindowShowStyle : uint
{
    /// <summary>Hides the window and activates another window.</summary>
    /// <remarks>See SW_HIDE</remarks>
    Hide = 0,

    /// <summary>Activates and displays a window. If the window is minimized 
    /// or maximized, the system restores it to its original size and 
    /// position. An application should specify this flag when displaying 
    /// the window for the first time.</summary>
    /// <remarks>See SW_SHOWNORMAL</remarks>
    ShowNormal = 1,

    /// <summary>Activates the window and displays it as a minimized window.</summary>
    /// <remarks>See SW_SHOWMINIMIZED</remarks>
    ShowMinimized = 2,

    /// <summary>Activates the window and displays it as a maximized window.</summary>
    /// <remarks>See SW_SHOWMAXIMIZED</remarks>
    ShowMaximized = 3,

    /// <summary>Maximizes the specified window.</summary>
    /// <remarks>See SW_MAXIMIZE</remarks>
    Maximize = 3,

    /// <summary>Displays a window in its most recent size and position. 
    /// This value is similar to "ShowNormal", except the window is not 
    /// actived.</summary>
    /// <remarks>See SW_SHOWNOACTIVATE</remarks>
    ShowNormalNoActivate = 4,

    /// <summary>Activates the window and displays it in its current size 
    /// and position.</summary>
    /// <remarks>See SW_SHOW</remarks>
    Show = 5,

    /// <summary>Minimizes the specified window and activates the next 
    /// top-level window in the Z order.</summary>
    /// <remarks>See SW_MINIMIZE</remarks>
    Minimize = 6,

    /// <summary>Displays the window as a minimized window. This value is 
    /// similar to "ShowMinimized", except the window is not activated.</summary>
    /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
    ShowMinNoActivate = 7,

    /// <summary>Displays the window in its current size and position. This 
    /// value is similar to "Show", except the window is not activated.</summary>
    /// <remarks>See SW_SHOWNA</remarks>
    ShowNoActivate = 8,

    /// <summary>Activates and displays the window. If the window is 
    /// minimized or maximized, the system restores it to its original size 
    /// and position. An application should specify this flag when restoring 
    /// a minimized window.</summary>
    /// <remarks>See SW_RESTORE</remarks>
    Restore = 9,

    /// <summary>Sets the show state based on the SW_ value specified in the 
    /// STARTUPINFO structure passed to the CreateProcess function by the 
    /// program that started the application.</summary>
    /// <remarks>See SW_SHOWDEFAULT</remarks>
    ShowDefault = 10,

    /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
    /// that owns the window is hung. This flag should only be used when 
    /// minimizing windows from a different thread.</summary>
    /// <remarks>See SW_FORCEMINIMIZE</remarks>
    ForceMinimized = 11
}