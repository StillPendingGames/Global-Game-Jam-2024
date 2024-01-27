using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DamageData 
{
    public int damage = 1;
    public string targetTag = "Player";
}

// Outlines the IAttack 
public abstract class IAttack : MonoBehaviour
{
    [SerializeField] protected float duration;
    // protected float currentTime;

    // On Attack Finished sends the cooldown value (subject to change)
    public event System.EventHandler OnAttackFinished;

    // public abstract void SetupAttack(GameObject attacker);

    public virtual void StartAttack()
    {
        StartCoroutine(Timer());
    }

    public virtual void StopAttack()
    {
        StopCoroutine(Timer());
    }

    protected void SendOnAttackFinished() => OnAttackFinished(this, EventArgs.Empty);

    protected IEnumerator Timer()
    {
        yield return new WaitForSeconds(duration);
        StopAttack();
        SendOnAttackFinished();
    }
}
