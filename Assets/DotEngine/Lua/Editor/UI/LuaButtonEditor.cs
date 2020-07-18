using DotEditor.GUIExtension;
using DotEngine.Lua.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaButton))]
    public class LuaButtonEditor : SelectableEditor
    {
        SerializedProperty dataProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            dataProperty = serializedObject.FindProperty("m_ButtonData");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.Update();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Button Data");
                    EGUI.BeginIndent();
                    {
                        SerializedProperty bindBehaviourProperty = dataProperty.FindPropertyRelative("bindBehaviour");
                        EditorGUILayout.PropertyField(bindBehaviourProperty);
                        SerializedProperty funcNameProperty = dataProperty.FindPropertyRelative("funcName");
                        EditorGUILayout.PropertyField(funcNameProperty);
                    }
                    EGUI.EndIndent();
                }
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
