using UnityEngine;
using UnityEditor;


namespace EdwinGameDev.BubbleTeaMatch4
{
    public class CustomToolWindow : EditorWindow
    {
        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;

        protected ScriptableObject[] scriptableObjects;
        protected string selectedPropertyName;
        protected string selectedProperty;

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