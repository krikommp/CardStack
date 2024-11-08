using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SplitImgTools
{
    [MenuItem("Assets/SplitImg")]
    private static void SelectSplitImg()
    {
        string param = "\"";
        Texture2D[] textures = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D texture2D in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture2D);
            path = path.Replace("Assets/", string.Empty);
            path = Path.Combine(Application.dataPath, path);
            path = path.Replace("/", "\\");
            param += path + ";";
        }

        param += "\"";
        Debug.Log(param);
        
        // 获取Assets目录的上一级目录路径
        string projectRootPath = Directory.GetParent(Application.dataPath)?.FullName;
        // 构建相对于项目根目录的BAT脚本路径
        string toolsPath = Path.Combine(projectRootPath, "Tools/SplitImg/SplitImg.exe"); // 请替换为你的BAT脚本的相对路径
        Process p = new Process();
        p.StartInfo.FileName = toolsPath;
        p.StartInfo.Arguments = param;
        p.StartInfo.CreateNoWindow = false;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.WorkingDirectory = Application.dataPath;
        p.OutputDataReceived += DataReceivedEvent;
        p.Start();
        p.BeginOutputReadLine();
        p.WaitForExit();
        p.Close();
        p.Dispose();
        AssetDatabase.Refresh();
    }

    private static void DataReceivedEvent(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            Debug.Log(e.Data);
        }
    }
}