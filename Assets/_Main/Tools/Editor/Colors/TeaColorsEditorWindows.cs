using UnityEngine;
using UnityEditor;


namespace EdwinGameDev.BubbleTeaMatch4
{
    public class TeaColorsEditorWindows : ColorTool
    {
        override protected string[] folders { get; set; } = new string[] { "Assets/_Main/Configuration/TeaPresets" };        
        override protected string colorName {get; set;} =  "TeaColor";

        [MenuItem("Tools/Tea Colors")]
        protected static void ShowWindow()
        {
            GetWindow<TeaColorsEditorWindows>("Tea Colors");
        }
    }
}