using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteAttack : IAttack
{
    [SerializeField] private ProjectileData m_projectileData;
    [SerializeField] private GameObject m_notePrefab;
    [SerializeField][Range(0.01f, 1f)] private float fireRate = 0.2f;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip startWhistleAnimation;
    private IEnumerator m_coroutine = null;

    private void Awake()
    {
        Transform parent = new GameObject("Music Notes").transform;
        // preload music notes sprites
        SimpleObjectPool.Preload(m_notePrefab, 30, parent);
    }

    public override void StartAttack()
    {
        base.StartAttack();
        if (m_coroutine != null) {
            return;
        }
        animator.Play("BossWhistleStart");
        m_coroutine = UpdateAttack();
        StartCoroutine(m_coroutine);

    }

    public override void StopAttack()
    {
        base.StopAttack();
        if (m_coroutine == null) return;
        StopCoroutine(m_coroutine);
        m_coroutine = null;
        animator.SetTrigger("DoneWhistling");
    }

    public IEnumerator UpdateAttack() 
    {
        yield return new WaitForSeconds(startWhistleAnimation.length);
        while (m_coroutine != null)
        {
            GameObject obj = SimpleObjectPool.Spawn(m_notePrefab, transform.position);
            if (obj.TryGetComponent(out Projectile projectile))
            {
                projectile.SetupProjectile(m_projectileData, new Vector3(Random.Range(-1.4f, 3f), -1, 0).normalized);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

}
