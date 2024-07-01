using Some1.Play.Info;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class CharacterElementStuff : MonoBehaviour
    {
        public Transform handle;
        public GameObject takableEffect;
        public ParticleSystem takableEffectParticleSystem;
        public float takableY;

        private const float EmissionFadeOutTime = 5;

        private float _targetY = 0;
        private float _speed = 0;
        private bool _isTaken;
        private float _originalParticle;
        private float _aliveCycles;
        private float _normalizedEmissionFadeOutTime;

        public bool IsTaken
        {
            get => _isTaken;

            set
            {
                if (_isTaken == value)
                {
                    return;
                }

                _isTaken = value;
                
                if (enabled)
                {
                    takableEffect.SetActive(!IsTaken);
                }
            }
        }

        public float AliveCycles
        {
            get => _aliveCycles;

            set
            {
                if (_aliveCycles == value)
                {
                    return;
                }

                // Must (cycles == seconds)
                _aliveCycles = value;

                float remainAliveTime = PlayConst.GiveStuffSeconds - value;
                SetNormalizedEmissionFadeOutTime(remainAliveTime / EmissionFadeOutTime);
            }
        }

        private void SetNormalizedEmissionFadeOutTime(float value)
        {
            value = Mathf.Clamp01(value);
            if (_normalizedEmissionFadeOutTime == value)
            {
                return;
            }

            _normalizedEmissionFadeOutTime = value;

            var emission = takableEffectParticleSystem.emission;
            emission.rateOverTime = _originalParticle * value;
        }

        private void Awake()
        {
            _originalParticle = takableEffectParticleSystem.emission.rateOverTime.constant;
        }

        private void Update()
        {
            UpdateHandle();
        }

        private void OnEnable()
        {
            takableEffect.SetActive(!IsTaken);
        }

        private void OnDisable()
        {
            handle.localPosition = Vector3.zero;
            takableEffect.SetActive(false);
            _targetY = 0;
            _speed = 0;
        }

        private void UpdateHandle()
        {
            if (IsTaken)
            {
                const float TakenSpeed = 4;

                _targetY = 0;
                _speed = TakenSpeed;
            }
            else
            {
                const float FirstUpSpeed = 2f;
                const float RoundTripHeight = 0.4f;
                const float RoundTripSpeed = 1f;

                if (_targetY == 0)
                {
                    _targetY = takableY + RoundTripHeight;
                    _speed = FirstUpSpeed;
                }
                else if (Mathf.Abs(_targetY - handle.localPosition.y) < 0.1f)
                {
                    _targetY = _targetY == takableY ? takableY + RoundTripHeight : takableY;
                    _speed = RoundTripSpeed;
                }
            }

            handle.localPosition = Vector3.Lerp(handle.localPosition, new(0, _targetY, 0), _speed * Time.deltaTime);
        }
    }
}
