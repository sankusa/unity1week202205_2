using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public interface IResultView
    {
        IObservable<Unit> OnTitleButtonPress{get;}
        UniTask Show(int successNum, int successScore, int deadNum, int deadScore, int totalScore, CancellationToken cancellationToken);
    }
}