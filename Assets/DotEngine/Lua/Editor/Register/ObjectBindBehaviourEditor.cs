using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEngine.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ObjectBindBehaviour))]
    public class ObjectBindBehaviourEditor : ScriptBindBehaviourEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EGUILayout.DrawHorizontalLine();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
