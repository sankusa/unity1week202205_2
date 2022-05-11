using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SankusaLib.SoundLib;
using SankusaLib;
using UnityEngine.SceneManagement;

namespace Sankusa.unity1week202205_2 {
    public class SceneLoader
    {
        public void LoadScene(string sceneName) {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(
                DOTween.To(() => SoundManager.Instance.BgmSubVolume, (value) => SoundManager.Instance.BgmSubVolume = value, 0f, 1f)
            );
            sequence.Append(
                DOTween.To(() => SoundManager.Instance.BgmSubVolume, (value) => SoundManager.Instance.BgmSubVolume = value, 1f, 1f)
            );
            Blackout.Instance.PlayBlackout(() => SceneManager.LoadScene(sceneName));
        }
    }
}