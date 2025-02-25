using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game {
    public class LightManager : MonoBehaviour {
        public static LightManager Instance;
        private static readonly int LightPosId = Shader.PropertyToID("_LightPos");
        private static readonly int LightRadiusId = Shader.PropertyToID("_LightRadius");
        private RevealLightOrb[] _lightOrbs;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            _lightOrbs = FindObjectsByType<RevealLightOrb>(FindObjectsSortMode.None);
        }

        private void Update() {
            if (_lightOrbs.Length < 0) return;
            Shader.SetGlobalFloat(LightRadiusId, (float)_lightOrbs[0].LightPixelRadius/16); // TODO: Change to int and get rid of constant
            Shader.SetGlobalVector(LightPosId, _lightOrbs[0].transform.position);
        }
        
    }
}
