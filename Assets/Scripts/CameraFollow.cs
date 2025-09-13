using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomInSize = 5f;
    public float zoomSpeed = 3f;

    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector3 followOffset;

    private float zoomOutSize;
    private Vector3 originalPosition;
    private Transform target;
    private Camera cam;

    void Start()
    {
        cam = transform.GetComponent<Camera>();
        originalPosition = transform.position;
        zoomOutSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + followOffset;
            desiredPosition.z = originalPosition.z;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }

    public void StartFollowing(Transform playerTarget)
    {
        target = playerTarget;
        StopAllCoroutines();
        StartCoroutine(Zoom(zoomInSize));
    }

    public void StopFollowing()
    {
        target = null;
        StopAllCoroutines();
        StartCoroutine(Zoom(zoomOutSize));
        StartCoroutine(MoveToPosition(originalPosition));
    }

    private IEnumerator Zoom(float targetSize)
    {
        while (Mathf.Abs(cam.orthographicSize - targetSize) > 0.05f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
            yield return null;
        }
        cam.orthographicSize = targetSize;
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, zoomSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
}
