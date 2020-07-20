using UnityEngine;

namespace DotEngine.UI.View
{
    public abstract class UIView : MonoBehaviour
    {
        public RectTransform Transfrom { get; private set; }

        protected UIViewController viewController;

        protected virtual void Awake()
        {
            Transfrom = (RectTransform)transform;
        }

        public void SetViewController(UIViewController vc)
        {
            viewController = vc;
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
