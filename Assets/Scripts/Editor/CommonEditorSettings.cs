using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CommonEditorSettings : EditorWindow
    {
        public        bool   saveInGameMode;
        private const string SAVE_KEY = nameof(CommonEditorSettings) + "KEY"; 
        [MenuItem("Tools/Setting")]
        public static void CreateWindow()
        {
            var window = CreateWindow<CommonEditorSettings>();
        }

        private void OnGUI()
        {
            saveInGameMode = EditorGUILayout.ToggleLeft("退出Game mode后保存数据", saveInGameMode);
        }

        private void OnEnable()
        {
            EditorJsonUtility.FromJsonOverwrite(EditorPrefs.GetString(SAVE_KEY), this);
        }

        private void OnDisable()
        {
            EditorPrefs.SetString(SAVE_KEY, EditorJsonUtility.ToJson(this));
        }
    }
}