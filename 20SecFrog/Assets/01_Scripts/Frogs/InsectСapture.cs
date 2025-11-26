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


        [SerializeField] private SpriteRenderer cursor;
        [SerializeField] private bool AtiveteCursor = false;

        private void Start()
        {
            gizmosPoint = gameObject.transform.position;
        }

        private void Update()
        {
            var mousePosition = GetMousePosition();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var insect = DetectInArea2D<InsectBase>(mousePosition, radiusCapture, layerMask);
                SpawnSpit(mousePosition);
                DestroyInsect(insect);
            }

            // Удалить условие когда прикрутим логику к меню и языку
            if (AtiveteCursor)
            {
                ActiveCursor();
            }
            else
            {
                DeactiveCursor();
            }

            CursorMoved(mousePosition);
        }

        public Vector2 Position => cursor.transform.position;

        private void CursorMoved(Vector2 position)
        {
            
            if (cursor is not null)
            {
                cursor.gameObject.transform.position = position;
            }
        }
        
        public void ActiveCursor()
        {
            // Выключаем курсор компа
            Cursor.visible = false;
            cursor?.gameObject.SetActive(true);
        }

        public void DeactiveCursor()
        {
            Cursor.visible = true;
            cursor?.gameObject.SetActive(false);
        }

        public static List<T> DetectInArea2D<T>(Vector2 pointOverlap, float radius, LayerMask mask)
        {
            var count = Physics2D.OverlapCircleAll(pointOverlap, radius, mask);
            return Physics2D.OverlapCircleAll(pointOverlap, radius, mask)
                .Where(i => i.GetComponent<T>() is not null)
                .Select(x => x.GetComponent<T>()).ToList();
        }

        // Временная мера по удалению. Нужен пул насекомых куда они будут возвращаться, возможно для каждого насекомого
        private void DestroyInsect(List<InsectBase> insects)
        {
            foreach (var insect in insects)
            {
                insect.gameObject.SetActive(false);
            }
        }

        private Vector2 GetMousePosition()
        {
            Vector3 screenPos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            return worldPos;
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
                spit = Instantiate(spitPrefab, position, Quaternion.identity, gameObject.transform);
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