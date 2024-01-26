using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private IAttack[] attacks;
    [SerializeField] protected float cooldownBetweenAttacks;
    private IAttack currentAttack;
    private bool attacking;

    private void Awake()
    {
        foreach (IAttack attack in attacks)
        {
            attack.OnAttackFinished += CurrentAttackEnded;
        }
    }

    private void Start()
    {
        StartFight();
    }

    public void StartFight()
    {
       StartCoroutine(Cooldown(3));
    }

    private void CurrentAttackEnded(object sender, System.EventArgs e)
    {
        StartCoroutine(Cooldown(cooldownBetweenAttacks));
    }

    private IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        NextAttack();
    }

    private void NextAttack()
    {
        currentAttack = attacks[Random.Range(0, attacks.Length)];
        currentAttack.StartAttack();
    }

    public void StartLaugh()
    {
        currentAttack.StopAttack();
    }
}
