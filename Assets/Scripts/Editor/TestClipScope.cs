using DotEditor.GUIExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;

public class TestClipScope : EditorWindow
{
    [MenuItem("Test/Test ClipScope")]
    public static void ShowWin()
    {
        GetWindow<TestClipScope>().Show();
    }

    private void OnGUI()
    {
        Rect rect = new Rect(20, 20, 200, 400);
        EGUI.DrawAreaLine(rect, Color.red);

        using(new ClipScope(rect))
        {
            if(Event.current.type == EventType.MouseUp)
            {
                Debug.LogError("MouseUp in ClipREct");
            }
        }
    }
}
