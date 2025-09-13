using System.Collections;
using UnityEngine;

public class TileAnimator : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine bounceCoroutine;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void PlayBounceAnimation()
    {
        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
            transform.localScale = originalScale;
        }
        bounceCoroutine = StartCoroutine(Bounce(0.3f, 15f));
    }

    private IEnumerator Bounce(float bounceAmount, float bounceSpeed)
    {
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y * (1 - bounceAmount), originalScale.z);

        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            t += Time.deltaTime * bounceSpeed;
            yield return null;
        }
        transform.localScale = targetScale;

        t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            t += Time.deltaTime * bounceSpeed;
            yield return null;
        }
        transform.localScale = originalScale;
        bounceCoroutine = null;
    }
}
