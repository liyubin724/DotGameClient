using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Lua.UI.View;
using DotEngine.UI;
using DotEngine.UI.View;
using Game.UI;
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

                LuaUIViewController controller = new LuaUIViewController("game", "Test/UI/View/TestViewController");
                FFacade.GetInstance().RegisterViewController("loginViewC", controller);
                controller.LoadView("login_panel");

                //UIPanelProxy panelProxy = FFacade.GetInstance().RetrieveProxy<UIPanelProxy>(UIPanelProxy.NAME);
                //LoginPanelController loginPanelController = new LoginPanelController();
                //loginPanelController.LoadView("login_panel");
                
                //panelProxy.OpenPanel(UILayerLevel.TopLayer, loginPanelController, PanelRelationShip.Append);
            }
            else
            {
                LogUtil.LogError("Command", "StarupCommand::OnAssetServiceInitFinished->assetService initied failed");
            }
        }
    }
}
