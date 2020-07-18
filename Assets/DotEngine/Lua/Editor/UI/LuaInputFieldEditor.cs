using DotEditor.GUIExtension;
using DotEngine.Lua.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaInputField))]
    public class LuaInputFieldEditor : InputFieldEditor
    {
        SerializedProperty fieldDataProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            fieldDataProperty = serializedObject.FindProperty("fieldData");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.Update();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Field Data");
                    EGUI.BeginIndent();
                    {
                        SerializedProperty bindBehaviourProperty = fieldDataProperty.FindPropertyRelative("bindBehaviour");
                        EditorGUILayout.PropertyField(bindBehaviourProperty);

                        SerializedProperty valueChangedProperty = fieldDataProperty.FindPropertyRelative("valueChangedFuncName");
                        EditorGUILayout.PropertyField(valueChangedProperty);

                        SerializedProperty endEditProperty = fieldDataProperty.FindPropertyRelative("endEditFuncName");
                        EditorGUILayout.PropertyField(endEditProperty);
                    }
                    EGUI.EndIndent();
                }
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
