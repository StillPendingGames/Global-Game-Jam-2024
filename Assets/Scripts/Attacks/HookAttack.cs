using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAttack : IAttack
{
    [SerializeField] private DamageData damageData;
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private Hook introHook;
    [SerializeField] private Hook mainHook;
    [SerializeField] private float heightOffset = 2;

    // Start is called before the first frame update
    void Awake()
    {
        introHook.gameObject.SetActive(false);
        mainHook.gameObject.SetActive(false);

        introHook.SetDamageData(damageData);
        mainHook.SetDamageData(damageData);
    }

    public override void StartAttack() 
    {
        animator.Play("Spit");
        float introDuration = (duration * 0.25f) * 0.5f;

        HookAnimationData introData = new HookAnimationData();
        introData.origin = transform.position;
        introData.destination = introData.origin + Vector3.left * 10;
        introData.duration = introDuration;

        introHook.OnComplete += OnIntroComplete;
        introHook.gameObject.SetActive(true);
        introHook.SetupHook(introData);
    }

    private void OnIntroComplete(object sender, System.EventArgs args)
    {
        float mainDuration = (duration * 0.75f) * 0.5f;
        introHook.OnComplete -= OnIntroComplete;
        int sign = Random.Range(0f, 1f) > 0.5f ? 1 : -1; 
        float xOffset = 10 * sign;

        mainHook.transform.localScale = new Vector3(sign, 1, 1);
        HookAnimationData mainData = new HookAnimationData();
        mainData.origin = new Vector3(xOffset + (2 * sign), -heightOffset, 0);
        mainData.destination = new Vector3(-xOffset, -heightOffset, 0);
        mainData.duration = mainDuration;

        mainHook.OnComplete += OnMainAttackFinished;
        mainHook.gameObject.SetActive(true);
        mainHook.SetupHook(mainData);
    }

    private void OnMainAttackFinished(object sender, System.EventArgs args) 
    {
        mainHook.OnComplete -= OnMainAttackFinished;
        mainHook.OnComplete += OnMainHookRecalled;
        mainHook.Restart();
    }

    private void OnMainHookRecalled(object sender, System.EventArgs args) 
    {
        mainHook.OnComplete -= OnMainHookRecalled;
        mainHook.gameObject.SetActive(false);

        introHook.OnComplete += OnHookAttackComplete;
        introHook.Restart();
    }

    private void OnHookAttackComplete(object sender, System.EventArgs args)
    {
        introHook.OnComplete -= OnHookAttackComplete;
        introHook.gameObject.SetActive(false);
        StopAttack();
    }

    public override void StopAttack()
    {
        SendOnAttackFinished();
    }
}
