using DotEngine.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObject = UnityEngine.Object;
using SystemObject = System.Object;

namespace DotEngine.UI.View
{
    /// <summary>
    /// Panel之间的依赖关系
    /// </summary>
    public enum PanelRelationShip
    {
        //追加，直接在指定的层上追加显示
        Append = 0,
        //同层互斥
        LayerMutex,
        //指定多层互斥
        MultilayerMutex,
        //导航的形式显示
        Navigate,
    }

    /// <summary>
    /// panel依赖关系处理的时机
    /// </summary>
    public enum PanelShowOccasion
    {
        //立即执行
        Immediately = 0,
        //等待显示创建后再执行
        ViewCreated,
    }

    public class UIPanelProxy : Proxy
    {
        public const string NAME = "panelProxy";

        private Dictionary<UILayerLevel, PanelData> panelDataDic = new Dictionary<UILayerLevel, PanelData>();
        public UIPanelProxy()
        {
        }

        public void SetLayerVisible(UILayerLevel layerLevel,bool visible)
        {
            
        }

        public void OpenPanel()
        {

        }
        
        public void ClosePanel()
        {

        }

        class PanelData
        {

        }

    }
}
