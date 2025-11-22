using System;
using UnityEngine;

namespace _01_Scripts.Frogs
{
    public sealed class FrogEyes : MonoBehaviour
    {
        [SerializeField] private Transform leftEye;
        [SerializeField] private Transform rightEye;


        private void FixedUpdate()
        {
        }
        
        private Vector3 MousePosition() => Camera.main.ScreenToWorldPoint(
        new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                10f) // расстояние от камеры
            );
    }
}