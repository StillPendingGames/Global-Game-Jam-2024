using UnityEngine;

// Outlines the IAttack 
public abstract class IAttack : MonoBehaviour
{

    // On Attack Finished sends the cooldown value (subject to change)
    public event System.EventHandler<float> OnAttackFinished;

    // public abstract void SetupAttack(GameObject attacker);

    // pass in the attacker
    public abstract void StartAttack(/* GameObject attacker */);

    protected void SendOnAttackFinished(float cooldown) => OnAttackFinished(this, cooldown);
}
