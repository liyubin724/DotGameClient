using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenAssemblyWindow : EditorWindow
    {
        [MenuItem("Game/XLua/1 Gen Assembly Window",priority =1)]
        public static GenAssemblyWindow ShowWin()
        {
            var win = GetWindow<GenAssemblyWindow>();
            win.titleContent = new GUIContent("Assembly Setting");
            win.Show();
            return win;
        }
        
        public Action ClosedCallback { get; set; }

        private GenAssemblyConfig assemblyConfig;
        private List<AssemblyData> assemblyDatas = new List<AssemblyData>();

        private SearchField searchField = null;
        private string searchText = string.Empty;

        private void OnEnable()
        {
            assemblyConfig = GenAssemblyConfig.GetConfig();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                string name = assembly.GetName().Name;
                if(name.IndexOf("Editor")>=0)
                {
                    continue;
                }
                AssemblyData data = new AssemblyData()
                {
                    AssemblyName = name,
                };
                foreach (var type in assembly.GetTypes())
                {
                    string ns = type.Namespace;
                    if (string.IsNullOrEmpty(ns))
                    {
                        ns = string.Empty;
                    }
                    if (!data.SpaceList.Contains(ns))
                    {
                        data.SpaceList.Add(ns);
                    }
                }

                if (data.SpaceList.Count > 0)
                {
                    data.SpaceList.Sort((item1, item2) =>
                    {
                        return item1.CompareTo(item2);
                    });
                }

                assemblyDatas.Add(data);
            }
            assemblyDatas.Sort((item1, item2) =>
            {
                return item1.AssemblyName.CompareTo(item2.AssemblyName);
            });
        }

        private Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            if (searchField == null)
            {
                searchField = new SearchField();
                searchField.autoSetFocusOnFindCommand = true;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if(GUILayout.Button("Collapse",EditorStyles.toolbarButton,GUILayout.Width(80)))
                {
                    assemblyDatas.ForEach((d) =>
                    {
                        d.isFoldout = false;
                    });
                }
                if (GUILayout.Button("Expand", EditorStyles.toolbarButton, GUILayout.Width(80)))
                {
                    assemblyDatas.ForEach((d) =>
                    {
                        d.isFoldout = true;
                    });
                }

                if (GUILayout.Button("Clear All", EditorStyles.toolbarButton, GUILayout.Width(80)))
                {
                    assemblyConfig.Clear();
                }

                GUILayout.FlexibleSpace();

                searchText = searchField.OnToolbarGUI(searchText);
            }
            EditorGUILayout.EndHorizontal();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
            {
                foreach (var data in assemblyDatas)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        Rect rect = GUILayoutUtility.GetRect(new GUIContent(data.AssemblyName), EditorStyles.foldout, GUILayout.ExpandWidth(true));
                        Rect foldoutRect = new Rect(rect.x, rect.y, rect.width - 150, rect.height);
                        data.isFoldout = EditorGUI.Foldout(foldoutRect, data.isFoldout, data.AssemblyName, true);

                        Rect btnRect = new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height);
                        if (GUI.Button(btnRect, "None",EditorStyles.miniButtonRight))
                        {
                            assemblyConfig.RemoveAssembly(data.AssemblyName);
                        }

                        btnRect.x -= btnRect.width;
                        if (GUI.Button(btnRect, "All", EditorStyles.miniButtonLeft))
                        {
                            assemblyConfig.AddAssembly(data.AssemblyName, data.SpaceList.ToArray());
                        }

                        EGUI.BeginIndent();
                        {
                            if (data.isFoldout || !string.IsNullOrEmpty(searchText))
                            {
                                foreach (var ns in data.SpaceList)
                                {
                                    if (!string.IsNullOrEmpty(searchText) && ns.ToLower().IndexOf(searchText.ToLower()) < 0)
                                    {
                                        continue;
                                    }

                                    bool isChecked = assemblyConfig.HasSpace(data.AssemblyName, ns);
                                    bool newIsChecked = EditorGUILayout.ToggleLeft(string.IsNullOrEmpty(ns) ? "--None--" : ns, isChecked);
                                    if (isChecked != newIsChecked)
                                    {
                                        if (newIsChecked)
                                        {
                                            assemblyConfig.AddSpace(data.AssemblyName, ns);
                                        }
                                        else
                                        {
                                            assemblyConfig.RemoveSpace(data.AssemblyName, ns);
                                        }
                                    }
                                }
                            }

                        }
                        EGUI.EndIndent();
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndScrollView();


            if (GUI.changed)
            {
                EditorUtility.SetDirty(assemblyConfig);
            }
        }

        private void OnDestroy()
        {
            ClosedCallback?.Invoke();
        }

        private void OnLostFocus()
        {
            if(ClosedCallback!=null)
            {
                Close();
            }
        }

        class AssemblyData
        {
            public string AssemblyName;
            public bool isFoldout = false;
            public List<string> SpaceList = new List<string>();
        }
    }
}
