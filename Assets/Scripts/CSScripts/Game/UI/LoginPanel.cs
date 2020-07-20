using DotEngine.UI.View;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : UIPanel
{
    
    public void OnCloseBtnClicked()
    {
        GetController<LoginPanelController>().Closed();
    }

    public void OnLoadBtnClicked()
    {
        GetController<LoginPanelController>().LoadLoginPanel();
    }
}
