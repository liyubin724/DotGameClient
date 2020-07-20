using DotEngine.Framework;
using DotEngine.UI;
using DotEngine.UI.View;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Commands
{
    public class TestLoadLoginPanelCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            UIPanelProxy panelProxy = FFacade.GetInstance().RetrieveProxy<UIPanelProxy>(UIPanelProxy.NAME);
            LoginPanelController loginPanelController2 = new LoginPanelController("test");
            loginPanelController2.LoadView("login_panel");
            panelProxy.OpenPanel(UILayerLevel.TopLayer, loginPanelController2, PanelRelationShip.LayerMutex);
        }
    }
}
