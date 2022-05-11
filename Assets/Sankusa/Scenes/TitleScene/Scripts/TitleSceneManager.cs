using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SankusaLib;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using SankusaLib.SoundLib;
using System.Linq;
using Zenject;

namespace Sankusa.unity1week202205_2.TitleScene {
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup startButtonCanvasGroup;
        [SerializeField] private UIButton startButton;

        [Inject] private SceneLoader sceneLoader;

        void Start() {
            if(!PlayerPrefs.HasKey("FirstTime")) {
                SoundManager.Instance.BgmVolume = 0.5f;
                SoundManager.Instance.SeVolume = 0.5f;
                PlayerPrefs.SetInt("FirstTime", 0);
            }
            if(SoundManager.Instance.GetComponentsInChildren<AudioSource>().ToList().Find(audio => audio.clip?.name == SoundDataMaster.Instance.FindSoundData(SoundId.BGM1).Clip.name) == null) {
                SoundManager.Instance.CrossFadeBgm(SoundId.BGM1);
            }
            
            startButtonCanvasGroup.alpha = 0;
            this.StartDelayCoroutine(2f, () => startButtonCanvasGroup.DOFade(0.8f, 1).OnComplete(() => startButtonCanvasGroup.enabled = false).SetLink(gameObject));
            startButton.pressedState.stateEvent.Event.AddListener(() => LoadInGameScene());
        }

        private void LoadInGameScene() {
            sceneLoader.LoadScene("InGameScene");
        }
    }
}