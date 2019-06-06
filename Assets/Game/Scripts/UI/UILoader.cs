﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class UILoader : UIWindow
    {
        public override string ResourcePath => "UI/UILoader";

        [Inst("StartGame")]
        public Button startBtn;

        [AutoClose]
        [Inst("Close")]
        public Button closeBtn;

        public override void OnShow()
        {
            startBtn.onClick.AddListener(start);
            loadTable();
        }

        public override void OnHide()
        {
            startBtn.onClick.RemoveAllListeners();
        }

        void start()
        {
            Game.ChangeScene("Main").Event(()=> { Debug.Log("加载场景完成"); });
            Close();
        }

        void loadTable()
        {
            DataTable table = Game.LoadTable("data/TestTable");
            int attack = table.Get<int>(2, "attack");
            Debug.Log(attack);
        }
    }
}
