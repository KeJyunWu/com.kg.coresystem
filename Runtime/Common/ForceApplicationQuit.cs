using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ForceApplicationQuit : MonoBehaviour
{

#if !UNITY_EDITOR
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern int TerminateProcess(IntPtr hProcess, uint uExitCode);
#endif

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        IntPtr id = System.Diagnostics.Process.GetCurrentProcess().Handle;
        TerminateProcess(id, 0);
#endif
    }
}
