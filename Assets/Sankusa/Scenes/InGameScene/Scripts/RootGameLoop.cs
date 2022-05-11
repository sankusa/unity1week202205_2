using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;

namespace Sankusa.unity1week202205_2.InGameScene {
    public class RootGameLoop : IInitializable, IDisposable
    {
        private MainGameLoop mainGameLoop;
        private SceneLoader sceneLoader;

        [Inject]
        public RootGameLoop(MainGameLoop mainGameLoop, SceneLoader sceneLoader) {
            this.mainGameLoop = mainGameLoop;
            this.sceneLoader = sceneLoader;
        }

        public void Initialize() {
            var _ = StartGameLoop();
        }

        public void Dispose() {
            
        }

        public async UniTask StartGameLoop() {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            try {
                ObserveRetry(source, token);
                await mainGameLoop.StartGameLoop(token);
            } catch(OperationCanceledException) {

            }
        }

        private async void ObserveRetry(CancellationTokenSource source, CancellationToken token) {
            try {
                while(true) {
                    token.ThrowIfCancellationRequested();
                    await UniTask.Yield(token);

                    if(Keyboard.current.rKey.wasPressedThisFrame) {
                        source.Cancel();
                        LoadInGameScene();
                    }
                }
            } catch(OperationCanceledException) {
                
            }
        }

        private void LoadInGameScene() {
            sceneLoader.LoadScene("InGameScene");
        }
    }
}