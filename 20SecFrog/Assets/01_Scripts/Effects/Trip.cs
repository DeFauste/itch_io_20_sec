using System;
using System.Collections;
using UnityEngine;

namespace _01_Scripts.Effects
{
    public class Trip : MonoBehaviour
    {
        [SerializeField] private float _rotationsCount = 1f; // кол-во вращений за время трипа
        [SerializeField] private float _timeTrip = 3f;
        [SerializeField] private Transform _tripObject;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _tripSound;
        private void Awake()
        {
            //_audioSource = GetComponent<AudioSource>();
        }

        public void StartTrip(Action callback = null)
        {
            _audioSource.Play();
            StartCoroutine(Rotation(callback));
        }

        private IEnumerator Rotation(Action callback = null)
        {
            var startRotation = _tripObject.localEulerAngles;
            var endRotation = _tripObject.localEulerAngles + new Vector3(0f, 0f, _rotationsCount * 360);
            float elapsedTime = 0f;

            while (elapsedTime < _timeTrip)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / _timeTrip;
                var newRotation = Vector3.Lerp(startRotation, endRotation, percentageComplete);
                transform.rotation = Quaternion.Euler(newRotation);
                yield return null;
            }

            callback?.Invoke();
        }
    }
}