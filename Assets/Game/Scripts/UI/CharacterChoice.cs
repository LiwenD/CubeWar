using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using YummyGame.Framework;

namespace YummyGame.CubeWar
{

    public class CharacterChoice : UIWindow
    {
        public override string ResourcePath => "UI/CharacterChoice";

        [Inst("Occupation/OccupationName")]
        public Text occupationName;

        [UIClick("BtnOccuUP")]
        [Inst("Occupation/BtnOccuUP")]
        public Button btnOccuUP;

        [UIClick("BtnOccuDown")]
        [Inst("Occupation/BtnOccuDown")]
        public Button btnDown;

        [UIClick("BtnEnter")]
        [Inst("Occupation/BtnEnter")]
        public Button btnEnter;

        [UIClick("BtnModeUP")]
        [Inst("ChoiceMode/BtnModeUP")]
        public Button btnModeUP;

        [UIClick("BtnModeDown")]
        [Inst("ChoiceMode/BtnModeDown")]
        public Button btnModeDown;


        #region 重载
        public override void OnShow()
        {
            base.OnShow();
            TestLoadTable();
        }
        #endregion

        #region Btn
        void BtnOccuUP()
        {
            Debug.Log("BtnUp");
        }

        void BtnOccuDown()
        {
            Debug.Log("BtnDown");
        }

        void BtnEnter()
        {
            Debug.Log("BtnEnter");
        }

        void BtnModeUP()
        {
            Debug.Log("BtnModeUP");
        }

        void BtnModeDown()
        {
            Debug.Log("BtnModeDown");
        }
        #endregion

        void TestLoadTable()
        {
            DataTable dataTable = Game.LoadTable("data/WeaponTable");
            int id = dataTable.Get<int>(1, "id");
            Debug.Log(id);
        }
    }
}
