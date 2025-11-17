using UnityEngine;

public class FrogController : MonoBehaviour
{
    [Header("Eye References")]
    [Tooltip("Reference to the left eye Image component")]
    public RectTransform leftEye;

    [Tooltip("Reference to the right eye Image component")]
    public RectTransform rightEye;

    [Header("Eye Movement Settings")]
    [Range(0f, 30f)]
    public float maxEyeMovement = 15f;

    [Range(0f, 1f)]
    public float smoothSpeed = 0.4f;

    private Vector2 leftEyeOriginalPos;
    private Vector2 rightEyeOriginalPos;

    private Vector2 leftEyeTargetPos;
    private Vector2 rightEyeTargetPos;

    private Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvas.pixelPerfect = false;

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in parent hierarchy!");
            enabled = false;
            return;
        }

        if (leftEye == null || rightEye == null)
        {
            Debug.LogError("Eye references are not set! Please assign both left and right eye RectTransforms in the inspector.");
            enabled = false;
            return;
        }

        leftEyeOriginalPos = leftEye.anchoredPosition;
        rightEyeOriginalPos = rightEye.anchoredPosition;

        leftEyeTargetPos = leftEyeOriginalPos;
        rightEyeTargetPos = rightEyeOriginalPos;
    }

    void UpdateEyePosition(RectTransform eye, Vector2 originalPos, ref Vector2 targetPos)
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, eye.position);
        Vector2 direction = (mouseScreenPosition - screenPosition).normalized;

        targetPos = originalPos + direction * maxEyeMovement;
        eye.anchoredPosition = Vector2.MoveTowards(eye.anchoredPosition, targetPos, smoothSpeed);
    }

    void Update()
    {
        if (leftEye == null || rightEye == null || canvas == null)
            return;

        UpdateEyePosition(leftEye, leftEyeOriginalPos, ref leftEyeTargetPos);
        UpdateEyePosition(rightEye, rightEyeOriginalPos, ref rightEyeTargetPos);
    }

    public void ResetEyes()
    {
        if (leftEye != null)
            leftEye.anchoredPosition = leftEyeOriginalPos;

        if (rightEye != null)
            rightEye.anchoredPosition = rightEyeOriginalPos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (leftEye != null && rightEye != null)
        {
            Gizmos.color = Color.yellow;

            Vector3 leftEyeWorldPos = leftEye.position;
            Vector3 rightEyeWorldPos = rightEye.position;

            Gizmos.DrawWireSphere(leftEyeWorldPos, maxEyeMovement);
            Gizmos.DrawWireSphere(rightEyeWorldPos, maxEyeMovement);
        }
    }
#endif
}
