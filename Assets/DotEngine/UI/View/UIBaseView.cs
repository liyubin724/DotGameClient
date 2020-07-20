using UnityEngine;

namespace DotEngine.UI.View
{
    public abstract class UIBaseView : MonoBehaviour
    {
        public RectTransform RectTran { get; private set; }
        protected UIBaseViewController viewController;

        protected virtual void Awake()
        {
            RectTran = (RectTransform)transform;
        }

        public void SetViewController(UIBaseViewController vc)
        {
            viewController = vc;
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
