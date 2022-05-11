using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using UnityEngine.EventSystems;
using Sankusa.unity1week202205_2.InGameScene.Model;
using UniRx;
using System;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public class CitizenHeightControlView : MonoBehaviour
    {
        [SerializeField] private Text heightText;
        [SerializeField] private Text powerText;

        [SerializeField] private UIButton upButton;
        public IObservable<Unit> OnUpButtonPressed => upButton.pressedState.stateEvent.Event.AsObservable();

        [SerializeField] private UIButton downButton;
        public IObservable<Unit> OnDownButtonPressed => downButton.pressedState.stateEvent.Event.AsObservable();

        public void SetHeightText(string text) {
            heightText.text = text;
        }

        public void SetPowerText(string text) {
            powerText.text = text;
        }
    }
}