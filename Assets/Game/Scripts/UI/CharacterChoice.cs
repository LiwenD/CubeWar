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

        [UIClick("BtnUp")]
        [Inst("BtnUp")]
        public Button btnUp;

        [UIClick("BtnDown")]
        [Inst("BtnDown")]
        public Button btnDown;

        [UIClick("BtnEnter")]
        [Inst("BtnEnter")]
        public Button btnEnter;


        void BtnUp()
        {
            Debug.Log("BtnUp");
        }

        void BtnDown()
        {
            Debug.Log("BtnDown");
        }

        void BtnEnter()
        {
            Debug.Log("BtnEnter");
        }
    }
}
