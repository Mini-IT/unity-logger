#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace MiniIT.Logging.Editor
{
    [InitializeOnLoad]
    internal static class CscRspInstaller
    {
        private const string LangVersionLine = "-langversion:10";
        private const string LangVersion = "10";

        private static readonly string RspPath = Path.Combine(Application.dataPath, "csc.rsp");

        private static string EditorPrefsKey => $"MiniIT.Logger.CscRspInit::{Application.dataPath}";

        static CscRspInstaller()
        {
            EditorApplication.delayCall += RunOnceOnLoad;
        }

        private static void RunOnceOnLoad()
        {
            if (EditorPrefs.GetBool(EditorPrefsKey, false))
            {
                return;
            }

            EnsureCscRsp(force: false);
            EditorPrefs.SetBool(EditorPrefsKey, true);
        }

        [MenuItem("MiniIT/Logger/Ensure C# 10 (csc.rsp)")]
        private static void EnsureCscRspMenu()
        {
            EnsureCscRsp(force: true);
        }

        private static void EnsureCscRsp(bool force)
        {
            try
            {
                if (!File.Exists(RspPath))
                {
                    File.WriteAllText(RspPath, LangVersionLine + Environment.NewLine);
                    AssetDatabase.Refresh();

                    Debug.Log("Created Assets/csc.rsp with -langversion:10.");

                    return;
                }

                var lines = File.ReadAllLines(RspPath);
                var updated = false;
                var hasLangVersion = false;

                for (var i = 0; i < lines.Length; ++i)
                {
                    var trimmed = lines[i].Trim();

                    if (trimmed.StartsWith("-langversion:", StringComparison.OrdinalIgnoreCase) ||
                        trimmed.StartsWith("/langversion:", StringComparison.OrdinalIgnoreCase))
                    {
                        hasLangVersion = true;

                        if (!trimmed.Equals(LangVersionLine, StringComparison.OrdinalIgnoreCase) &&
                            !trimmed.Equals("/langversion:" + LangVersion, StringComparison.OrdinalIgnoreCase))
                        {
                            lines[i] = LangVersionLine;
                            updated = true;
                        }
                    }
                }

                if (!hasLangVersion)
                {
                    var newLines = lines.ToList();

                    newLines.Add(LangVersionLine);

                    lines = newLines.ToArray();
                    updated = true;
                }

                if (updated)
                {
                    File.WriteAllLines(RspPath, lines);
                    AssetDatabase.Refresh();

                    Debug.Log("Updated Assets/csc.rsp to use -langversion:10.");
                }
                else if (force)
                {
                    Debug.Log("Assets/csc.rsp already uses -langversion:10.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to ensure Assets/csc.rsp: " + ex.Message);
            }
        }
    }
}
#endif
