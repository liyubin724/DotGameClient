using DotEngine.Framework;
using DotEngine.Lua;
using UnityEngine;

namespace Game.Commands
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            LuaEnvService luaEnvService = GameFacade.GetInstance().RetrieveService<LuaEnvService>(LuaEnvService.NAME);

            luaEnvService.CreateEnv("game", 
                new string[] { LuaConst.GetScriptPathFormat() }, 
                new string[] { "DotLua/Startup"},
                "Game/GameEnvManager");
        }
    }
}
