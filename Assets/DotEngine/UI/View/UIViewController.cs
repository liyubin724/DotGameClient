using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI.View
{
    public abstract class UIViewController : ViewController
    {
        public UIView View { get; set; } = null;
        public UIViewController Parent { get; set; } = null;

        private bool m_Visible = true;
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible != value)
                {
                    m_Visible = value;
                    if (View != null)
                    {
                        View.SetVisible(m_Visible);
                    }
                }
            }
        }

        private AssetHandler m_AssetHandler = null;

        public UIViewController():base()
        {

        }

        public void SetView(UIView view)
        {
            View = view;
            View.SetViewController(this);

            OnViewCreated();

            View.SetVisible(m_Visible);
        }

        public void LoadView(string address)
        {
            if(string.IsNullOrEmpty(address))
            {
                LogUtil.LogError("UI", "UIBaseViewController::LoadView->address is emtpy");
                return;
            }
            if(m_AssetHandler!=null)
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
                UIView view = gObj.GetComponent<UIView>();
                if(view == null)
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
            if(viewController is UIViewController uiVC)
            {
                base.AddSubViewController(name, viewController);
                uiVC.Parent = this;
            }else
            {
                LogUtil.LogError("UI", "UIViewController::AddSubViewController->viewController is not UIViewController");
            }
        }

        public override void RemoveSubViewController(string name)
        {
            base.RemoveSubViewController(name);
        }

        public virtual void AddSubView(string name,UIView view)
        {

        }

        protected internal abstract void OnViewCreated();
        protected internal abstract void OnViewDestroy();
    }
}
