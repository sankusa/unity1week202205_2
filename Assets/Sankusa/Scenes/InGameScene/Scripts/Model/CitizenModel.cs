using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202205_2.InGameScene.Model {
    public class CitizenModel
    {

        private bool isMinister;
        public bool IsMinister {
            get => isMinister;
            set {
                isMinister = value;
            }
        }

        private float height;
        public float Height {
            get => height;
            set {
                height = Mathf.Max(value, 0);
                if(height == 0) isDead = true; 
            }
        }

        private int power = 0;
        public int Power {
            get => power;
            set => power = value;
        }

        private bool isDead = false;
        public bool IsDead => isDead;

        public CitizenModel(bool isMinister, float height) {
            this.isMinister = isMinister;
            this.height = height;
        }
    }
}