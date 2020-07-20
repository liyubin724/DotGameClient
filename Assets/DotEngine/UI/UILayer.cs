using UnityEngine;

namespace DotEngine.UI
{
    public enum UILayerLevel
    {
        BottomlowestLayer = 0,
        BottomLayer,
        DefaultLayer,
        TopLayer,
        TopmostLayer,
    }

    public class UILayer : MonoBehaviour
    {
        public UILayerLevel layerLevel = UILayerLevel.DefaultLayer;

        private RectTransform m_Transform;

        public RectTransform RectTran
        {
            get
            {
                return m_Transform;
            }
        }

        private void Awake()
        {
            m_Transform = (RectTransform)transform;
        }
    }
}
