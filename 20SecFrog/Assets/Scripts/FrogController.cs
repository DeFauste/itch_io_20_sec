using System.Collections;
using System.Collections.Generic;
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

    [Header("Tongue References")]
    public RectTransform tongueOrigin;
    public GameObject tongueTip;

    [Header("Tongue Physics")]
    [Range(1, 50)] public int ropeSegments = 15;
    [Range(0.01f, 1f)] public float segmentLength = 20f;
    [Range(1f, 100f)] public float tongueSpeed = 20f;
    [Range(1f, 100f)] public float retractSpeed = 15f;
    [Range(0.01f, 1f)] public float gravity = 1f;
    [Range(0.01f, 1f)] public float damping = 0.95f;
    [Range(1, 10)] public int physicsIterations = 3;

    [Header("Tongue Timing")]
    [Range(0.01f, 1f)] public float tongueTipAliveTimeSec = 0.1f;
    // TODO: Maybe not needed, because have retraction, but may be will use it for debuffs
    [Range(0.01f, 1f)] public float tongueCooldownTimeSec = 0.1f;

    [Header("Tongue Visual")]
    public LineRenderer tongueLineRenderer;
    public Material tongueMaterial;
    public Texture2D tongueTexture;
    public Color tongueBaseColor = new Color(1f, 0.3f, 0.3f, 1f);
    public Color tongueTipColor = new Color(1f, 0.6f, 0.6f, 1f);
    [Range(0f, 1f)] public float endRoundness = 0.3f;
    [Range(0.01f, 1f)] public float tongueWidth = 0.5f;

    private Canvas canvas;
    private RectTransform canvasTransform;

    private bool coolingDown = false;
    private Vector3 mousePointWorldSpace = Vector3.zero;
    private RectTransform tongueTipTransform;

    private class RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }

    // Rope simulation
    private List<RopeSegment> ropeSegments_list = new List<RopeSegment>();
    private bool tongueActive = false;
    private Vector3 targetPosition;
    private bool extending = false;
    private bool retracting = false;
    private float retractTimer = 0f;
    private float segmentsToRemove = 0f;

    private Vector2 leftEyeOriginalPos;
    private Vector2 rightEyeOriginalPos;

    private Vector2 leftEyeTargetPos;
    private Vector2 rightEyeTargetPos;

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

        canvasTransform = canvas.GetComponent<RectTransform>();
        if (canvasTransform == null)
        {
            Debug.LogError("Failed to get transform component from canvas.");
            enabled = false;
            return;
        }

        if (leftEye == null || rightEye == null)
        {
            Debug.LogError("Eye references are not set! Please assign both left and right eye RectTransforms in the inspector.");
            enabled = false;
            return;
        }

        if (tongueOrigin == null)
        {
            Debug.LogError("No tongue origin set.");
            enabled = false;
            return;
        }

        if (tongueTip == null)
        {
            Debug.LogError("No tongue tip set.");
            enabled = false;
            return;
        }

        tongueTipTransform = tongueTip.GetComponent<RectTransform>();
        if (tongueTipTransform == null)
        {
            Debug.LogError("Failed to get tongue tip transform.");
            enabled = false;
            return;
        }

        leftEyeOriginalPos = leftEye.anchoredPosition;
        rightEyeOriginalPos = rightEye.anchoredPosition;

        leftEyeTargetPos = leftEyeOriginalPos;
        rightEyeTargetPos = rightEyeOriginalPos;

        // Init line renderer
        if (tongueLineRenderer == null)
        {
            tongueLineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        if (tongueMaterial != null)
        {
            tongueLineRenderer.material = tongueMaterial;
        }
        else
        {
            // Try to find the custom shader
            Shader tongueShader = Shader.Find("Custom/TongueShader");
            if (tongueShader != null)
            {
                tongueMaterial = new Material(tongueShader);
                tongueLineRenderer.material = tongueMaterial;
            }
            else
            {
                // Fallback to default material
                tongueMaterial = new Material(Shader.Find("Sprites/Default"));
                tongueLineRenderer.material = tongueMaterial;
            }
        }

        if (tongueMaterial != null)
        {
            if (tongueTexture != null)
            {
                tongueMaterial.mainTexture = tongueTexture;
            }

            if (tongueMaterial.HasProperty("_Color"))
                tongueMaterial.SetColor("_Color", tongueBaseColor);

            if (tongueMaterial.HasProperty("_TipColor"))
                tongueMaterial.SetColor("_TipColor", tongueTipColor);

            if (tongueMaterial.HasProperty("_EndRoundness"))
                tongueMaterial.SetFloat("_EndRoundness", endRoundness);
        }

        tongueLineRenderer.startWidth = tongueWidth * 0.3f;
        tongueLineRenderer.endWidth = tongueWidth;
        tongueLineRenderer.sortingOrder = 100;
        tongueLineRenderer.textureMode = LineTextureMode.DistributePerSegment;

        tongueLineRenderer.enabled = false;
        tongueTip.SetActive(false);
    }

    void UpdateMouseWorldPosition()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasTransform,
            mouseScreenPosition,
            canvas.worldCamera,
            out mousePointWorldSpace
        );
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
        UpdateMouseWorldPosition();

        Debug.DrawLine(tongueOrigin.position, mousePointWorldSpace, Color.green);

        if (Input.GetMouseButtonDown(0) && !tongueActive && !coolingDown)
        {
            ShootTongue();
        }

        if(canvas == null)
            return;

        if (leftEye != null)
        {
            UpdateEyePosition(leftEye, leftEyeOriginalPos, ref leftEyeTargetPos);
        }

        if (rightEye != null)
        {
            UpdateEyePosition(rightEye, rightEyeOriginalPos, ref rightEyeTargetPos);
        }
    }

    void FixedUpdate()
    {
        if (tongueActive)
        {
            SimulateRope();
        }
    }

    void ShootTongue()
    {
        targetPosition = mousePointWorldSpace;
        tongueActive = true;
        extending = true;
        retracting = false;
        retractTimer = 0f;
        segmentsToRemove = 0f;

        ropeSegments_list.Clear();
        Vector3 startPos = tongueOrigin.position;

        ropeSegments_list.Add(new RopeSegment(startPos));
        ropeSegments_list.Add(new RopeSegment(startPos));

        tongueLineRenderer.enabled = true;
        StartCoroutine(TongueAnimation());
    }

    void SimulateRope()
    {
        if (extending)
        {
            // Calculate how many segments to add per second
            float addRate = tongueSpeed / segmentLength;
            segmentsToRemove += addRate * Time.fixedDeltaTime; // Reusing the counter variable

            while (segmentsToRemove >= 1f && ropeSegments_list.Count < ropeSegments)
            {
                // Add new segment at the current tip position
                Vector3 tipPos = ropeSegments_list[ropeSegments_list.Count - 1].posNow;
                ropeSegments_list.Add(new RopeSegment(tipPos));
                segmentsToRemove -= 1f;
            }
        }

        if (retracting)
        {
            retractTimer += Time.fixedDeltaTime;
            // Calculate how many segments to remove per second
            float removeRate = retractSpeed / segmentLength;
            segmentsToRemove += removeRate * Time.fixedDeltaTime;

            while (segmentsToRemove >= 1f && ropeSegments_list.Count > 2)
            {
                ropeSegments_list.RemoveAt(ropeSegments_list.Count - 1);
                segmentsToRemove -= 1f;
            }

            // Check if tongue is fully retracted
            if (ropeSegments_list.Count <= 2)
            {
                tongueActive = false;
                extending = false;
                retracting = false;
                tongueLineRenderer.enabled = false;
                return;
            }
        }

        // Apply physics to segments
        for (int i = 0; i < ropeSegments_list.Count; i++)
        {
            RopeSegment segment = ropeSegments_list[i];
            Vector3 velocity = segment.posNow - segment.posOld;
            segment.posOld = segment.posNow;

            velocity.y -= gravity * Time.fixedDeltaTime;
            velocity *= damping;

            segment.posNow += velocity;
        }

        for (int iteration = 0; iteration < physicsIterations; iteration++)
        {
            ApplyConstraints();
        }

        UpdateLineRenderer();
    }

    void ApplyConstraints()
    {
        ropeSegments_list[0].posNow = tongueOrigin.position;

        if (extending)
        {
            Vector3 direction = (targetPosition - ropeSegments_list[ropeSegments_list.Count - 1].posNow).normalized;
            ropeSegments_list[ropeSegments_list.Count - 1].posNow += direction * tongueSpeed * Time.fixedDeltaTime;
        }

        for (int i = 0; i < ropeSegments_list.Count - 1; i++)
        {
            RopeSegment segmentA = ropeSegments_list[i];
            RopeSegment segmentB = ropeSegments_list[i + 1];

            Vector3 delta = segmentB.posNow - segmentA.posNow;
            float distance = delta.magnitude;
            float difference = distance - segmentLength;

            if (distance > 0)
            {
                Vector3 correction = delta.normalized * difference * 0.5f;

                // NOTE: Don't move first segment (it's anchored)
                if (i != 0) 
                {
                    segmentA.posNow += correction;
                }
                segmentB.posNow -= correction;
            }
        }
    }

    void UpdateLineRenderer()
    {
        tongueLineRenderer.positionCount = ropeSegments_list.Count;

        for (int i = 0; i < ropeSegments_list.Count; i++)
        {
            tongueLineRenderer.SetPosition(i, ropeSegments_list[i].posNow);
        }

        // Update tongue tip position
        if (ropeSegments_list.Count > 0)
        {
            tongueTipTransform.position = ropeSegments_list[ropeSegments_list.Count - 1].posNow;
        }
    }

    IEnumerator TongueAnimation()
    {
        // Wait for tongue to reach target
        while (extending)
        {
            float distToTarget = Vector3.Distance(
                ropeSegments_list[ropeSegments_list.Count - 1].posNow,
                targetPosition
            );

            if (distToTarget < segmentLength * 1.5f || ropeSegments_list.Count >= ropeSegments)
            {
                extending = false;
                break;
            }

            yield return null;
        }

        tongueTip.SetActive(true);
        yield return new WaitForSeconds(tongueTipAliveTimeSec);
        tongueTip.SetActive(false);


        retracting = true;
        while (retracting && tongueActive)
        {
            yield return null;
        }

        coolingDown = true;
        yield return new WaitForSeconds(tongueCooldownTimeSec);
        coolingDown = false;
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
