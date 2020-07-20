using DotEngine.UI;
using DotEngine.UI.View;
using Game.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class LoginPanelController : UIPanelController
    {
        public const string NAME = "login_panel";
        public LoginPanelController(string name = NAME) : base(name)
        {
        }

        protected internal override void OnViewCreated()
        {
            base.OnViewCreated();
        }

        protected internal override void OnViewDestroy()
        {
            base.OnViewDestroy();
        }

        public void LoadLoginPanel()
        {
            SendNotification(CommandNames.TEST_LOAD_LOGIN_PANEL);
        }
    }

}
