using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAttack : IAttack
{
    [SerializeField] private DamageData damageData;
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform bassMouth;
    [SerializeField] private float heightOffset = 2;

    private Hook introHook;
    private Hook mainHook;

    // Start is called before the first frame update
    void Awake()
    {
        Transform parent = new GameObject("Hooks").transform;
        GameObject introObj = SimpleObjectPool.Spawn(hookPrefab, bassMouth.position, parent);
        introHook = introObj.GetComponent<Hook>();
        introObj.SetActive(false);

        GameObject mainObj = SimpleObjectPool.Spawn(hookPrefab, bassMouth.position, parent);
        mainHook = mainObj.GetComponent<Hook>();
        mainObj.SetActive(false);

        introHook.SetDamageData(damageData);
        mainHook.SetDamageData(damageData);
    }

    public override void StartAttack() 
    {
        float introDuration = (duration * 0.25f) * 0.5f;

        HookAnimationData introData = new HookAnimationData();
        introData.origin = bassMouth.position;
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
