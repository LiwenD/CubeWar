using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YummyGame.Framework
{
    public class UITest : UIWindow
    {
        public override string ResourcePath => "TestPanel";

        [Inst("Button")]
        public Button okBtn;

        public override void OnShow()
        {
            okBtn.onClick.AddListener(() =>
            {
                Open<UITest2>("Pop");
            });
        }
    }
}
