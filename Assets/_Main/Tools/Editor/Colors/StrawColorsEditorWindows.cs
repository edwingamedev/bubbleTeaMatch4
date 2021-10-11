using UnityEditor;


namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StrawColorsEditorWindows : ColorTool
    {
        override protected string[] folders { get; set; } = new string[] { "Assets/_Main/Configuration/StrawPresets" };
        override protected string colorName { get; set; } = "StrawColor";

        [MenuItem("Tools/Straw Colors")]
        protected static void ShowWindow()
        {
            GetWindow<StrawColorsEditorWindows>("Straw Colors");
        }

    }
}