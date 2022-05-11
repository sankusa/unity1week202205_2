using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace Sankusa.unity1week202205_2.InGameScene.View {
    [ExecuteAlways()]
    public class CitizenView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private float height;
        public float Height {
            get => height;
            set => height = value;
        }
        private float heightOld = 0f;

        [SerializeField] private float imageHeightPerHeight;
        [SerializeField] private bool isMinister = false;
        public bool IsMinister => isMinister;

        private Subject<Unit> onHeightNothing = new Subject<Unit>();
        public IObservable<Unit> OnHeightNothing => onHeightNothing;

        // Update is called once per frame
        void Update()
        {
            if(image != null)
                image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, imageHeightPerHeight * height);

            if(height <= 0 && heightOld > 0) {
                onHeightNothing.OnNext(Unit.Default);
            }
            heightOld = height;
        }
    }
}