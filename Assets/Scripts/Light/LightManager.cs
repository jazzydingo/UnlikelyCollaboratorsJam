using UnityEngine;

namespace game {
    public class LightManager : MonoBehaviour {
        public static LightManager Instance;
        private static readonly int RevealLightsId = Shader.PropertyToID("_RevealLights");
        private static readonly int RevealLightCountId = Shader.PropertyToID("_RevealLightCount");
        [SerializeField] private Material _revealMaterial;
        private RevealLight[] _revealLights;
        private ComputeBuffer _revealLightsBuffer;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            _revealLights = FindObjectsByType<RevealLight>(FindObjectsSortMode.None);
            int elementSizeInBytes = 4 * 4; // float4; four floats; each float is 4 bytes

            _revealLightsBuffer = new ComputeBuffer(_revealLights.Length, elementSizeInBytes);
            _revealMaterial.SetInt(RevealLightCountId, _revealLights.Length);
        }

        private void Update() {
            Vector4[] spheres = new Vector4[_revealLights.Length];

            for (int i = 0; i < _revealLights.Length; i++) {
                RevealLight light = _revealLights[i];
                Vector4 sphere = new(
                    light.transform.position.x, 
                    light.transform.position.y,
                    light.transform.position.z,
                    light.LightPixelRadius
                );
                spheres[i] = sphere;
            }

            _revealLightsBuffer.SetData(spheres);
            _revealMaterial.SetBuffer(RevealLightsId, _revealLightsBuffer);
            // if (_lightOrbs.Length < 0) return;
            // Shader.SetGlobalFloat(LightRadiusId, _lightOrbs[0].LightPixelRadius / 16f);
            // Shader.SetGlobalVector(LightPosId, _lightOrbs[0].transform.position);
        }

        private void OnDisable() {
            _revealLightsBuffer.Release();
            _revealLightsBuffer = null;
        }
        
    }
}
