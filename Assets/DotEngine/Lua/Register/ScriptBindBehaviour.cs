using UnityEngine;
using XLua;

namespace DotEngine.Lua.Register
{
    public class ScriptBindBehaviour : MonoBehaviour
    {
        public LuaBindScript bindScript = new LuaBindScript();

        public LuaEnv Env
        {
            get 
            {
                return bindScript.Env;
            }
        }

        public LuaTable ObjTable
        {
            get
            {
                return bindScript.ObjTable;
            }
        }

        public void InitLua()
        {
            bindScript.InitLua(OnInitFinished);
        }

        protected virtual void OnInitFinished()
        {
            ObjTable.Set("gameObject", gameObject);
            ObjTable.Set("transform", transform);
        }

        protected virtual void Awake()
        {
            InitLua();

            bindScript.CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            bindScript.CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            bindScript.CallAction(LuaConst.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            bindScript.CallAction(LuaConst.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if(bindScript.IsValid())
            {
                bindScript.CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                bindScript.Dispose();
            }
        }
    }
}
