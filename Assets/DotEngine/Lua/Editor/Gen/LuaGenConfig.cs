using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class LuaGenConfig : ScriptableObject
    {
        public List<string> callCSharpTypeNames = new List<string>();
        public List<string> callLuaTypeNames = new List<string>();
        public List<string> optimizeTypeNames = new List<string>();

        public List<string> callCSharpGenericTypeNames = new List<string>();
        public List<string> callLuaGenericTypeNames = new List<string>();

        public List<string> blackDatas = new List<string>();

        private static string GEN_CONFIG_ASSET_PATH = "Assets/XLua/gen_config.asset";
        public static LuaGenConfig GetConfig(bool createIfNotExist = true)
        {
            LuaGenConfig genConfig = AssetDatabase.LoadAssetAtPath<LuaGenConfig>(GEN_CONFIG_ASSET_PATH);
            if (genConfig == null && createIfNotExist)
            {
                genConfig = ScriptableObject.CreateInstance<LuaGenConfig>();
                AssetDatabase.CreateAsset(genConfig, GEN_CONFIG_ASSET_PATH);
                AssetDatabase.ImportAsset(GEN_CONFIG_ASSET_PATH);
            }
            return genConfig;
        }
    }
}
