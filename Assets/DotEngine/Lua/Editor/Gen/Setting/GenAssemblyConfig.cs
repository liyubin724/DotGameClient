using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenAssemblyConfig : ScriptableObject
    {
        public List<GenAssemblyData> datas = new List<GenAssemblyData>();

        private GenAssemblyData GetData(string assemblyName)
        {
            foreach (var d in datas)
            {
                if (d.Name == assemblyName)
                {
                    return d;
                }
            }
            return null;
        }

        public void AddAssembly(string assemblyName, string[] spaces)
        {
            GenAssemblyData data = GetData(assemblyName);
            if(data == null)
            {
                data = new GenAssemblyData() { Name = assemblyName };
                datas.Add(data);
            }
            data.SpaceList.AddRange(spaces);
        }

        public void RemoveAssembly(string assemblyName)
        {
            GenAssemblyData data = GetData(assemblyName);
            if(data!=null)
            {
                datas.Remove(data);
            }
        }

        public void AddSpace(string assemblyName,string space)
        {
            GenAssemblyData data = GetData(assemblyName);
            if(data == null)
            {
                data = new GenAssemblyData()
                {
                    Name = assemblyName,
                };
                data.SpaceList.Add(space);
                datas.Add(data);
            }
        }

        public void RemoveSpace(string assemblyName,string space)
        {
            GenAssemblyData data = GetData(assemblyName);
            if(data!=null)
            {
                data.SpaceList.Remove(space);

                if(data.SpaceList.Count == 0)
                {
                    RemoveAssembly(assemblyName);
                }
            }
        }

        public void Clear()
        {
            datas.Clear();
        }

        public bool HasAssembly(string assemblyName)
        {
            return GetData(assemblyName) != null;
        }

        public bool HasSpace(string assemblyName,string space)
        {
            foreach (var d in datas)
            {
                if (d.Name == assemblyName)
                {
                    return d.SpaceList.Contains(space);
                }
            }
            return false;
        }

        private static string CONFIG_ASSET_PATH = "Assets/XLua/gen_assembly_config.asset";
        public static GenAssemblyConfig GetConfig(bool createIfNotExist = true)
        {
            GenAssemblyConfig genConfig = AssetDatabase.LoadAssetAtPath<GenAssemblyConfig>(CONFIG_ASSET_PATH);
            if (genConfig == null && createIfNotExist)
            {
                genConfig = ScriptableObject.CreateInstance<GenAssemblyConfig>();
                AssetDatabase.CreateAsset(genConfig, CONFIG_ASSET_PATH);
                AssetDatabase.ImportAsset(CONFIG_ASSET_PATH);
            }
            return genConfig;
        }

        [Serializable]
        public class GenAssemblyData
        {
            public string Name;
            public List<string> SpaceList = new List<string>();
        }
    }
}
