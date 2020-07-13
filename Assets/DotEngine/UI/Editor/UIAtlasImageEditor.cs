using DotEditor.GUIExtension.Windows;
using DotEditor.UI;
using System;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.U2D;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIAtlasImage), true)]
    public class UIAtlasImageEditor : ImageEditor
    {
        private GUIContent m_AtlasContent;
        private SerializedProperty m_Atlas;
        private SerializedProperty m_SpriteName;

        private string[] m_SpriteInAtlasNames = null;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_AtlasContent = new GUIContent("");
            m_Atlas = serializedObject.FindProperty("m_atlas");
            m_SpriteName = serializedObject.FindProperty("m_SpriteName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var spriteAtlas = m_Atlas.objectReferenceValue as SpriteAtlas;
            if (spriteAtlas != null && m_SpriteInAtlasNames == null)
            {
                m_SpriteInAtlasNames = GetSpriteNames(spriteAtlas);
            }
            EditorGUI.BeginChangeCheck();
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Atlas", "DropDown", GUILayout.Width(76f)))
                        ComponentSelector.Show<SpriteAtlas>(OnSelectAtlas);
                    EditorGUILayout.PropertyField(m_Atlas, m_AtlasContent);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (spriteAtlas == null)
                {
                    m_SpriteInAtlasNames = new string[0];
                }
                else
                {
                    m_SpriteInAtlasNames = GetSpriteNames(spriteAtlas);
                }
            }
            if (m_SpriteInAtlasNames != null && m_SpriteInAtlasNames.Length > 0)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Sprite", "DropDown", GUILayout.Width(76f)))
                    {
                        UISetting.atlas = spriteAtlas;
                        UISetting.selectedSprite = m_SpriteName.stringValue;
                        SpriteSelector.Show(SelectSprite);
                    }
                    int index = Array.IndexOf(m_SpriteInAtlasNames, m_SpriteName.stringValue);
                    if (index < 0)
                        index = 0;

                    index = EditorGUILayout.Popup("", index, m_SpriteInAtlasNames);
                    string newSpriteName = m_SpriteInAtlasNames[index];

                    if (m_SpriteName.stringValue != newSpriteName)
                    {
                        Array.ForEach<UnityObject>(targets, (t) =>
                        {
                            (t as UIAtlasImage).SpriteName = newSpriteName;
                        });
                    }
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                Array.ForEach<UnityObject>(targets, (t) =>
                {
                    (t as UIAtlasImage).SpriteName = "";
                });
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            base.OnInspectorGUI();

        }

        void OnSelectAtlas(UnityObject obj)
        {
            serializedObject.Update();
            SerializedProperty sp = serializedObject.FindProperty("m_atlas");
            sp.objectReferenceValue = obj;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
            UISetting.atlas = obj as SpriteAtlas;
        }

        void SelectSprite(string spriteName)
        {
            Array.ForEach<UnityObject>(targets, (t) =>
            {
                (t as UIAtlasImage).SpriteName = spriteName;
            });
        }

        private string[] GetSpriteNames(SpriteAtlas atlas)
        {
            string[] names = new string[0];
            Sprite[] mSprite = new Sprite[atlas.spriteCount];
            atlas.GetSprites(mSprite);
            if (mSprite.Length > 0)
            {
                string[] mSpriteName = new string[mSprite.Length];
                for (int i = 0; i < mSprite.Length; ++i)
                {
                    mSpriteName[i] = mSprite[i].name.Replace("(Clone)", "");
                }
                names = mSpriteName;
            }
            return names;
        }
    }
}
