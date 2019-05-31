using System;
using System.Collections.Generic;


namespace YummyGame.Framework
{
    public interface ILooper
    {
        void onLooperStart();
        bool onLooperUpdate();
        void onLooperEnd();
    }
}
