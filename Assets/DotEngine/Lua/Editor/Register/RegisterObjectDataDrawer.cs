using DotEditor.GUIExtension;
using DotEngine.Lua.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DotEngine.Lua.Register.RegisterObjectData;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Lua.Register
{
    public class ObjectDataDrawer
    {
        private ObjectData data;
        private bool isArrayItem;

        public ObjectData Data { get => data; }

        public ObjectDataDrawer(ObjectData data,bool isArrayItem = false)
        {
            this.data = data;
            this.isArrayItem = isArrayItem;
        }

        public void OnGUI(Rect rect)
        {
            Rect drawRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            if (!isArrayItem)
            {
                data.name = EditorGUI.TextField(drawRect, "Name", data.name);
                drawRect.y += drawRect.height;
            }

            GameObject newGO = (GameObject)EditorGUI.ObjectField(drawRect, "Object", data.obj, typeof(GameObject), true);
            if (newGO != data.obj)
            {
                if (newGO == null)
                {
                    data.regObj = null;
                    data.typeName = string.Empty;
                }
                else
                {
                    data.obj = newGO;
                    if (string.IsNullOrEmpty(data.name))
                    {
                        data.name = data.obj.name;
                    }
                    if (data.regObj == null)
                    {
                        data.regObj = data.obj;
                        data.typeName = "GameObject";
                    }
                }
            }

            drawRect.y += drawRect.height;
            EditorGUI.BeginDisabledGroup(true);
            {
                if (data.regObj != null)
                {
                    EditorGUI.ObjectField(drawRect, "Reg Obj", data.regObj, data.regObj.GetType(), true);
                }
                else
                {
                    EditorGUI.ObjectField(drawRect, "Reg Obj", data.regObj, typeof(GameObject), true);
                }
            }
            EditorGUI.EndDisabledGroup();

            drawRect.y += drawRect.height;
            if (data.obj == null)
            {
                EditorGUI.LabelField(drawRect, "Type Name", "Null");
            }
            else
            {
                List<string> componentNames = new List<string>();
                List<UnityObject> components = new List<UnityObject>();
                
                componentNames.Add(typeof(GameObject).Name);
                components.Add(data.obj);

                GameObject uObj = data.obj as GameObject;
                var comArr = uObj.GetComponents<Component>();
                foreach (var component in comArr)
                {
                    string componentTypeName = component.GetType().Name;
                    if (componentNames.IndexOf(componentTypeName) < 0)
                    {
                        componentNames.Add(componentTypeName);
                        components.Add(component);
                    }
                }
                string[] typeNames = componentNames.ToArray();
                string newTypeName = EGUI.DrawPopup<string>(drawRect, "Type Name", typeNames, typeNames, data.typeName);
                if(newTypeName != data.typeName)
                {
                    data.typeName = newTypeName;
                    data.regObj = components[componentNames.IndexOf(newTypeName)];
                }

                drawRect.y += drawRect.height;
                EditorGUI.LabelField(drawRect, ":SSSSS");
                EGUI.DrawHorizontalLine(drawRect,Color.red);
                EGUI.DrawAreaLine(drawRect, Color.yellow);
            }
        }
    }

    public class RegisterObjectDataDrawer
    {
        private RegisterObjectData objectData = null;

        private List<ObjectDataDrawer> objectDataDrawers = null;
        ReorderableList objectDatasRL = null;

        public RegisterObjectDataDrawer(RegisterObjectData data)
        {
            objectData = data;
            objectDataDrawers = new List<ObjectDataDrawer>();
            if(data.objectDatas.Length>0)
            {
                foreach(var d in data.objectDatas)
                {
                    ObjectDataDrawer drawer = new ObjectDataDrawer(d, false);
                    objectDataDrawers.Add(drawer);
                }
            }
        }

        public void OnGUILayout()
        {
            if(objectDatasRL == null)
            {
                objectDatasRL = new ReorderableList(objectDataDrawers, typeof(ObjectDataDrawer), true, true, true, true);
                objectDatasRL.elementHeight = GetObjectDataHeight(false);
                objectDatasRL.drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, "Object Data List");
                };
                objectDatasRL.onAddCallback = (list) =>
                {
                    objectDataDrawers.Add(new ObjectDataDrawer(new ObjectData(), false));
                };
                objectDatasRL.drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    objectDataDrawers[index].OnGUI(rect);
                };
            }

            objectDatasRL.DoLayoutList();

            if(GUI.changed)
            {
                objectData.objectDatas = (from d in objectDataDrawers select d.Data).ToArray();
            }
        }

        private float GetObjectDataHeight(bool isArrayItem)
        {
            return EditorGUIUtility.singleLineHeight * (isArrayItem?4:5);
        }

    }
}
