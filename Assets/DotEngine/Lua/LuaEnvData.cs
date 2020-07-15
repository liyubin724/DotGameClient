using System;
using XLua;

namespace DotEngine.Lua
{
    internal class LuaEnvData
    {
        public LuaEnv Env { get; private set; }
        public ScriptLoader Loader { get; private set; }
        public LuaTable MgrTable { get; private set; }

        private Action<LuaTable, float> updateAction = null;

        public LuaEnvData(LuaEnv luaEnv, ScriptLoader loader)
        {
            Env = luaEnv;
            if (luaEnv != null && loader != null)
            {
                luaEnv.AddLoader(loader.LoadScript);
            }
        }

        public void SetMgr(LuaTable mgrTable)
        {
            MgrTable = mgrTable;
            if (MgrTable != null)
            {
                Action<LuaTable> luaMgrStartAction = mgrTable.Get<Action<LuaTable>>(LuaConst.START_FUNCTION_NAME);
                luaMgrStartAction?.Invoke(mgrTable);

                updateAction = mgrTable.Get<Action<LuaTable, float>>(LuaConst.UPDATE_FUNCTION_NAME);
            }
        }

        public bool IsValid()
        {
            return Env != null && Env.IsValid();
        }

        public void DoUpdate(float deltaTime)
        {
            if (IsValid() && updateAction != null)
            {
                updateAction(MgrTable, deltaTime);
            }
        }

        public void Tick()
        {
            if(IsValid())
            {
                Env.Tick();
            }
        }

        public void GC()
        {
            if(IsValid())
            {
                Env.FullGc();
            }
        }

        public void Dispose()
        {
            updateAction = null;

            MgrTable?.Dispose();
            MgrTable = null;

            Env?.Dispose();
            Env = null;

        }
    }
}
