using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterPickAttack : MonoBehaviour 
{
    [SerializeField] private PhysicsProjectileData m_projectileData;
    [SerializeField] private GameObject m_waterPrefab;
    [SerializeField] private Transform m_originPoint;
    [SerializeField][Range(0.01f, 1f)] private float m_fireRate = 0.2f;
    // private GameObject m_attacker;
    private Coroutine m_coroutine = null;
    private Transform waterContainer;

    private void Awake() {
        // preload water sprites
        SimpleObjectPool.Preload(m_waterPrefab, 180);
        waterContainer = new GameObject("Water Pick Particles").transform;
    }

    public void StartAttack()
    {
        if (m_coroutine != null) 
        {
            return;
        }
        m_coroutine = StartCoroutine(UpdateAttack());
    }

    public void StopAttack() 
    {
        if (m_coroutine == null) return;
        StopCoroutine(m_coroutine);
        m_coroutine = null;
    }

    public IEnumerator UpdateAttack() 
    {
        while (true) {
            // Debug.Log(">> Spawning Water Particle");
            GameObject obj = SimpleObjectPool.Spawn(m_waterPrefab, m_originPoint.position, waterContainer);

            if (obj.TryGetComponent(out PhysicsProjectile projectile)) 
            {
                Vector3 direction = Vector3.Normalize(m_originPoint.right);
                projectile.SetupProjectile(m_projectileData, direction);
            }
            yield return new WaitForSeconds(m_fireRate);
        }
    }
}
