using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading;
using Zenject;
using Cysharp.Threading.Tasks;
using Sankusa.unity1week202205_2.InGameScene.Model;
using Sankusa.unity1week202205_2.InGameScene.View;
using SankusaLib.SoundLib;
using UniRx;
using GreyEngine.Basic;

namespace Sankusa.unity1week202205_2.InGameScene {
    public class MainGameLoop
    {
        private enum GameOverCause {
            None,
            TimeUp,
            MinisterDead
        }

        private ICitizenViewManager citizenViewManager;
        private IGameStatusView gameStatusView;
        private CommandBookReader commandBookReader;
        private IResultView resultView;
        private SceneLoader sceneLoader;

        CompositeDisposable rootCompositeDisposable;

        public bool CommandBookExecuting {get;set;} = false;

        private float timeMax = 120f;
        private float targetHeight = 170f;
        private int enterCitizenNum = 5;

        [Inject]
        public MainGameLoop(ICitizenViewManager citizenViewManager,
                            IGameStatusView gameStatusView,
                            CommandBookReader commandBookReader,
                            IResultView resultView,
                            SceneLoader sceneLoader) {
            this.citizenViewManager = citizenViewManager;
            this.gameStatusView = gameStatusView;
            this.commandBookReader = commandBookReader;
            this.resultView = resultView;
            this.sceneLoader = sceneLoader;
        }

        public async UniTask StartGameLoop(CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();

            CompositeDisposable rootCompositeDisposable = new CompositeDisposable();
            float time = timeMax;
            int successNum = 0;
            int deadNum = 0;

            GameOverCause gameOverCause = GameOverCause.None;

            try {
                // 変数とViewを紐づけ
                this.ObserveEveryValueChanged(_ => time).Subscribe(time => gameStatusView.SetTimeText(time.ToString("0.0"))).AddTo(rootCompositeDisposable);
                this.ObserveEveryValueChanged(_ => successNum).Subscribe(successNum => gameStatusView.SetSuccessNumText("成功 : " + successNum.ToString() + "人")).AddTo(rootCompositeDisposable);
                this.ObserveEveryValueChanged(_ => deadNum).Subscribe(deadNum => gameStatusView.SetDeadNumText(deadNum == 0 ? "" : "死者 : " + deadNum.ToString() + "人")).AddTo(rootCompositeDisposable);

                this.ObserveEveryValueChanged(_ => time).Where(time => time <= 10f).Take(1).Subscribe(_ => SoundManager.Instance.PlaySe(SoundId.SE_StopWatch)).AddTo(rootCompositeDisposable);

                resultView.OnTitleButtonPress.Subscribe(_ => LoadTitleScene());
                
                // ループ(入室～退室)
                while(true) {
                    CompositeDisposable compositeDisposable_EnterToLeave = new CompositeDisposable();
                    rootCompositeDisposable.Add(compositeDisposable_EnterToLeave);

                    gameStatusView.SetNextButtonActive(false);

                    // 国民View生成
                    citizenViewManager.GenerateCitizen(enterCitizenNum);

                    // 国民Viewから国民Modelを生成
                    List<CitizenModel> citizenModels = new List<CitizenModel>();
                    for(int i = 0; i < citizenViewManager.CitizenViews.Count; i++) {
                        CitizenView view = citizenViewManager.CitizenViews[i];
                        CitizenModel model = new CitizenModel(view.IsMinister, view.Height);
                        citizenModels.Add(model);

                        this.ObserveEveryValueChanged(_ => model.Height).Subscribe(height => view.Height = height).AddTo(compositeDisposable_EnterToLeave);

                        CitizenHeightControlView controlView = citizenViewManager.HeightControlViews[i];
                        this.ObserveEveryValueChanged(_ => model.Height).Subscribe(height => controlView.SetHeightText(height.ToString("0.00"))).AddTo(compositeDisposable_EnterToLeave);
                        this.ObserveEveryValueChanged(_ => model.Power).Subscribe(power => controlView.SetPowerText(power.ToString())).AddTo(compositeDisposable_EnterToLeave);
                        controlView.OnUpButtonPressed.Subscribe(_ => model.Power += 1);
                        controlView.OnDownButtonPressed.Subscribe(_ => model.Power -= 1);
                    }

                    // 合計身長が一定になるよう調整
                    float averageHeight = targetHeight;
                    float heightAdjust = (averageHeight * enterCitizenNum - citizenModels.Where(model => !model.IsMinister).Select(model => model.Height).Sum()) / (float)enterCitizenNum;
                    foreach(CitizenModel model in citizenModels) {
                        if(!model.IsMinister) {
                            model.Height += heightAdjust;
                        }
                    }

                    await UniTask.Delay(1000, cancellationToken: cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();

                    // 国民表示
                    citizenViewManager.ShowCitizen();
                    await UniTask.WaitUntil(() => citizenViewManager.IsShowing == false, cancellationToken: cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();

                    citizenViewManager.ActivateHightControlView();

                    // Update
                    while(true) {
                        await UniTask.Yield(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();

                        time -= Time.deltaTime;

                        // 身長増減
                        List<float> heightDeltaList = new List<float>(Enumerable.Repeat(0f, citizenModels.Count));
                        for(int i = 0; i < citizenModels.Count; i++) {
                            heightDeltaList[i] += citizenModels[i].Power;
                            for(int j = 0; j < citizenModels.Count; j++) {
                                if(j != i) heightDeltaList[j] += -(float)citizenModels[i].Power / (float)citizenModels.Count;
                            }
                        }
                        for(int i = 0; i < citizenModels.Count; i++) {
                            citizenModels[i].Height += heightDeltaList[i] * Time.deltaTime;
                        }

                        // 死亡判定
                        for(int i = citizenModels.Count - 1; i >= 0; i--) {
                            if(citizenModels[i].IsDead && !citizenModels[i].IsMinister) {
                                citizenModels.RemoveAt(i);
                                deadNum += 1;
                            }
                        }

                        // 身長判定
                        bool succcess = true;
                        for(int i = 0; i < citizenModels.Count; i++) {
                            if(citizenModels[i].IsMinister) continue;
                            if(citizenModels[i].Height < 170f) {
                                succcess = false;
                                break;
                            }
                        }
                        if(succcess) {
                            gameStatusView.SetNextButtonActive(true);
                        } else {
                            gameStatusView.SetNextButtonActive(false);
                        }

                        // 終了判定
                        if(citizenModels.Find(model => model.IsMinister).IsDead) {
                            successNum += citizenModels.Where(model => model.Height >= targetHeight).Count();
                            gameOverCause = GameOverCause.MinisterDead;
                            break;
                        }

                        if(time <= 0) {
                            successNum += citizenModels.Where(model => !model.IsMinister && model.Height >= targetHeight).Count();
                            gameOverCause = GameOverCause.TimeUp;
                            break;
                        }

                        // 
                        if(gameStatusView.NextButtonPressed || citizenModels.Count == 1) {
                            successNum += citizenModels.Where(model => !model.IsMinister).Count();
                            break;
                        }
                    }

                    if(gameOverCause == GameOverCause.TimeUp) SoundManager.Instance.PlaySe(SoundId.SE_Bell);
                    
                    citizenViewManager.InactivateHightControlView();

                await UniTask.Delay(500, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                    // 退室処理
                    if(gameOverCause != GameOverCause.MinisterDead) {
                        citizenViewManager.HideCitizen();
                        await UniTask.WaitUntil(() => citizenViewManager.IsHiding == false, cancellationToken: cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();

                        compositeDisposable_EnterToLeave.Dispose();

                        citizenViewManager.Clear();
                    }

                    if(gameOverCause != GameOverCause.None) break;
                }

                rootCompositeDisposable.Dispose();

                await UniTask.Delay(1000, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                // 大臣死亡イベント
                if(gameOverCause == GameOverCause.MinisterDead) {
                    commandBookReader.Run("MinisterDead");
                }
                await UniTask.WaitUntil(() => CommandBookExecuting == false, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                // スコア計算
                int successScore = successNum * 1000;
                int deadScore = deadNum * -5000;
                int totalScore = successScore + deadScore;
                await resultView.Show(successNum, successScore, deadNum, deadScore, totalScore, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(totalScore);
            } catch(OperationCanceledException e) {
                rootCompositeDisposable?.Dispose();
                throw e;
            }
        }

        private void LoadTitleScene() {
            rootCompositeDisposable?.Dispose();
            sceneLoader.LoadScene("TitleScene");
        }
    }
}