using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace YummyGame.Framework
{
    public class UITest2 : UIWindow
    {
        public override string ResourcePath => "UITest2";

        [Inst("Btn")]
        public Button okBtn;

        [Inst("Text")]
        public Text text;

        public override void OnHide()
        {
            okBtn.onClick.RemoveAllListeners();
        }

        public override void OnShow()
        {
            text.text = "hello world";
            okBtn.onClick.AddListener(() => Close());
        }
    }
}
