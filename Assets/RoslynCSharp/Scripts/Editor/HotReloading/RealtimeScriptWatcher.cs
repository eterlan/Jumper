using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using RoslynCSharp.Modding;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace RoslynCSharp.HotReloading
{
    public class RealtimeScriptWatcher
    {
        // Private
        private static bool reloadingFlag = false;

        private ScriptDomain domain = null;
        private FileSystemWatcher watcher = null;
        private Queue<FileSystemEventArgs> fileEvents = new Queue<FileSystemEventArgs>();
        
        // Constructor
        public RealtimeScriptWatcher(ScriptDomain domain, string folderPath)
        {
            this.domain = domain;
            this.watcher = new FileSystemWatcher(folderPath);
            this.watcher.Filter = "*.cs";
            this.watcher.IncludeSubdirectories = true;
            this.watcher.EnableRaisingEvents = true;

            // Add listener
            this.watcher.Changed += OnFileWatcherChanged;

            EditorApplication.update += OnEditorUpdate;
            SceneView.duringSceneGui += OnSceneView;
        }

        // Methods
        private void OnFileWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if(e.ChangeType == WatcherChangeTypes.Changed)
            {
                lock (fileEvents)
                {
                    fileEvents.Enqueue(e);
                }
            }
        }

        private void OnEditorUpdate()
        {
            while (fileEvents.Count > 0)
            {
                FileSystemEventArgs e = fileEvents.Dequeue();

                // Check for play mode
                if (Application.isPlaying == true)
                {
                    // Reload all scripts in order
                    OnReloadScript(e.FullPath);
                }
            }
        }

        private void OnSceneView(SceneView view)
        {
            if (reloadingFlag == true)
                GUILayout.Label("Hot Reloading...");
        }

        private void OnReloadScript(string path)
        {
            reloadingFlag = true;

            // Time the reload duration
            Stopwatch timer = Stopwatch.StartNew();

            // Use settings options
            ScriptSecurityMode securityMode = ScriptSecurityMode.UseSettings;

            // Check for disabled security checks
            if (RoslynCSharp.Settings.HotReloadSecurityCheckCode == false)
                securityMode = ScriptSecurityMode.EnsureLoad;

            // Recompile the script
            ScriptAssembly asm = domain.CompileAndLoadFile(path, securityMode);

            // Check for success
            if(asm == null)
            {
                domain.LogCompilerOutputToConsole();
                return;
            }

            // Find the type for the changed source file
            Type mainMonoType = GetMainMonoTypeForSourceFile(path);

            // Find type with matching full name
            ScriptType reloadType = asm.FindType(mainMonoType);

            if(reloadType != null)
            {
                ScriptReplacerOptions options = ScriptReplacerOptions.DontRequireAttribute;

                // Setup options
                if (RoslynCSharp.Settings.HotReloadCopySerializedFields == true) options |= ScriptReplacerOptions.CopySerializeFields;
                if (RoslynCSharp.Settings.HotReloadCopyNonSerializedFields == true) options |= ScriptReplacerOptions.CopyNonSerializeFields;
                if (RoslynCSharp.Settings.HotReloadDestroyOriginalScript == true) options |= ScriptReplacerOptions.DestroyOriginalScript;
                if (RoslynCSharp.Settings.HotReloadDisableOriginalScript == true) options |= ScriptReplacerOptions.DisableOriginalScript;

                // Run the script replacement
                ModScriptReplacerReport report;
                ModScriptReplacer.ReplaceScriptsForActiveScene(reloadType, out report, options);

                // Check for any issues
                report.LogToConsole();
            }

            // Log reload info
            Debug.Log("Hot Reload: '" + path + "', Reload Time: " + timer.ElapsedMilliseconds.ToString() + "ms");

            reloadingFlag = false;
        }

        private Type GetMainMonoTypeForSourceFile(string path)
        {
            path = path.Replace("\\", "/");
            path = FileUtil.GetProjectRelativePath(path);

            // Load the mono script
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

            // Get the mono class
            return script.GetClass();
        }
    }
}
