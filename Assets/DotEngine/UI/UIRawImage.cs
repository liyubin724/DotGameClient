//using UnityEngine;
//using UnityEngine.UI;
//using UnityObject = UnityEngine.Object;

//namespace DotEngine.UI
//{
//    public class UIRawImage : RawImage
//    {
//        [SerializeField]
//        private string m_RawImagePath;
//        public string RawImagePath
//        {
//            get
//            {
//                return m_RawImagePath;
//            }

//            set
//            {
//                if (m_RawImagePath != value)
//                {
//                    if (loadingIndex >= 0)
//                    {
//                        GameAsset.CancelLoad(loadingIndex);
//                        loadingIndex = -1;
//                    }

//                    m_RawImagePath = value;
//                    if (string.IsNullOrEmpty(m_RawImagePath))
//                    {
//                        texture = null;
//                    }
//                    else
//                    {
//                        LoadRawImage();
//                    }
//                }
//            }
//        }

//        private int loadingIndex = -1;
//        private void LoadRawImage()
//        {
//            if (!string.IsNullOrEmpty(m_RawImagePath))
//            {
//                loadingIndex = GameAsset.LoadAssetAsync(m_RawImagePath, OnLoadRawImageFinish);
//            }
//        }

//        private void OnLoadRawImageFinish(int assetIndex, string assetPath, UnityObject obj)
//        {
//            loadingIndex = -1;
//            if (obj != null)
//            {
//                texture = (obj as Texture2D);
//            }
//        }

//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//            if (loadingIndex >= 0)
//            {
//                GameAsset.CancelLoad(loadingIndex);
//                loadingIndex = -1;
//            }
//        }
//    }
//}