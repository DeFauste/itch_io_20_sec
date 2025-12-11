using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01_Scripts.Effects;
using AYellowpaper.SerializedCollections;
using MainMenu;
using UnityEngine;

namespace _01_Scripts.Frogs
{
    public class InsectCapture : MonoBehaviour
    {
        [SerializeField] private Score.Score score; 
        
        [SerializeField] private float radiusCapture = 1f;
        [SerializeField] private LayerMask layerMask;
        private Vector2 gizmosPoint;

        [SerializeField] private UpdateFader spitPrefab;
        private List<UpdateFader> pullSpits = new List<UpdateFader>();


        [SerializeField] private SpriteRenderer cursor;
        [SerializeField] public bool AtiveteCursor = false;
        public int CursorReverse = 1; // инвертирование прицела

        private bool isLocked = false;
        
        [Header("Трупы насекомых")] [Tooltip("Записываем все трупы под тип")]
        [SerializeField] private SerializedDictionary<InsectType, GameObject> listInsectDead = new();
        private GameState  gameState;
        // Дебафы
        [SerializeField] private Trip trip;
        private bool isActiveSpeedEffect = false; // активировано уже замедление или нет
        // Дебафы
        
        private Vector2 _mousePosition;
        private RandomSoundPlayer _smashSound;

        private void Awake()
        {
            _smashSound = GetComponent<RandomSoundPlayer>();
        }

        private void Start()
        {
            gameState = GameState.Instance;
            gizmosPoint = gameObject.transform.position;
        }

        private void Update()
        {
            _mousePosition = GetMousePosition();
            CursorMoved(_mousePosition);
        }

        public void LockCursor()
        {
            isLocked = true;
        }

        public void MakeShot(Transform tongueTip)
        {
            isLocked = false;

            var insects = DetectInArea2D<InsectBase>(_mousePosition, radiusCapture, layerMask);
            SpawnSpit(cursor.transform.position);
            DestroyInsects(insects, tongueTip);
        }
        public Vector2 PositionCursor => cursor.transform.position;

        private void CursorMoved(Vector2 position)
        {
            if(isLocked)
            {
                return;
            }

            if (cursor is not null)
            {
                cursor.gameObject.transform.position = position * CursorReverse;
            }
        }

        public void ActiveReverseCursor()
        {
            trip.gameObject.SetActive(true);
            trip.StartTrip(DeactiveReverseCursor);
            CursorReverse *= -1;
        }

        public void DeactiveReverseCursor()
        {
            trip.gameObject.SetActive(false);
            CursorReverse = 1;
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
            return Physics2D.OverlapCircleAll(pointOverlap, radius, mask)
                .Where(i => i.GetComponent<T>() is not null)
                .Select(x => x.GetComponent<T>()).ToList();
        }

        // Временная мера по удалению. Нужен пул насекомых куда они будут возвращаться, возможно для каждого насекомого
        private void DestroyInsects(List<InsectBase> insects, Transform tongueTip)
        {
            foreach (var insect in insects)
            {      
                var objInsect = Instantiate(listInsectDead[insect.GetInsectType()],  insect.transform.position, Quaternion.identity);
                // убрал механику съедания
                //objInsect.transform.parent = tongueTip;
                _smashSound.PlaySound();
                Effects(insect.GetInsectType());
                score.AddScore(insect.GetScore());
                insect.gameObject.SetActive(false);
            }
        }

        private void Effects(InsectType type)
        {
            switch (type)
            {
                case InsectType.Mosquito:
                    break;
                case InsectType.Fly:
                    break;
                case InsectType.Dragonfly:
                    break;
                case InsectType.Maybug:
                    break;
                case InsectType.Ladybug:
                    SpeedEffect();
                    break;
                case InsectType.Wasp:
                    break;
                case InsectType.Butterfly:
                    ActiveReverseCursor();
                    break;
            }
        }

        private void SpeedEffect()
        {
            if (!isActiveSpeedEffect)
            {
                gameState.SetSpeedInsectPercent(0.5f);
                TimerDisableEffect(2,() =>
                {
                    gameState.ResetSpeedInsectPercent();
                    isActiveSpeedEffect = false;
                });
                isActiveSpeedEffect = true;
            }
        }
        
        private void TimerDisableEffect(int seconds, Action  action)
        {
            StartCoroutine(StartTimer(seconds, action));
        }
        
        private IEnumerator StartTimer(int seconds, Action action)
        {
            float time = seconds;
            while (time > 0)
            {
                yield return new WaitForSeconds(1);

                    time -= 1;
            }
            action?.Invoke();
        }
        
        public void DestroyAfterTime(GameObject obj, float timeSeconds)
        {
            Destroy(obj, timeSeconds);
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

            var player = spit.GetComponent<RandomSoundPlayer>();
            if(player != null)
            {
                player.PlaySound();
            }
        }
    }
}