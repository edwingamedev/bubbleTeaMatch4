using UnityEngine;
using UnityEditor;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class ScriptableObjectWindowGenerator : EditorWindow
    {
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;

        private ScriptableObject newScriptableObject;        
        private string savePath;

        public void SetupItem(ScriptableObject newScriptableObject, string path, string presetName)
        {
            savePath = $"{path}/{presetName}.asset";
            this.newScriptableObject = newScriptableObject;
        }

        private void OnGUI()
        {
            serializedObject = new SerializedObject(newScriptableObject);
            serializedProperty = serializedObject.GetIterator();
            serializedProperty.NextVisible(true);
            DrawProperties(serializedProperty);

            if (GUILayout.Button("save"))
            {
                AssetDatabase.CreateAsset(newScriptableObject, savePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Close();
            }

            Apply();
        }

        protected void DrawProperties(SerializedProperty p)
        {
            while (p.NextVisible(false))
            {
                EditorGUILayout.PropertyField(p, true);
            }
        }

        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}