using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;
using SankusaLib;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UniRx;
using System;
using SankusaLib.SoundLib;
using System.Threading;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public class ResultView : MonoBehaviour, IResultView
    {
        [SerializeField] private Image background;
        [SerializeField] private Text successScoreText;
        [SerializeField] private Text deadScoreText;
        [SerializeField] private Text totalScoreText;
        [SerializeField] private UIButton titleButton;
        public IObservable<Unit> OnTitleButtonPress => titleButton.pressedState.stateEvent.Event.AsObservable();

        public async UniTask Show(int successNum, int successScore, int deadNum, int deadScore, int totalScore, CancellationToken cancellationToken = default) {
            cancellationToken.ThrowIfCancellationRequested();

            background.gameObject.SetActive(true);

            await UniTask.Delay(500, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            successScoreText.text = "成功 " + successNum + "人 : " + successScore + "点";
            successScoreText.gameObject.SetActive(true);

            await UniTask.Delay(500, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            if(deadNum != 0) {
                deadScoreText.text = "死者 " + deadNum + "人 : " + deadScore + "点";
                deadScoreText.gameObject.SetActive(true);

                await UniTask.Delay(500, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            totalScoreText.text = "合計得点 : " + totalScore + "点";
            totalScoreText.gameObject.SetActive(true);

            await UniTask.Delay(500, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            titleButton.gameObject.SetActive(true);

            await UniTask.Delay(500, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}