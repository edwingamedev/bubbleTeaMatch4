using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSettingsTool : EditorWindow
{


    [MenuItem("Tools/Game Settings")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameSettingsTool));
    }
}
