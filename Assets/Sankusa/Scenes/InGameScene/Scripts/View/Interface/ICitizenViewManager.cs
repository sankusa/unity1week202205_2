using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public interface ICitizenViewManager
    {
        IReadOnlyList<CitizenView> CitizenViews {get;}
        IReadOnlyList<CitizenHeightControlView> HeightControlViews {get;}
        bool IsShowing{get;}
        bool IsHiding{get;}
        void GenerateCitizen(int citizenNumber);
        void ActivateHightControlView();
        void InactivateHightControlView();
        void Clear();
        void ShowCitizen();
        void HideCitizen();
    }
}