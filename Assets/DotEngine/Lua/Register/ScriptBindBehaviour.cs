using DotEngine.Framework;
using DotEngine.Log;
using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.Register
{
    public class ScriptBindBehaviour : MonoBehaviour
    {
        public string envName = string.Empty;
        public LuaAsset luaAsset;

        protected LuaEnv luaEnv;

        public LuaTable ObjTable { get; private set; }

        private bool isInited = false;
        public void InitLua()
        {
            if (isInited)
                return;

            LuaEnvService service = Facade.GetInstance().RetrieveService<LuaEnvService>(LuaEnvService.NAME);
            luaEnv = service.GetEnv(envName);
            if (luaEnv == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, $"ScriptBindBehaviour::InitLua->LuaEnv is null.");
                return;
            }

            if (luaAsset != null)
            {
                ObjTable = luaAsset.Instance(luaEnv);
                if (ObjTable != null)
                {
                    ObjTable.Set("gameObject", gameObject);
                    ObjTable.Set("transform", transform);

                    isInited = true;

                    OnInitFinished();
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, $"ScriptBindBehaviour::InitLua->objTable is null.");
                    return;
                }
            }
        }

        protected virtual void OnInitFinished()
        {

        }

        protected virtual void Awake()
        {
            InitLua();
            CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            CallAction(LuaConst.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            CallAction(LuaConst.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if (luaEnv == null || !luaEnv.IsValid() || ObjTable == null)
            {
                return;
            }

            CallAction(LuaConst.DESTROY_FUNCTION_NAME);

            ObjTable.Dispose();
            ObjTable = null;
        }

        public void CallAction(string funcName)
        {
            if (ObjTable != null)
            {
                ObjTable.Get<Action<LuaTable>>(funcName)?.Invoke(ObjTable);
            }
        }

        public void CallAction(string funcName, LuaTable item, int intValue)
        {
            if (ObjTable != null)
            {
                Action<LuaTable, LuaTable, int> action = ObjTable.Get<Action<LuaTable, LuaTable, int>>(funcName);
                if (action != null)
                {
                    action.Invoke(ObjTable, item, intValue);
                }
            }
        }

        public string CallFunc(string funcName, int intValue)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, int, string> func = ObjTable.Get<Func<LuaTable, int, string>>(funcName);
                if (func != null)
                {
                    return func(ObjTable, intValue);
                }
            }
            return null;
        }

    }
}
