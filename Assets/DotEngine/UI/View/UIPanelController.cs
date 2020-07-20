using DotEngine.Framework;

namespace DotEngine.UI.View
{
    public class UIPanelController : UIViewController
    {
        public string Name { get; private set; }
        public UILayerLevel LayerLevel { get; set; } = UILayerLevel.DefaultLayer;

        public T GetPanel<T>() where T:UIPanel
        {
            return (T)View;
        }

        public UIPanelController(string name) : base()
        {
            Name = name;
        }

        public virtual void Closed()
        {
            UIPanelProxy panelProxy = FFacade.GetInstance().RetrieveProxy<UIPanelProxy>(UIPanelProxy.NAME);
            panelProxy.ClosePanel(this);
        }

        protected internal override void OnViewCreated()
        {
            base.OnViewCreated();

            var tran = UIRoot.Root.GetLayer(LayerLevel).transform;
            View.ViewTransform.SetParent(tran, false);
        }
        
        protected internal override void OnViewDestroy()
        {
            base.OnViewDestroy();
        }
    }
}
