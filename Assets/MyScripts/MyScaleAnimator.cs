using System.Collections;
using UnityEngine;

public class MyScaleAnimator : MonoBehaviour
{
    [SerializeField] float duration = 1.0f;

    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    void Start()
    {
        StartCoroutine(ScaleOverTime(duration));
    }

    private IEnumerator ScaleOverTime(float duration)
    {
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = scaleCurve.Evaluate(elapsed / duration) * Vector3.one;
            
            elapsed += Time.deltaTime;
            
            yield return null;
        }
    }
}