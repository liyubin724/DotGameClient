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

        public void OpenPanel(
            UIPanelController panelController,
            UILayerLevel layerLevel,
            PanelRelationShip relationShip = PanelRelationShip.Append,
            UILayerLevel[] meutexLayerLevels = null)
        {
            if(!panelDataDic.TryGetValue(layerLevel,out var data))
            {
                data = new PanelData();
                panelDataDic.Add(layerLevel, data);
            }

            if(data.Panels.Count>0)
            {
                if(relationShip == PanelRelationShip.LayerMutex)
                {

                }else if(relationShip == PanelRelationShip.MultilayerMutex)
                {

                }else if(relationShip == PanelRelationShip.Navigate)
                {

                }
            }
            data.Panels.Add(panelController);
            Facade.RegisterViewController(panelController.Name, panelController);
        }

        public void ClosePanel(UIPanelController panelController)
        {

        }
       
        class PanelData
        {
            public bool Visible = true;
            public List<UIPanelController> Panels = new List<UIPanelController>();
        }
    }
}
