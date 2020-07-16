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
        class AssemblyData
        {
            public string AssemblyName;
            public bool isFoldout = false;
            public List<string> SpaceList = new List<string>();
        }
        [MenuItem("Test/Test GenAssembly")]
        public static void ShowWin(Action callback = null)
        {
            var win = GetWindow<GenAssemblyWindow>();
            win.titleContent = new GUIContent("Gen Assembly");
            win.closedCallback = callback;
            win.Show();
        }
        private Action closedCallback = null;
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

            searchText = searchField.OnGUI(searchText);

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
                        if (GUI.Button(btnRect, "None"))
                        {
                            assemblyConfig.RemoveAssembly(data.AssemblyName);
                        }

                        btnRect.x -= btnRect.width;
                        if (GUI.Button(btnRect, "All"))
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
            closedCallback?.Invoke();
        }

        private void OnLostFocus()
        {
            Close();
        }
    }
}
