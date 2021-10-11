using UnityEngine;
using UnityEditor;


namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleSettingsTool : CustomToolWindow
    {
        protected string[] folders = new string[] { "Assets/_Main/Configuration/BubbleSettings" };        
        protected string saveFolder => folders[0];
        protected string colorName = "BubbleSettings";
                
        protected static void ShowWindow()
        {
            GetWindow<BubbleSettingsTool>("Bubble Settings");
        }

        private void OnGUI()
        {
            scriptableObjects = EditorUtils.GetAllInstances<BubbleSettings>(folders);
            serializedObject = new SerializedObject(scriptableObjects[0]);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

            DrawSliderBar(scriptableObjects);

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

            if (selectedProperty != null)
            {
                for (int i = 0; i < scriptableObjects.Length; i++)
                {
                    if (scriptableObjects[i].name == selectedProperty)
                    {
                        serializedObject = new SerializedObject(scriptableObjects[i]);
                        serializedProperty = serializedObject.GetIterator();
                        serializedProperty.NextVisible(true);
                        DrawProperties(serializedProperty);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("select an item from the list");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            Apply();
        }

        protected void DrawSliderBar(ScriptableObject[] prop)
        {
            foreach (ScriptableObject p in prop)
            {
                if (GUILayout.Button(p.name))
                {
                    selectedPropertyName = p.name;
                }
            }

            if (!string.IsNullOrEmpty(selectedPropertyName))
            {
                selectedProperty = selectedPropertyName;
            }

            if (GUILayout.Button("New Settings"))
            {
                var newScriptableObject = ScriptableObject.CreateInstance<BubbleSettings>();
                ScriptableObjectWindowGenerator newWindow = GetWindow<ScriptableObjectWindowGenerator>("New Settings");
                newWindow.SetupItem(newScriptableObject, saveFolder, $"{colorName}{scriptableObjects.Length + 1}");
            }
        }
    }
}