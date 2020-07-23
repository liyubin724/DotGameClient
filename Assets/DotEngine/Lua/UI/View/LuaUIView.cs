using DotEngine.Lua.Register;
using XLua;

namespace DotEngine.Lua.UI.View
{
    public class LuaUIView : ComposeBindBehaviour
    {
        protected LuaUIViewController viewController;

        public void SetViewController(LuaUIViewController vc)
        {
            viewController = vc;

            SetValue<LuaTable>("controller", vc.ObjTable);
        }
    }
}
