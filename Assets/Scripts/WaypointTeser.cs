using UnityEngine;

public class WaypointTester : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentIndex = 0;

    void Start()
    {
        // วาร์ปไปที่จุดเริ่มต้น
        if (waypoints.Length > 0 && waypoints[0] != null)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        // เมื่อกด Spacebar ให้วาร์ปไปช่องถัดไป
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (waypoints.Length == 0) return;

            currentIndex = (currentIndex + 1) % waypoints.Length;

            if (waypoints[currentIndex] != null)
            {
                transform.position = waypoints[currentIndex].position;
                Debug.Log("วาร์ปไปยัง Waypoint หมายเลข: " + currentIndex);
            }
        }
    }
}
