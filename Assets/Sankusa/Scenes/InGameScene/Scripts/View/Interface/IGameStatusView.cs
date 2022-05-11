using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public interface IGameStatusView
    {
        bool NextButtonPressed {get;}
        void SetNextButtonActive(bool active);
        void SetTimeText(string text);
        void SetSuccessNumText(string text);
        void SetDeadNumText(string text);
    }
}