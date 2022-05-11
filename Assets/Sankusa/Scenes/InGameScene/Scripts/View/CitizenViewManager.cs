using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using System.Linq;
using UniRx;
using SankusaLib.SoundLib;
using SankusaLib;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    public class CitizenViewManager : MonoBehaviour, ICitizenViewManager
    {
        [SerializeField] private CitizenView ministerView;
        [SerializeField] private List<GameObject> citizenViewPrefabs;
        [SerializeField] private GameObject heightControlViewPrefab;

        [SerializeField] private RectTransform citizenGenerateArea;
        [SerializeField] private UIView citizenGenerateAreaUiView;

        private List<CitizenView> citizenViews = new List<CitizenView>();
        public IReadOnlyList<CitizenView> CitizenViews => citizenViews.AsReadOnly();

        private List<CitizenHeightControlView> heightControlViews = new List<CitizenHeightControlView>();
        public IReadOnlyList<CitizenHeightControlView> HeightControlViews => heightControlViews;

        [SerializeField] private float callAndReturnDuration;

        public bool IsShowing => citizenGenerateAreaUiView.isShowing;
        public bool IsHiding => citizenGenerateAreaUiView.isHiding;

        [SerializeField] private GameObject shockWaveParticlePrefab;

        void Awake() {
            // 大臣
            citizenViews.Add(ministerView);
        }

        public void GenerateCitizen(int citizenNumber) {
            float generateSpace = citizenGenerateArea.sizeDelta.x / (citizenNumber + 1);
            for(int i = 0; i < citizenNumber; i++) {
                // 国民生成
                GameObject prefab = citizenViewPrefabs[Random.Range(0, citizenViewPrefabs.Count)];
                GameObject instance = Instantiate(prefab, citizenGenerateArea);
                instance.transform.localPosition = new Vector2(-citizenGenerateArea.rect.width / 2 + (i + 1) * generateSpace, 0);
                
                citizenViews.Add(instance.GetComponent<CitizenView>());
            }
            for(int i = 0; i < citizenViews.Count; i++) {
                CitizenView citizenView = citizenViews[i];

                // 死亡時エフェクト
                citizenView.OnHeightNothing.Subscribe(_ => Instantiate(shockWaveParticlePrefab, citizenView.transform.position, Quaternion.identity));
                
                // ボタン生成
                GameObject heightControlViewInstance = Instantiate(heightControlViewPrefab, citizenViews[i].transform);
                heightControlViewInstance.transform.localPosition = Vector2.zero;

                heightControlViews.Add(heightControlViewInstance.GetComponent<CitizenHeightControlView>());
                heightControlViewInstance.SetActive(false);
            }
        }

        public void ActivateHightControlView() {
            foreach(var view in heightControlViews) {
                view.gameObject.SetActive(true);
            }
        }

        public void InactivateHightControlView() {
            foreach(var view in heightControlViews) {
                view.gameObject.SetActive(false);
            }
        }

        public void Clear() {
            for(int i = heightControlViews.Count - 1; i >= 0; i--) {
                CitizenHeightControlView heightControlView = heightControlViews[i];
                heightControlViews.RemoveAt(i);
                Destroy(heightControlView.gameObject);
            }
            for(int i = citizenViews.Count - 1; i >= 0; i--) {
                CitizenView citizen = citizenViews[i];
                if(citizen.IsMinister) continue;
                citizenViews.RemoveAt(i);
                Destroy(citizen.gameObject);
            }
        }

        public void ShowCitizen() {
            citizenGenerateAreaUiView.Show();
            SoundManager.Instance.PlaySe(SoundId.SE_Walk);
            this.StartDelayCoroutine(0.05f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.1f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.15f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.2f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
        }

        public void HideCitizen() {
            citizenGenerateAreaUiView.Hide();
            SoundManager.Instance.PlaySe(SoundId.SE_Walk);
            this.StartDelayCoroutine(0.05f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.1f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.15f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
            this.StartDelayCoroutine(0.2f, () => SoundManager.Instance.PlaySe(SoundId.SE_Walk));
        }
    }
}