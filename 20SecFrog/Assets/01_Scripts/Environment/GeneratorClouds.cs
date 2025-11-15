using System.Collections.Generic;
using UnityEngine;
namespace Assets._01_Scripts.Environment
{
    public class GeneratorClouds : MonoBehaviour
    {
        [Header("Clouds")]
        [SerializeField] private SpriteRenderer _prefSprite;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private bool rightDirection; //облака движуться в право если true
        [SerializeField] private Transform leftPoint; //крайняя левая точка полёта облаков
        [SerializeField] private Transform rightPoint;//крайняя правая точка полёта облаков
        [SerializeField] private Transform heightPoint; // высота на которой генерируются облака
        [SerializeField] private float offsetHeightSpawn = 0f; // отклонение появление от точки высоты + offset
        [SerializeField] private Vector2 minMaxSpeed; //x - минимальная скорость, y - максимальная скорость
        [SerializeField] private float timeSpawn; //  время между появлением облаков в секундах
        
        [SerializeField] private bool generateCloudsOnStart = false; //true - генерирует облака при запуске скрипта в позиции между левой и правой точкой
        [SerializeField] private int countClounsOnStart = 5; //количество облаков при старте

        private List<GameObject> clouds = new List<GameObject>(); //Ключ это сами облака, значение их скорость движения

        private void Start()
        {
            //Random.InitState(((int)Time.timeAsDouble));

            if (generateCloudsOnStart)
            {
                GenerateCloudsBetweenPoints();
            }
        }
        private void FixedUpdate()
        {
            foreach (var cloud in clouds)
            {
                if (rightDirection && cloud.transform.transform.position.x > rightPoint.position.x)
                {
                    cloud.transform.position = new Vector2(leftPoint.position.x, cloud.transform.position.y);
                }
                else if (!rightDirection && cloud.transform.position.x < leftPoint.position.x)
                {
                    cloud.transform.position = new Vector2(rightPoint.position.x, cloud.transform.position.y);
                }
            }
        }

        private void GenerateCloudsBetweenPoints()
        {
            int countClouds = sprites.Length;
            for (int i = 0; i < countClounsOnStart; i++)
            {
                var rndCloud = Random.Range(0, countClouds);
                var positionX = Random.Range(leftPoint.position.x, rightPoint.position.x);
                var positionY = Random.Range(heightPoint.position.y, heightPoint.position.y + offsetHeightSpawn);
                var spriteRenderer = Instantiate(_prefSprite, new Vector3(positionX, positionY), Quaternion.identity);
                spriteRenderer.sprite = sprites[rndCloud];
                var speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
                var cloudMove = spriteRenderer.GetComponent<MoveClouds>();
                if (cloudMove != null)
                {
                    cloudMove.speed = speed;
                    cloudMove.DirectionRight = rightDirection;
                    cloudMove.IsMove = true;
                }
                spriteRenderer.gameObject.transform.SetParent(this.transform, false);
                clouds.Add(spriteRenderer.gameObject);

            }
        }
    }
}
