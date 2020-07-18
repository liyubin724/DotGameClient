using DotEngine.Lua.Register;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.UI
{
    [Serializable]
    public class LuaInputFieldData
    {
        public ScriptBindBehaviour bindBehaviour;
        public string valueChangedFuncName;
        public string endEditFuncName;
        
        public void InvokeValueChanged(string text)
        {
            if(bindBehaviour != null && !string.IsNullOrEmpty(valueChangedFuncName))
            {
                bindBehaviour.CallAction<string>(valueChangedFuncName, text);
            }
        }

        public void InvokeEndEdit(string text)
        {
            if(bindBehaviour != null && !string.IsNullOrEmpty(endEditFuncName))
            {
                bindBehaviour.CallAction<string>(endEditFuncName, text);
            }
        }
    }

    public class LuaInputField : InputField
    {
        [SerializeField]
        private LuaInputFieldData fieldData = new LuaInputFieldData();

        public LuaInputField():base()
        {
            onValueChanged.AddListener(OnFieldValueChanged);
            onEndEdit.AddListener(OnFieldEndEdit);
        }

        private void OnFieldValueChanged(string text)
        {
            fieldData.InvokeValueChanged(text);
        }

        private void OnFieldEndEdit(string text)
        {
            fieldData.InvokeEndEdit(text);
        }
    }
}
