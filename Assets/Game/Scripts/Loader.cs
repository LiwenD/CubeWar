using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YummyGame.Framework;

namespace YummyGame.CubeWar
{
    public class Loader : MonoBehaviour
    {
        private void Awake()
        {
            Game.Initalize(initAssetComplete);
        }

        private void initAssetComplete()
        {
            Game.OpenUI<UILoader>("Background");
        }
    }
}

