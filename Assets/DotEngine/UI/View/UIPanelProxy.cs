using DotEngine.Framework;
using System.Collections.Generic;

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
        //导航的形式显示
        Navigate,
    }

    class PanelData
    {
        public UILayerLevel LayerLevel = UILayerLevel.DefaultLayer;
        public PanelRelationShip RelationShip = PanelRelationShip.Append;
        public UIPanelController PanelController;
    }

    public class UIPanelProxy : Proxy
    {
        public const string NAME = "panelProxy";

        private Dictionary<UILayerLevel, List<PanelData>> panelInLayerDic = new Dictionary<UILayerLevel, List<PanelData>>();
        public UIPanelProxy()
        {
        }

        public void SetLayerVisible(UILayerLevel layerLevel,bool visible)
        {
            UIRoot.Root.GetLayer(layerLevel).Visible = visible;
        }

        public void RemoveAllPanelInLayer(UILayerLevel layerLevel)
        {
            if(panelInLayerDic.TryGetValue(layerLevel,out var panelList) && panelList.Count>0)
            {
                for(int i =panelList.Count-1;i>=0;--i)
                {
                    FFacade.GetInstance().RemoveViewController(panelList[i].PanelController.Name);
                }
                panelList.Clear();
            }
        }

        public void OpenPanel(
            UILayerLevel layerLevel,
            UIPanelController panelController,
            PanelRelationShip relationShip)
        {
            PanelData panelData = new PanelData()
            {
                LayerLevel = layerLevel,
                RelationShip = relationShip,
                PanelController = panelController,
            };
            panelController.LayerLevel = layerLevel;

            if(!panelInLayerDic.TryGetValue(layerLevel,out var panelList))
            {
                panelList = new List<PanelData>();
                panelInLayerDic.Add(layerLevel, panelList);
            }

            if(panelList.Count>0)
            {
                if(relationShip == PanelRelationShip.LayerMutex)
                {
                    RemoveAllPanelInLayer(layerLevel);
                }else if(relationShip == PanelRelationShip.Navigate)
                {
                    for (int i = panelList.Count - 1; i >= 0; --i)
                    {
                        PanelData prePanelData = panelList[i];
                        if(panelData.RelationShip == PanelRelationShip.Navigate || panelData.RelationShip == PanelRelationShip.LayerMutex)
                        {
                            prePanelData.PanelController.Visible = false;
                            break;
                        }else if(panelData.RelationShip == PanelRelationShip.Append)
                        {
                            prePanelData.PanelController.Visible = false;
                        }
                    }
                }
            }
            panelList.Add(panelData);
            FFacade.GetInstance().RegisterViewController(panelController.Name, panelController);
        }

        public void ClosePanel(UIPanelController panelController)
        {
            if (panelInLayerDic.TryGetValue(panelController.LayerLevel, out var panelList))
            {
                PanelData panelData = null;
                for (int i = panelList.Count - 1; i >= 0; --i)
                {
                    PanelData pData = panelList[i];
                    if(panelData == null)
                    {
                        if (pData.PanelController != panelController)
                        {
                            FFacade.GetInstance().RemoveViewController(pData.PanelController.Name);
                            panelList.RemoveAt(i);
                        }
                        else
                        {
                            panelData = pData;
                            panelList.RemoveAt(i);
                        }
                    }else
                    {
                        if(panelData.RelationShip == PanelRelationShip.Navigate)
                        {
                            pData.PanelController.Visible = true;
                        }
                        break;
                    }
                }
                if(panelData!=null)
                {
                    FFacade.GetInstance().RemoveViewController(panelData.PanelController.Name);
                }
            }
        }
    }
}
