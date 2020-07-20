using DotEngine.Asset;
using DotEngine.Framework;
using DotEngine.Log;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI.View
{
    public abstract class UIBaseViewController : ComplexViewController
    {
        private string m_Name;
        public string Name { get => m_Name; }

        private bool m_Visible = true;
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if(m_Visible!=value)
                {
                    m_Visible = value;
                    if(View != null)
                    {
                        View.SetVisible(m_Visible);
                    }
                }
            }
        }

        public UIBaseViewController Parent { get; set; }

        protected UIBaseView View;

        private string m_ViewAddress = string.Empty;
        private AssetHandler m_AssetHandler = null;

        protected UIBaseViewController(string name, string viewAddress = null)
        {
            m_Name = name;
            m_ViewAddress = viewAddress;
        }

        public void SetView(UIBaseView view)
        {
            View = view;
            View.SetViewController(this);

            OnViewCreated();

            View.SetVisible(m_Visible);
        }

        public void LoadView()
        {
            if(string.IsNullOrEmpty(m_ViewAddress))
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
            m_AssetHandler = assetService.InstanceAssetAsync(m_ViewAddress, OnViewLoadComplete);
        }

        private void OnViewLoadComplete(string address, UnityObject uObj, SystemObject userData)
        {
            m_AssetHandler = null;

            if (uObj != null && uObj is GameObject gObj)
            {
                UIBaseView view = gObj.GetComponent<UIBaseView>();
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

        protected internal abstract void OnViewCreated();
        protected internal abstract void OnViewDestroy();

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
    }
}
