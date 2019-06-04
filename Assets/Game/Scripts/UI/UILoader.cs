using System;
using System.Collections.Generic;
using UnityEngine.UI;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class UILoader : UIWindow
    {
        public override string ResourcePath => "UI/UILoader";

        [Inst("StartGame")]
        public Button startBtn;

        public override void OnShow()
        {
            startBtn.onClick.AddListener(start);
        }

        public override void OnHide()
        {
            startBtn.onClick.RemoveAllListeners();
        }

        void start()
        {
            Game.ChangeScene("Main");
            Close();
        }
    }
}
