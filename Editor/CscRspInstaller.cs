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
        private static readonly Version RequiredLangVersion = new Version(10, 0);
        private static readonly string[] LangVersionPrefixes = { "-langversion:", "/langversion:" };

        private static readonly string RspPath = Path.Combine(UnityEngine.Application.dataPath, "csc.rsp");

        private static string EditorPrefsKey => $"MiniIT.Logger.CscRspInit::{UnityEngine.Application.dataPath}";

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
                var hasSufficientLangVersion = false;

                for (var i = 0; i < lines.Length; ++i)
                {
                    var trimmed = lines[i].Trim();

                    if (TryGetLangVersionValue(trimmed, out var value))
                    {
                        hasLangVersion = true;

                        if (TryParseLangVersion(value, out var currentVersion))
                        {
                            if (currentVersion.CompareTo(RequiredLangVersion) < 0)
                            {
                                lines[i] = LangVersionLine;
                                updated = true;
                            }
                            else
                            {
                                hasSufficientLangVersion = true;
                            }
                        }
                        else if (IsNonNumericLangVersion(value))
                        {
                            hasSufficientLangVersion = true;
                            if (force)
                            {
                                lines[i] = LangVersionLine;
                                updated = true;
                            }
                        }
                        else if (force)
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
                    var message = hasSufficientLangVersion
                        ? "Assets/csc.rsp already uses langversion >= 10."
                        : "Assets/csc.rsp already includes a langversion setting.";

                    Debug.Log(message);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to ensure Assets/csc.rsp: " + ex.Message);
            }
        }

        private static bool TryGetLangVersionValue(string trimmedLine, out string value)
        {
            foreach (var prefix in LangVersionPrefixes)
            {
                if (!trimmedLine.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var raw = trimmedLine.Substring(prefix.Length).Trim();
                var parts = raw.Split((char[])null, 2, StringSplitOptions.RemoveEmptyEntries);

                value = parts.Length > 0 ? parts[0] : string.Empty;
                return true;
            }

            value = string.Empty;
            return false;
        }

        private static bool TryParseLangVersion(string value, out Version version)
        {
            var normalized = value.Trim();

            if (string.IsNullOrEmpty(normalized))
            {
                version = null;
                return false;
            }

            if (normalized.EndsWith(".", StringComparison.Ordinal))
            {
                normalized += "0";
            }

            for (var i = 0; i < normalized.Length; i++)
            {
                var ch = normalized[i];
                if (!char.IsDigit(ch) && ch != '.')
                {
                    version = null;
                    return false;
                }
            }

            return Version.TryParse(normalized, out version);
        }

        private static bool IsNonNumericLangVersion(string value)
        {
            return value.Equals("latest", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("latestmajor", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("preview", StringComparison.OrdinalIgnoreCase);
        }
    }
}
#endif
