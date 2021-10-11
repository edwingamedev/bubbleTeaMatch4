using UnityEngine;
using UnityEditor;


namespace EdwinGameDev.BubbleTeaMatch4
{
    public class BubbleColorsEditorWindows : ColorTool
    {
        override protected string[] folders { get; set; } = new string[] { "Assets/_Main/Configuration/BubbleColorsPresets" };
        override protected string colorName {get; set;} = "BubbleColor";

        [MenuItem("Tools/Bubble Colors")]
        protected static void ShowWindow()
        {
            GetWindow<BubbleColorsEditorWindows>("Bubble Colors");
        }
    }
}