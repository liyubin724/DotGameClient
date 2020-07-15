using DotEngine.Lua.Register;
using UnityEditor;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ScriptBindBehaviour))]
    public class ScriptBindBehaviourEditor : Editor
    {
        SerializedProperty envNameProperty;
        SerializedProperty luaAssetProperty;

        protected virtual void OnEnable()
        {
            envNameProperty = serializedObject.FindProperty("envName");
            luaAssetProperty = serializedObject.FindProperty("luaAsset");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(envNameProperty);
                EditorGUILayout.PropertyField(luaAssetProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
