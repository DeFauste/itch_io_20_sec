using UnityEngine;

namespace Assets._01_Scripts.Environment
{
    public class MoveClouds : MonoBehaviour
    {
        public bool DirectionRight { get; set; } = false;
        public float speed { get; set; } = 0f;
        public bool IsMove { get; set; } = false;

        private void FixedUpdate()
        {
            if (IsMove)
            {
                var direction = DirectionRight ? 1 : -1;
                gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed * direction * Time.fixedDeltaTime, gameObject.transform.position.y);

            }
        }
    }
}
