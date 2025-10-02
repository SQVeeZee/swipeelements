// Assets/Editor/LogFolderContents.cs
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class LogFolderContents
{
    // Часто нужные папки для Entitas/Jenny:
    static readonly string[] DefaultFolders =
    {
        "Assets/Entitas/Jenny/Editor/Jenny",          // Jenny.Plugins*.dll, Jenny.Plugins.Unity*.dll
        "Assets/Entitas/Entitas/Editor/Plugins",      // Entitas.CodeGeneration.Plugins*.dll
        "Assets/Entitas/DesperateDevs/Editor/Plugins",// DesperateDevs*.dll
        "Assets/Generated",                           // выход генерации (если есть)
        "Assets/Project/Scripts",                     // твой runtime-код (пример)
        "Assets"                                      // корень Assets (для проверки *.cs, *.asmdef)
    };

    // Какие расширения логировать (оставь пустым, если нужно всё)
    static readonly string[] Extensions = { ".dll", ".cs", ".asmdef" };

    [MenuItem("Tools/Entitas/Jenny/Log Folders")]
    public static void LogDefault()
    {
        LogFolders(DefaultFolders);
    }

    [MenuItem("Tools/Entitas/Jenny/Pick & Log Folder")]
    public static void LogPicked()
    {
        var path = EditorUtility.OpenFolderPanel("Pick a folder under project", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            // привести к относительному пути от корня проекта, если это внутри проекта
            var proj = Directory.GetParent(Application.dataPath)!.FullName.Replace('\\','/');
            if (path.Replace('\\','/').StartsWith(proj))
            {
                var rel = "Assets" + path.Replace('\\','/').Substring(proj.Length);
                LogFolders(new[] { rel });
            }
            else
            {
                // вне проекта тоже логируем
                LogAbsolute(path);
            }
        }
    }

    static void LogFolders(string[] relativeFolders)
    {
        var sb = new StringBuilder();
        var projRoot = Directory.GetParent(Application.dataPath)!.FullName.Replace('\\','/');

        sb.AppendLine($"=== Entitas/Jenny Folders Scan ({DateTime.Now:yyyy-MM-dd HH:mm:ss}) ===");
        sb.AppendLine($"Project: {projRoot}");
        sb.AppendLine();

        foreach (var rel in relativeFolders.Distinct())
        {
            var abs = Path.GetFullPath(Path.Combine(projRoot, rel)).Replace('\\','/');
            AppendFolderReport(sb, rel, abs);
        }

        WriteReport(sb, projRoot);
    }

    static void LogAbsolute(string absPath)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== External Folder Scan ({DateTime.Now:yyyy-MM-dd HH:mm:ss}) ===");
        AppendFolderReport(sb, absPath, absPath);
        var tmp = Path.Combine(Path.GetTempPath(), "Jenny_external_scan.txt").Replace('\\','/');
        File.WriteAllText(tmp, sb.ToString(), Encoding.UTF8);
        Debug.Log($"[JennyScan] External report saved: {tmp}");
    }

    static void AppendFolderReport(StringBuilder sb, string label, string abs)
    {
        sb.AppendLine($"-- {label}");
        if (!Directory.Exists(abs))
        {
            sb.AppendLine($"   [!] Not found: {abs}");
            sb.AppendLine();
            return;
        }

        var files = Directory.EnumerateFiles(abs, "*", SearchOption.AllDirectories);
        if (Extensions != null && Extensions.Length > 0)
            files = files.Where(f => Extensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase));

        int count = 0;
        long totalBytes = 0;

        foreach (var f in files.OrderBy(f => f, StringComparer.OrdinalIgnoreCase))
        {
            count++;
            var fi = new FileInfo(f);
            totalBytes += fi.Exists ? fi.Length : 0;

            var relToProj = MakeProjectRelative(f);
            sb.AppendLine($"   - {relToProj}  |  {fi.Length} bytes  |  modified: {fi.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
        }

        sb.AppendLine($"   Total: {count} files, {totalBytes} bytes");
        sb.AppendLine();
    }

    static string MakeProjectRelative(string absolutePath)
    {
        var proj = Directory.GetParent(Application.dataPath)!.FullName.Replace('\\','/');
        var abs = absolutePath.Replace('\\','/');
        return abs.StartsWith(proj) ? abs.Substring(proj.Length + 1) : abs;
    }

    static void WriteReport(StringBuilder sb, string projRoot)
    {
        var outPath = Path.Combine(projRoot, "Jenny_scan_report.txt").Replace('\\','/');
        File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
        Debug.Log($"[JennyScan] Report saved: {outPath}\n\n{sb}");
    }
}
