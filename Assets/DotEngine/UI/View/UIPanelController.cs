using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.UI.View
{
    public class UIPanelController : UIViewController
    {
        public string Name { get; private set; }
        public UIPanelController(string name) : base()
        {
            Name = name;
        }

        protected internal override void OnViewCreated()
        {
            
        }

        protected internal override void OnViewDestroy()
        {
            
        }
    }
}
