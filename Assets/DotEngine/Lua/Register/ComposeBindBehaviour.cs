namespace DotEngine.Lua.Register
{
    public class ComposeBindBehaviour : ScriptBindBehaviour
    {
        public RegisterBehaviourData registerBehaviourData = new RegisterBehaviourData();
        public RegisterObjectData registerObjectData = new RegisterObjectData();

        protected override void OnInitFinished()
        {
            registerObjectData.RegisterToLua(luaEnv, ObjTable);
            registerBehaviourData.RegisterToLua(luaEnv, ObjTable);
        }
    }
}
