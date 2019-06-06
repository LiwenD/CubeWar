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

<<<<<<< HEAD
        [Inst("ContinueGame")]
        public Button continueBtn;

        [Inst("Setting")]
        public Button settingBtn;
=======
        [AutoClose]
        [Inst("Close")]
        public Button closeBtn;
>>>>>>> a5dc2f93d37d2b3acf3189f53e9a201c4d880212

        public override void OnShow()
        {
            startBtn.onClick.AddListener(start);
<<<<<<< HEAD
            settingBtn.onClick.AddListener(setting);
            continueBtn.onClick.AddListener(continueGame);
=======
            loadTable();
>>>>>>> a5dc2f93d37d2b3acf3189f53e9a201c4d880212
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

<<<<<<< HEAD
        void setting()
        {

        }

        void continueGame()
        {

=======
        void loadTable()
        {
            DataTable table = Game.LoadTable("data/TestTable");
            int attack = table.Get<int>(2, "attack");
            Debug.Log(attack);
>>>>>>> a5dc2f93d37d2b3acf3189f53e9a201c4d880212
        }
    }
}
