using System;
using System.Collections;
using UnityEngine;

namespace _01_Scripts.Effects
{
    public class Trip : MonoBehaviour
    {
        public float targetAngle = 10; // угол в градусах
        public float rotationSpeed = 2f;
        public float SpeedRound = 0.1f;
        public float TimeTrip = 5;
        public Transform tripObject;
        private void Start()
        {
        }

        public void StartTrip()
        {
            StartCoroutine(Rotation());
        }

        private IEnumerator Rotation()
        {
            float time = 0;
            while (time <= TimeTrip)
            {
                yield return new WaitForSeconds(SpeedRound);
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                tripObject.rotation = Quaternion.Slerp(tripObject.rotation, targetRotation, rotationSpeed);
                targetAngle += rotationSpeed;
                time += SpeedRound;
            }
        }
        
    }
}