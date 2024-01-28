using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HookAnimationData {
    public Vector3 destination;
    public Vector3 origin;
    public float duration;
}


public class Hook : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform knot;

    private DamageData damageData;
    private HookAnimationData animationData;
    private Coroutine coroutine = null;
    private float progress = 0;

    public event System.EventHandler OnComplete;

    public void SetDamageData(DamageData data)
    {
        damageData = data;
    }

    public void SetupHook(HookAnimationData data) 
    {
        lineRenderer.SetPosition(0, data.origin + knot.localPosition);
        animationData = data;
        progress = 0;
        StartAnimation();
    }

    private IEnumerator UpdateAnimation() 
    {
        while (true) 
        {
            progress += Time.deltaTime;
            float prog = Mathf.Clamp01(progress / animationData.duration);

            transform.position = Vector3.Lerp(animationData.origin, animationData.destination, prog); 
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, knot.position);

            if (progress >= animationData.duration) 
            {
                StopAnimation();
                OnComplete?.Invoke(this, System.EventArgs.Empty);
                break;
            }
            yield return null;
        }
    }

    private void StartAnimation()
    {
        if (coroutine != null) return;
        coroutine = StartCoroutine(UpdateAnimation());
    }

    private void StopAnimation() 
    {
        if (coroutine == null) return;
        StopCoroutine(coroutine);
        coroutine = null;
    }

    public void Restart() {
        StopAnimation();
        if (progress < animationData.duration) 
        {
            animationData.destination = transform.position;
        }
        Vector3 swapTemp = animationData.destination;
        animationData.destination = animationData.origin;
        animationData.origin = swapTemp;

        progress = 0;
        StartAnimation();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != damageData.targetTag || damageData.damage <= 0)
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out HealthComponent health))
        {
            health.TakeDamage(damageData.damage);
        }
    }
}
