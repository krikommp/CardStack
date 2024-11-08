using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LubanGen : MonoBehaviour
{
    [MenuItem("Tools/Run Luban Gen")]
    public static void RunBat()
    {
        // 获取Assets目录的上一级目录路径
        string projectRootPath = Directory.GetParent(Application.dataPath)?.FullName;
        // 构建相对于项目根目录的BAT脚本路径
        string batScriptPath = Path.Combine(projectRootPath, "gen.bat"); // 请替换为你的BAT脚本的相对路径

        var processInfo = new ProcessStartInfo();
        processInfo.FileName = batScriptPath;
        processInfo.CreateNoWindow = true; // 设置为true以隐藏命令行窗口
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;

        try
        {
            using (Process process = Process.Start(processInfo))
            {
                process.OutputDataReceived += ProcessOnOutputDataReceived;
                process.BeginOutputReadLine();
                
                process.WaitForExit(); // 等待BAT脚本运行完成
                int exitCode = process.ExitCode;
                if (exitCode == 0)
                {
                    Debug.Log("BAT脚本运行成功");
                }
                else
                {
                    Debug.LogError($"BAT脚本运行失败，退出码：{exitCode}");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"运行BAT脚本时发生错误：{ex.Message}");
        }
    }

    private static void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.Log(e.Data);
    }
}
