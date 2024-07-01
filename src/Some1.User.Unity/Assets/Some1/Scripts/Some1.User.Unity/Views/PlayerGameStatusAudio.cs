using System;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerCastAudio
    {

    }

    public class PlayerEnergyAudio
    {

    }

    public enum PlayerGameStatus
    {
        Ready,
        Return,
        Start,
        End,
    }

    public class PlayerGameStatusAudio : MonoBehaviour
    {
        public AudioSource[] _sources;

        private PlayerGameStatus _value;

        public void SetStatus(PlayerGameStatus value)
        {
            if (_value == value)
            {
                return;
            }

            _value = value;

            var source = GetSource(value);
            if (source != null)
            {
                source.Play();
            }
        }

        private void OnDisable()
        {
            foreach (var item in _sources)
            {
                item.Stop();
            }
        }

        private AudioSource GetSource(PlayerGameStatus value)
        {
            throw new NotImplementedException();
        }
    }
}
