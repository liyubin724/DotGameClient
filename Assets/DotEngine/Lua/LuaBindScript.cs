using DotEngine.Framework;
using DotEngine.Log;
using System;
using XLua;
using UnityEngine;

namespace DotEngine.Lua
{
    [Serializable]
    public class LuaBindScript
    {
        [SerializeField]
        private string m_EnvName = string.Empty;
        public string EnvName 
        {
            get
            {
                return m_EnvName;
            }
        }
        [SerializeField]
        private string m_ScriptFilePath = null;

        public LuaEnv Env { get; private set; }
        public LuaTable ObjTable { get; private set; }

        private bool m_IsInited = false;

        public bool IsValid()
        {
            if (Env == null || !Env.IsValid() || ObjTable == null)
            {
                return false;
            }
            return true;
        }

        public void InitLua(Action initFinished)
        {
            if(m_IsInited)
            {
                return;
            }

            LuaEnvService service = Facade.GetInstance().RetrieveService<LuaEnvService>(LuaEnvService.NAME);
            Env = service.GetEnv(m_EnvName);
            if (Env == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, $"LuaScript::InitLua->LuaEnv is null.");
                return;
            }

            if (!string.IsNullOrEmpty(m_ScriptFilePath))
            {
                ObjTable = LuaUtility.Instance(Env, m_ScriptFilePath);

                if (ObjTable != null)
                {
                    m_IsInited = true;
                    initFinished?.Invoke();
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, $"ScriptBindBehaviour::InitLua->objTable is null.");
                }
            }
        }

        public void Dispose()
        {
            ObjTable?.Dispose();
            ObjTable = null;
            Env = null;
            m_IsInited = false;
        }

        public void CallAction(string funcName)
        {
            if (ObjTable != null)
            {
                Action<LuaTable> action = ObjTable.Get<Action<LuaTable>>(funcName);
                if(action!=null)
                {
                    action(ObjTable);
                }else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBindScript::CallAction->function not found.name = " + funcName);
                }
            }
        }

        public void CallAction<T>(string funcName,T value)
        {
            if (ObjTable != null)
            {
                Action<LuaTable, T> action = ObjTable.Get<Action<LuaTable, T>>(funcName);
                if(action!=null)
                {
                    action(ObjTable, value);
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBindScript::CallAction->function not found.name = " + funcName);
                }
            }
        }

        public void CallAction<T1,T2>(string funcName,T1 value1,T2 value2)
        {
            if(ObjTable!=null)
            {
                Action<LuaTable, T1, T2> action = ObjTable.Get<Action<LuaTable, T1, T2>>(funcName);
                if(action!=null)
                {
                    action(ObjTable, value1, value2);
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBindScript::CallAction->function not found.name = " + funcName);
                }
            }
        }

        public R CallFunc<R>(string funcName)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, R> func = ObjTable.Get<Func<LuaTable, R>>(funcName);
                if (func != null)
                {
                    return func(ObjTable);
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBindScript::CallFunc->function not found.name = " + funcName);
                }
            }
            return default(R);
        }

        public R CallFunc<T,R>(string funcName,T value)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, T,R> func = ObjTable.Get<Func<LuaTable, T,R>>(funcName);
                if (func != null)
                {
                    return func(ObjTable, value);
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaBindScript::CallFunc->function not found.name = " + funcName);
                }
            }
            return default(R);
        }
    }
}
