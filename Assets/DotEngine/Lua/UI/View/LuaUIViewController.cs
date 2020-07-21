using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.UI.View;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua.UI.View
{
    public class LuaUIViewController : ViewController
    {
        private const string VIEW_CREATE_FUNC_NAME = "OnViewCreated";

        public LuaUIViewController Parent { get; set; } = null;
        public LuaUIView View { get; set; } = null;

        private LuaBindScript m_BindScript = null;
        private AssetHandler m_AssetHandler = null;

        public LuaUIViewController(string envName,string scriptFilePath) : base()
        {
            m_BindScript = new LuaBindScript(envName, scriptFilePath);
        }

        public void SetView(LuaUIView view)
        {
            View = view;
            View.SetViewController(this);

            OnViewCreated();
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

        public override void OnRegister()
        {
            base.OnRegister();
            m_BindScript.InitLua(OnLuaInitFinish);
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

        public override void AddSubViewController(string name, IViewController viewController)
        {
            if (viewController is LuaUIViewController uiVC)
            {
                base.AddSubViewController(name, viewController);
                uiVC.Parent = this;
            }
            else
            {
                LogUtil.LogError("UI", "UIViewController::AddSubViewController->viewController is not UIViewController");
            }
        }

        public override void RemoveSubViewController(string name)
        {
            base.RemoveSubViewController(name);
        }

        public virtual void AddSubView(string name, UIView view)
        {
            Transform transform = View.GetBindTransform(name);
            if (transform != null)
            {
                view.ViewTransform.SetParent(transform, false);
            }
        }

        protected internal virtual void OnLuaInitFinish()
        {
            m_BindScript.ObjTable.Set("viewController", this);
        }

        private void OnViewCreated()
        {
            if(m_BindScript.IsValid())
            {
                
            }
        }
        protected internal virtual void OnViewDestroy()
        {
            if (View != null)
            {
                GameObject.Destroy(View.ViewGameObject);
            }
        }
    }
}
