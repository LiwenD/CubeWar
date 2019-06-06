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

        [Inst("StartGame")]
        public Button startBtn;

        [Inst("ContinueGame")]
        public Button continueBtn;

        [Inst("Setting")]
        public Button settingBtn;

        public override void OnShow()
        {
            startBtn.onClick.AddListener(start);
            settingBtn.onClick.AddListener(setting);
            continueBtn.onClick.AddListener(continueGame);
        }

        public override void OnHide()
        {
            startBtn.onClick.RemoveAllListeners();
            settingBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
        }

        void start()
        {
            Game.ChangeScene("Main").Event(()=> { Debug.Log("加载场景完成"); });
            Close();
        }

        void setting()
        {

        }

        void continueGame()
        {

        }
    }
}
