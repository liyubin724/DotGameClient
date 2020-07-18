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

        public bool IsValid()
        {
            return bindBehaviour != null && !string.IsNullOrEmpty(funcName);
        }

        public void Invoke()
        {
            bindBehaviour.CallAction(funcName);
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
            if(m_ButtonData!=null && m_ButtonData.IsValid())
            {
                m_ButtonData.Invoke();
            }
        }
    }
}
