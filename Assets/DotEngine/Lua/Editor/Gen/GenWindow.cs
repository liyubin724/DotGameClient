using DotEditor.GUIExtension;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenWindow : EditorWindow
    {
        [MenuItem("Game/XLua/2 Gen Window",priority =2)]
        public static void ShowWin()
        {
            var win = GetWindow<GenWindow>();
            win.titleContent = new GUIContent("Gen Window");
            win.Show();
        }

        private static readonly int TOOLBAR_HEIGHT = 18;
        private static readonly int TOOLBAR_BTN_HEIGHT = 60;
        private static readonly int SPACE_HEIGHT = 10;

        private LuaGenConfig genConfig;
        private GenData data;

        private int toolbarSelectedIndex = 0;
        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("LuaCallCSharp"),
            new GUIContent("CSharpCallLua"),
            new GUIContent("GCOptimize"),
            new GUIContent("BlackList"),
        };

        private SearchField searchField = null;
        private string searchText = string.Empty;

        private Vector2 scrollPos = Vector2.zero;

        private void OnEnable()
        {
            data = new GenData();
            genConfig = LuaGenConfig.GetConfig(true);
        }

        private void OnGUI()
        {
            if(searchField == null)
            {
                searchField = new SearchField();
                searchField.autoSetFocusOnFindCommand = true;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(TOOLBAR_HEIGHT), GUILayout.ExpandWidth(true));
            {
                GUILayout.FlexibleSpace();
                string newSearchText = searchField.OnToolbarGUI(searchText, GUILayout.Width(160));
                if (newSearchText != searchText)
                {
                    searchText = newSearchText;
                }

                if(GUILayout.Button("Setting",EditorStyles.toolbarButton,GUILayout.Width(60)))
                {
                    var win = GenAssemblyWindow.ShowWin();
                    win.ClosedCallback = () =>
                    {
                        data = new GenData();
                    };
                }

            }
            EditorGUILayout.EndHorizontal();

            int selectedIndex = GUILayout.Toolbar(toolbarSelectedIndex, toolbarContents, GUILayout.Height(TOOLBAR_BTN_HEIGHT), GUILayout.ExpandWidth(true));
            if(selectedIndex != toolbarSelectedIndex)
            {
                toolbarSelectedIndex = selectedIndex;
                scrollPos = Vector2.zero;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
            {
                if(selectedIndex == 0)
                {
                    DrawLuaCallCSharp();
                }
            }
            EditorGUILayout.EndScrollView();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(genConfig);
            }
        }

        private void DrawLuaCallCSharp()
        {
            foreach(var assemblyData in data.AssemblyDatas)
            {
                EGUILayout.DrawBoxHeader(assemblyData.Name,GUILayout.ExpandWidth(true));
                EGUI.BeginIndent();
                {
                    foreach(var typeData in assemblyData.Types)
                    {
                        string typeFullName = typeData.Type.FullName;
                        bool isSelected = genConfig.callCSharpTypeNames.IndexOf(typeFullName) >= 0;

                        bool newIsSelected = EditorGUILayout.ToggleLeft(typeFullName, isSelected);
                        if(newIsSelected!=isSelected)
                        {
                            if(newIsSelected)
                            {
                                genConfig.callCSharpTypeNames.Add(typeFullName);
                            }else
                            {
                                genConfig.callCSharpTypeNames.Remove(typeFullName);
                            }
                        }
                    }
                }
                EGUI.EndIndent();
            }
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(genConfig);
            AssetDatabase.SaveAssets();
        }

    }
}
