using DotEngine.Lua;
using DotEngine.Lua.Register;
using SuperScrollView;
using UnityEngine;

namespace Assets.DotEngine.Lua.ListView
{
    [RequireComponent(typeof(LoopListViewItem2))]
    public class LuaListViewItem : LoopListViewItem2
    {
        private const string ITEM_NAME = "listViewItem";

        public LuaBindScript BindScript = new LuaBindScript();
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected virtual void Awake()
        {
            BindScript.InitLua(OnInitFinished);
            BindScript.CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void OnInitFinished()
        {
            registerObjectData.RegisterToLua(BindScript.Env, BindScript.ObjTable);
            registerBehaviourData.RegisterToLua(BindScript.Env, BindScript.ObjTable);

            BindScript.ObjTable.Set(ITEM_NAME, this);
        }

        protected internal virtual void SetItemData(int index)
        {
            BindScript.CallAction("SetItemData", index);
        }

        protected virtual void OnDestroy()
        {
            if (BindScript.IsValid())
            {
                BindScript.CallAction(LuaConst.DESTROY_FUNCTION_NAME);
                BindScript.Dispose();
            }
        }
    }
}
