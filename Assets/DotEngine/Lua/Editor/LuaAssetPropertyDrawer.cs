using DotEngine.Lua;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua
{
    [CustomPropertyDrawer(typeof(LuaAsset))]
    public class LuaAssetPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("scriptFilePath");

            TextAsset scriptAsset = null;
            string scriptAssetPath = string.Empty;
            if (!string.IsNullOrEmpty(scriptPathProperty.stringValue))
            {
                scriptAssetPath = LuaConst.GetScriptAssetPath(scriptPathProperty.stringValue);
                scriptAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);
                if(scriptAsset == null)
                {
                    scriptAsset = null;
                    scriptPathProperty.stringValue = null;
                    scriptAssetPath = string.Empty;
                }
            }

            Rect rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            TextAsset newScriptAsset = (TextAsset)EditorGUI.ObjectField(rect, "Lua Script", scriptAsset, typeof(TextAsset), false);

            rect.y += rect.height;
            rect.x += 20;
            rect.width -= 20;
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUI.TextField(rect, "Script Path",scriptAssetPath);
            }
            EditorGUI.EndDisabledGroup();

            if (newScriptAsset != scriptAsset)
            {
                if(newScriptAsset == null)
                {
                    scriptPathProperty.stringValue = null;
                }else
                {
                    string assetPath = AssetDatabase.GetAssetPath(newScriptAsset);
                    scriptPathProperty.stringValue = LuaConst.GetScriptPath(assetPath);
                }
            }
        }

    }
}
