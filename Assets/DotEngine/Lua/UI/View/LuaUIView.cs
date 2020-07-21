using DotEngine.Lua.Register;
using DotEngine.UI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.Lua.UI.View
{
    public class LuaUIView : ComposeBindBehaviour
    {
        public RectTransform ViewTransform { get; private set; }
        public GameObject ViewGameObject { get; private set; }

        protected LuaUIViewController viewController;
        protected override void Awake()
        {
            ViewTransform = (RectTransform)transform;
            ViewGameObject = gameObject;
            base.Awake();
        }

        public void SetViewController(LuaUIViewController vc)
        {
            viewController = vc;
        }

        public void SetVisible(bool visible)
        {
            ViewGameObject.SetActive(visible);
        }

        public virtual Transform GetBindTransform(string name)
        {
            return ViewTransform;
        }
    }
}
