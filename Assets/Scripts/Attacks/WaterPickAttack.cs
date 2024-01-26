using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterPickAttack : IAttack 
{
    [SerializeField] private ProjectileData m_projectileData;
    [SerializeField] private GameObject m_waterPrefab;
    [SerializeField][Range(0.01f, 1f)] private float fireRate = 0.2f;
    // private GameObject m_attacker;
    private Coroutine m_coroutine = null;

    private void Awake() {
        // preload water sprites
        SimpleObjectPool.Preload(m_waterPrefab, 50);
    }

    public override void StartAttack()
    {
        if (m_coroutine != null) {
            return;
        }
        m_coroutine = StartCoroutine(UpdateAttack());
    }

    public override void StopAttack() 
    {
        if (m_coroutine == null) return;
        StopCoroutine(m_coroutine);
        m_coroutine = null;
    }

    public IEnumerator UpdateAttack() 
    {
        while (m_coroutine != null) {
            GameObject obj = SimpleObjectPool.Spawn(m_waterPrefab, transform.position);
            
            if (obj.TryGetComponent(out Projectile projectile)) {
                projectile.SetupProjectile(m_projectileData, transform.position - obj.transform.position);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }
}
