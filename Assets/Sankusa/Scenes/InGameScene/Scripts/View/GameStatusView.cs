using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using UniRx;
using System;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public class GameStatusView : MonoBehaviour, IGameStatusView
    {
        [SerializeField] private UIButton nextButton;
        [SerializeField] private Text timeText;
        [SerializeField] private Text successNumText;
        [SerializeField] private Text deadNumText;

        void Awake() {
            timeText.text = "";
            successNumText.text = "";
            deadNumText.text = "";
        }

        public bool NextButtonPressed => nextButton.currentUISelectionState == UISelectionState.Pressed;

        public void SetNextButtonActive(bool active) {
            nextButton.gameObject.SetActive(active);
        }

        public void SetTimeText(string text) {
            timeText.text = text;
        }

        public void SetSuccessNumText(string text) {
            successNumText.text = text;
        }

        public void SetDeadNumText(string text) {
            deadNumText.text = text;
        }
    }
}