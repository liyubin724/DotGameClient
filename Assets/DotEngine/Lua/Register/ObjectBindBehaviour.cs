namespace DotEngine.Lua.Register
{
    public class ObjectBindBehaviour : ScriptBindBehaviour
    {
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            registerObjectData.RegisterToLua(luaEnv, ObjTable);
        }
    }
}
