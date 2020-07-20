using UnityEngine;

namespace DotEngine.UI.View
{
    public class UIView : MonoBehaviour
    {
        public RectTransform ViewTransform { get; private set; }
        public GameObject ViewGameObject { get; private set; }
        
        protected UIViewController viewController;

        protected virtual void Awake()
        {
            ViewTransform = (RectTransform)transform;
            ViewGameObject = gameObject;
        }

        public void SetViewController(UIViewController vc)
        {
            viewController = vc;
        }

        public void SetVisible(bool visible)
        {
            ViewGameObject.SetActive(visible);
        }
    }
}
