using System.Collections;
using UnityEngine;

namespace game {
    public class RevealLightOrb : MonoBehaviour {
        private enum ELightState : byte { On, Off, TurningOn, TurningOff }
        [SerializeField] private float _maxLightPixelRadius = 60;
        [SerializeField] private ELightState _lightState = ELightState.On;
        [SerializeField] private float _turnOffLightTimeSeconds = 1f;
        [SerializeField] private float _turnOnLightTimeSeconds = 1f;

        [HideInInspector] public float LightPixelRadius;
        [SerializeField] private bool _shouldSwitchState = false; // TODO: remove serialize field when done testing

        private void Awake() {
            LightPixelRadius = _maxLightPixelRadius;
        }

        private void Update() {
            if (_shouldSwitchState && _lightState == ELightState.On) {
                StartCoroutine(TurnOffLightCoroutine());
            }

            if (_shouldSwitchState && _lightState == ELightState.Off) {
                StartCoroutine(TurnOnLightCoroutine());
            }
        }

        public void TurnOnLight() {
            if (_lightState == ELightState.On) return;
            if (_lightState == ELightState.TurningOn) return;
            _shouldSwitchState = true;
        }

        public void TurnOffLight() {
            if (_lightState == ELightState.Off) return;
            if (_lightState == ELightState.TurningOff) return;
            _shouldSwitchState = true;
        }

        private IEnumerator TurnOffLightCoroutine() {
            _shouldSwitchState = false;
            _lightState = ELightState.TurningOff;
            
            float timeTotal = _turnOffLightTimeSeconds;
            float radiusFinal = 0f;
            float radiusInitial = LightPixelRadius;

            yield return StartCoroutine(ChangeRadius(timeTotal, radiusInitial, radiusFinal));
            
            // The while loop doesn't guarantee that we get to the target radius
            LightPixelRadius = radiusFinal;
            _lightState = ELightState.Off;
        }

        private IEnumerator TurnOnLightCoroutine() {
            _shouldSwitchState = false;
            _lightState = ELightState.TurningOn;
            
            float timeTotal = _turnOnLightTimeSeconds;
            float radiusFinal = _maxLightPixelRadius;
            float radiusInitial = LightPixelRadius;

            yield return StartCoroutine(ChangeRadius(timeTotal, radiusInitial, radiusFinal));
            
            // The while loop doesn't guarantee that we get to the target radius
            LightPixelRadius = radiusFinal;
            _lightState = ELightState.On;
        }

        private IEnumerator ChangeRadius(float timeTotal, float radiusInitial, float radiusFinal) {
            float timeElapsed = 0f;

            while (timeElapsed <= timeTotal) {
                timeElapsed += Time.deltaTime;
                float fractionComplete = timeElapsed / timeTotal;
                LightPixelRadius = Mathf.Lerp(radiusInitial, radiusFinal, fractionComplete);
                yield return null;
            }
        }

    }
}
