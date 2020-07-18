using DotEngine.Lua.Register;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace DotEngine.Lua.UI
{
    [Serializable]
    public class LuaButtonData
    {
        public ScriptBindBehaviour bindBehaviour;
        public string funcName;

        public void InvokeClicked()
        {
            if(bindBehaviour != null && !string.IsNullOrEmpty(funcName))
            {
                bindBehaviour.CallAction(funcName);
            }
        }
    }

    public class LuaButton : Button
    {
        [SerializeField]
        private LuaButtonData m_ButtonData = new LuaButtonData();

        public LuaButton():base()
        {
            onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            m_ButtonData?.InvokeClicked();
        }
    }
}
