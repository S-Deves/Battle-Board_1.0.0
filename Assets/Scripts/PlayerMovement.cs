using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] waypoints;          // ช่องเดินทั้งหมด
    public float moveDuration = 0.5f;      // เวลาในการเคลื่อนที่แต่ละก้าว
    public float stepPauseDuration = 0.2f; // เวลาหยุดพักแต่ละช่อง
    public float jumpHeight = 1.5f;        // ความสูงของการกระโดด

    public bool isMoving { get; private set; } = false;

    private int currentWaypointIndex = 0;  // ช่องปัจจุบัน
    private SpriteRenderer characterSpriteRenderer;
    private Vector3 originalSpritePosition;

    void Start()
    {
        // หา SpriteRenderer ของตัวละคร
        characterSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (characterSpriteRenderer != null)
            originalSpritePosition = characterSpriteRenderer.transform.localPosition;

        // Sync ตำแหน่งเริ่มต้นให้ตรงกับ Waypoint ที่ใกล้ที่สุด
        if (waypoints.Length > 0)
        {
            float minDist = Mathf.Infinity;
            int nearestIndex = 0;
            for (int i = 0; i < waypoints.Length; i++)
            {
                float dist = Vector3.Distance(transform.position, waypoints[i].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestIndex = i;
                }
            }
            currentWaypointIndex = nearestIndex;
            transform.position = waypoints[nearestIndex].position;
        }
    }

    // เริ่มการเดินตามจำนวนก้าว
    public void StartMoveSequence(int steps)
    {
        if (!isMoving)
            StartCoroutine(MoveSteps(steps));
    }

    // ลูปการเดินทีละก้าว
    private IEnumerator MoveSteps(int steps)
    {
        isMoving = true;
        for (int i = 0; i < steps; i++)
        {
            int nextIndex = (currentWaypointIndex + 1) % waypoints.Length;
            Transform targetWaypoint = waypoints[nextIndex];

            yield return StartCoroutine(JumpToTarget(targetWaypoint));
            currentWaypointIndex = nextIndex;

            // ให้บล็อกเด้งเมื่อเหยียบ
            BounceTile(targetWaypoint);

            yield return new WaitForSeconds(stepPauseDuration);
        }
        isMoving = false;
    }

    // ฟังก์ชันทำให้บล็อกเด้ง
    private void BounceTile(Transform waypoint)
    {
        TileAnimator animator = waypoint.GetComponent<TileAnimator>();
        if (animator == null) animator = waypoint.GetComponentInChildren<TileAnimator>();
        if (animator != null) animator.PlayBounceAnimation();
    }

    // กระโดดไปยังช่องเป้าหมาย
    private IEnumerator JumpToTarget(Transform target)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float fraction = elapsedTime / moveDuration;

            // เคลื่อนตำแหน่ง
            transform.position = Vector3.Lerp(startPos, targetPos, fraction);

            // ทำให้ Sprite เด้งขึ้นลงเหมือนกระโดด
            float jumpArc = jumpHeight * Mathf.Sin(fraction * Mathf.PI);
            if (characterSpriteRenderer != null)
                characterSpriteRenderer.transform.localPosition = originalSpritePosition + new Vector3(0, jumpArc, 0);

            yield return null;
        }

        transform.position = targetPos;
        if (characterSpriteRenderer != null)
            characterSpriteRenderer.transform.localPosition = originalSpritePosition;
    }
}