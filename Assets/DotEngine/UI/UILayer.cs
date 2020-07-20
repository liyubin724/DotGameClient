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
        public RectTransform layerTransform = null;

        private void Awake()
        {
            if(layerTransform == null)
            {
                layerTransform = (RectTransform)transform;
            }
        }
    }
}
