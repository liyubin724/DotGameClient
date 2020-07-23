using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.UI;
using UnityEngine;
using XLua;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua.UI.View
{
    public class LuaUIViewController : ViewController
    {
        private const string VIEW_CREATE_FUNC_NAME = "OnViewCreate";
        private const string VIEW_DESTROY_FUNC_NAME = "OnViewDestroy";
        private const string ADD_SUB_CONTROLLER_FUNC_NAME = "OnAddSubController";
        private const string REMOVE_SUB_CONTROLLER_FUNC_NAME = "OnRemoveSubController";

        public LuaUIViewController Parent { get; set; } = null;
        public LuaUIView View { get; set; } = null;

        private LuaBindScript m_BindScript = null;
        private AssetHandler m_AssetHandler = null;

        public LuaTable ObjTable
        {
            get
            {
                return m_BindScript?.ObjTable;
            }
        }

        public LuaUIViewController(string envName,string scriptFilePath) : base()
        {
            m_BindScript = new LuaBindScript(envName, scriptFilePath);
        }

        public override void OnRegister()
        {
            base.OnRegister();
            m_BindScript.InitLua(OnLuaInitFinish);
        }

        protected internal virtual void OnLuaInitFinish()
        {
            m_BindScript.SetValue<LuaUIViewController>("csController", this);
        }

        public void LoadView(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                LogUtil.LogError("UI", "UIBaseViewController::LoadView->address is emtpy");
                return;
            }
            if (m_AssetHandler != null)
            {
                LogUtil.LogWarning("UI", "UIBaseViewController::LoadView->asset is loading");
                return;
            }

            AssetService assetService = Facade.RetrieveService<AssetService>(AssetService.NAME);
            m_AssetHandler = assetService.InstanceAssetAsync(address, OnViewLoadComplete);
        }

        private void OnViewLoadComplete(string address, UnityObject uObj, SystemObject userData)
        {
            m_AssetHandler = null;

            if (uObj != null && uObj is GameObject gObj)
            {
                LuaUIView view = gObj.GetComponent<LuaUIView>();
                if (view == null)
                {
                    GameObject.Destroy(gObj);
                    LogUtil.LogError("UI", "UIBaseViewController::OnViewLoadComplete->the component can't found.address = " + address);
                }
                else
                {
                    SetView(view);
                }
            }
            else
            {
                LogUtil.LogError("UIBaseViewController", "load panel failed.address = " + address);
            }
        }

        public void SetView(LuaUIView view)
        {
            View = view;
            View.SetViewController(this);
            if(Parent!=null)
            {
                m_BindScript.SetValue<LuaTable>("parent", Parent.ObjTable);
            }

            OnViewCreated();

            view.transform.SetParent(UIRoot.Root.layers[0].LayerTransform, false);
        }

        protected virtual void OnViewCreated()
        {
            m_BindScript.SetValue<LuaTable>("view", View.ObjTable);
            m_BindScript.CallAction(VIEW_CREATE_FUNC_NAME);
        }


        public override void AddSubViewController(string name, IViewController viewController)
        {
            if (viewController is LuaUIViewController uiVC)
            {
                uiVC.Parent = this;
                base.AddSubViewController(name, viewController);

                m_BindScript.CallAction<string, LuaTable>(ADD_SUB_CONTROLLER_FUNC_NAME, name, uiVC.ObjTable);
            }
            else
            {
                LogUtil.LogError("UI", "UIViewController::AddSubViewController->viewController is not UIViewController");
            }
        }

        public override void RemoveSubViewController(string name)
        {
            IViewController viewController = subControllerDic[name];
            if(viewController is LuaUIViewController uiVC)
            {
                uiVC.Parent = null;
                m_BindScript.CallAction<string>(REMOVE_SUB_CONTROLLER_FUNC_NAME, name);
            }
            base.RemoveSubViewController(name);
        }


        public override void OnRemove()
        {
            if (m_AssetHandler != null)
            {
                AssetService assetService = Facade.RetrieveService<AssetService>(AssetService.NAME);
                assetService.UnloadAssetAsync(m_AssetHandler);
                m_AssetHandler = null;
            }

            base.OnRemove();
            OnViewDestroy();

            View = null;
        }

        protected internal virtual void OnViewDestroy()
        {
            m_BindScript.CallAction(VIEW_DESTROY_FUNC_NAME);
        }
    }
}
