using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    [Serializable]
    public class LuaAsset
    {
        [SerializeField]
        private string scriptFilePath = "";

        public bool Require(LuaEnv luaEnv)
        {
            return LuaUtility.Require(luaEnv, scriptFilePath);
        }

        public LuaTable Instance(LuaEnv luaEnv)
        {
            return LuaUtility.Instance(luaEnv, scriptFilePath);
        }
    }
}

