using DotEngine.Lua.Register;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.Lua.ListView
{
    [RequireComponent(typeof(LoopListView2), typeof(ScrollRect))]
    public class LuaLoopListView : ScriptBindBehaviour
    {
        private const string LISTVIEW_NAME_IN_LUA = "listView";

        public LoopListView2 listView = null;

        protected override void OnInitFinished()
        {
            base.OnInitFinished();

            if (listView == null)
            {
                listView = GetComponent<LoopListView2>();
            }
            if (ObjTable != null)
            {
                ObjTable.Set(LISTVIEW_NAME_IN_LUA, this);
            }
        }

        public void InitListView(int totalCount)
        {

        }



    }
}
