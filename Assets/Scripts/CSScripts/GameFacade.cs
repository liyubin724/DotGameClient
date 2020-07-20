using DotEngine.Asset;
using DotEngine.Config;
using DotEngine.Framework;
using DotEngine.GOPool;
using DotEngine.Lua;
using DotEngine.Net.Services;
using DotEngine.Timer;
using DotEngine.UI.View;
using DotEngine.Utilities;
using Game.Commands;

public class GameFacade : FFacade
{
    public new static IFacade GetInstance()
    {
        if(instance == null)
        {
            instance = new GameFacade();
        }
        return instance;
    }

    private GameFacade():base()
    {
    }

    protected override void InitializeFacade()
    {
        base.InitializeFacade();

        SendNotification(CommandNames.STARTUP);
    }

    protected override void InitializeService()
    {
        base.InitializeService();

        IniConfigService iniConfigService = new IniConfigService();
        RegisterService(IniConfigService.NAME, iniConfigService);

        NDBService ndbService = new NDBService();
        RegisterService(NDBService.NAME, ndbService);

        TimerService timerService = new TimerService();
        RegisterService(TimerService.NAME, timerService);

        AssetService assetService = new AssetService();
        RegisterService(AssetService.NAME, assetService);

        GameObjectPoolService gameObjectPoolService = new GameObjectPoolService(assetService.InstantiateAsset);
        RegisterService(GameObjectPoolService.NAME, gameObjectPoolService);

        LuaEnvService luaEnvService = new LuaEnvService();
        RegisterService(LuaEnvService.NAME, luaEnvService);

        ClientNetService clientNetService = new ClientNetService();
        RegisterService(ClientNetService.NAME, clientNetService);

        ServerNetService serverNetService = new ServerNetService();
        RegisterService(ServerNetService.NAME, serverNetService);
    }

    protected override void InitializeController()
    {
        base.InitializeController();

        StartupCommand startupCommand = new StartupCommand();
        RegisterCommand(CommandNames.STARTUP, startupCommand);

        TestLoadLoginPanelCommand loadLoginPanelCommand = new TestLoadLoginPanelCommand();
        RegisterCommand(CommandNames.TEST_LOAD_LOGIN_PANEL, loadLoginPanelCommand);

    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        UIPanelProxy panelProxy = new UIPanelProxy();
        RegisterProxy(UIPanelProxy.NAME, panelProxy);

    }
}
