using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.Lua;
using UnityEngine;

namespace Game.Commands
{
    public class StartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            AssetService assetService = GameFacade.GetInstance().RetrieveService<AssetService>(AssetService.NAME);
            assetService.InitDatabaseLoader(OnAssetServiceInitFinished);
        }

        private void OnAssetServiceInitFinished(bool result)
        {
            if(result)
            {
                LuaEnvService luaEnvService = GameFacade.GetInstance().RetrieveService<LuaEnvService>(LuaEnvService.NAME);

                luaEnvService.CreateEnv("game",
                    new string[] { LuaConst.GetScriptPathFormat() },
                    new string[] { "DotLua/Startup" },
                    "Game/GameEnvManager");
            }else
            {
                LogUtil.LogError("Command", "StarupCommand::OnAssetServiceInitFinished->assetService initied failed");
            }
        }
    }
}
