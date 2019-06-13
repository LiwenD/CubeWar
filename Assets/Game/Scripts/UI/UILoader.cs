using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class UILoader : UIWindow
    {
        public override string ResourcePath => "UI/UILoader";

        [UIClick("start")]
        [Inst("StartGame")]
        public Button startBtn;

        [UIClick("continueGame")]
        [Inst("ContinueGame")]
        public Button continueBtn;

        [UIClick("setting")]
        [Inst("Setting")]
        public Button settingBtn;

        [AutoClose]
        [Inst("Close")]
        public Button closeBtn;

        public override void OnShow()
        {
            //startBtn.onClick.AddListener(start);
            //settingBtn.onClick.AddListener(setting);
            //continueBtn.onClick.AddListener(continueGame);

            loadTable();
        }

        public override void OnHide()
        {
            //startBtn.onClick.RemoveAllListeners();
            //settingBtn.onClick.RemoveAllListeners();
            //continueBtn.onClick.RemoveAllListeners();
        }

        void start()
        {
            Game.ChangeScene("Main").Event(()=> { Debug.Log("加载场景完成"); });
            Close();
        }

        void setting()
        {
            Debug.Log("setting");
        }

        void continueGame()
        {
            Debug.Log("continueGame");
        }

        void loadTable()
        {
            DataTable table = Game.LoadTable("data/TestTable");
            int attack = table.Get<int>(2, "attack");
            Debug.Log(attack);
        }

    }
}
