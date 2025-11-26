using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _01_Scripts.Frogs
{
    public class InsectCapture : MonoBehaviour
    {
        [SerializeField] private float radiusCapture = 1f;
        [SerializeField] private LayerMask layerMask;
        private Vector2 gizmosPoint;

        [SerializeField] private UpdateFader spitPrefab;
        private List<UpdateFader> pullSpits = new List<UpdateFader>();
        
        private void Start()
        {
            gizmosPoint = gameObject.transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var mousePosition = GetMousePosition();
                var insect = InsectCapture.DetectInArea2D<InsectBase>(mousePosition, radiusCapture, layerMask);
                SpawnSpit(mousePosition);
                DestroyInsect(insect);
            }

        }

        public static List<T> DetectInArea2D<T>(Vector2 pointOverlap, float radius, LayerMask mask)
        {
            var count = Physics2D.OverlapCircleAll(pointOverlap, radius, mask);
            return Physics2D.OverlapCircleAll(pointOverlap, radius, mask)
                .Where(i => i.GetComponent<T>() is not null)
                .Select(x => x.GetComponent<T>()).ToList();
        }
        
        // Временная мера по удалению. Нужен пул насекомых куда они будут возвращаться, возможно для каждого насекомого
        private void DestroyInsect(List<InsectBase>  insects)
        {
            foreach (var insect in insects)
            {
                insect.gameObject.SetActive(false);
            }
        }

        private Vector2 GetMousePosition()
        {
            Vector3 screenPosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0;
            return worldPosition;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gizmosPoint, radiusCapture);
        }

        private void SpawnSpit(Vector2 position)
        {
            var spit = pullSpits.FirstOrDefault(s => !s.gameObject.activeInHierarchy);
            if (spit is null)
            {
                spit = Instantiate(spitPrefab,  position, Quaternion.identity, gameObject.transform);
                pullSpits.Add(spit);
            }
            else
            {
                spit.transform.position = position;
            }
            spit.gameObject.SetActive(true);
            spit.StartFade();
        }

    }
}