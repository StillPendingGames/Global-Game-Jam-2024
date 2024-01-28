using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class FinSlamAttack : IAttack
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform dropBase;
    [SerializeField] private float shakeDuration = 3;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private GameObject[] fallingObjPrefabs;
    [SerializeField][Range(0.01f, 1f)] private float fireRate = 0.2f;
    [SerializeField] private AnimationClip finSlamAnimation;
    private IEnumerator coroutine = null;

    private void Awake()
    {
        Transform parent = new GameObject("Falling Objects").transform;
        foreach (GameObject item in fallingObjPrefabs)
        {
            SimpleObjectPool.Preload(item, 8, parent);
        }
    }

    public override void StartAttack()
    {
        if (coroutine != null) return;

        animator.Play("Smack");
        coroutine = UpdateAttack();
        StartCoroutine(coroutine);
    }

    public override void StopAttack()
    {
        base.StopAttack();
        if (coroutine == null) return;
        StopCoroutine(coroutine);
        coroutine = null;
        BossController.Instance.StopLaughing();
    }

    public IEnumerator UpdateAttack()
    {
        Vector3 startPosition = cameraTransform.position;
        float elapsedTime = 0f;

        yield return new WaitForSeconds(finSlamAnimation.length);
        AudioManager.Instance.Play("Fish Slap");

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float strength = shakeCurve.Evaluate(elapsedTime / duration);
            cameraTransform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        cameraTransform.position = startPosition;
        base.StartAttack();

        StartCoroutine(BossController.Instance.ShowValve());

        while (coroutine != null)
        {   
            Vector3 spawnPos = dropBase.position;
            spawnPos.x = dropBase.position.x + Random.Range(-8.36f, 8.36f);
            GameObject obj = SimpleObjectPool.Spawn(fallingObjPrefabs[Random.Range(0, fallingObjPrefabs.Length)], spawnPos);
            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetupProjectile(projectileData, new Vector3(0, -1, 0).normalized);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

}