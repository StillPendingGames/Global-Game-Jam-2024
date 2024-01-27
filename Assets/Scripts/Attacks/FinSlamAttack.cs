using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class FinSlamAttack : IAttack
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform dropBase;
    [SerializeField] private float shakeDuration = 3;
    [SerializeField] private AnimationCurve shakeCurve;
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private GameObject[] fallingObjPrefabs;
    [SerializeField][Range(0.01f, 1f)] private float fireRate = 0.2f;
    private IEnumerator m_coroutine = null;

    private void Awake()
    {
        foreach (GameObject item in fallingObjPrefabs)
        {
            SimpleObjectPool.Preload(item, 8);
        }
    }

    public override void StartAttack()
    {
        if (m_coroutine != null) {
            return;
        }
        m_coroutine = UpdateAttack();
        StartCoroutine(m_coroutine);
    }

    public override void StopAttack()
    {
        base.StopAttack();
        if (m_coroutine == null) return;
        StopCoroutine(m_coroutine);
        m_coroutine = null;
    }

    public IEnumerator UpdateAttack()
    {

        Vector3 startPosition = cameraTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float strength = shakeCurve.Evaluate(elapsedTime / duration);
            cameraTransform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        cameraTransform.position = startPosition;
        base.StartAttack();

        while (m_coroutine != null)
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