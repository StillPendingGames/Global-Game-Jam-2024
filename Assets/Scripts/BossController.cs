using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private IAttack[] attacks;
    [SerializeField] protected float cooldownBetweenAttacks;
    [SerializeField] private GameObject valve;
    [SerializeField] private Vector3 valveTargetRotation = new Vector3(0f, 0f, 45f);
    [SerializeField] private float valveRotationSpeed = 1.0f;
    [SerializeField] private GameObject gasRoomObject;
    [SerializeField] private Material gasMaterial;
    [SerializeField] private float  gasFillSpeed;
    [SerializeField] private float  gasMaxAlpha;
    [SerializeField] private GameObject gasShootObject;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip laughTransitionAnimation;
    [SerializeField] private Collider2D teethCollider;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private GameObject winScreen;
    public static BossController Instance;
    private IAttack currentAttack;
    private bool laughing;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (IAttack attack in attacks)
        {
            attack.OnAttackFinished += CurrentAttackEnded;
        }
    }

    private void Start()
    {

        healthComponent.OnDeath += OnDeath;
        StartFight();
        valve.SetActive(false);
        teethCollider.enabled = false;
        gasRoomObject.SetActive(false);
        gasShootObject.SetActive(false);
    }

    public void StartFight()
    {
        StartCoroutine(Cooldown(3));
        AudioManager.Instance.StopCurrentSongPlayNew("Blue Water Blues - Bahs");
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
        //currentAttack.StopAttack();
        if (!laughing)
        {
            Debug.Log("Activating Laugh Gas");
            laughing = true;
            teethCollider.enabled = false;
            StartCoroutine(HideValveCo());
            gasShootObject.SetActive(true);
            StartCoroutine(Laughing());
            StartCoroutine(ShowRoomGas());
            AudioManager.Instance.Play("Laugh");
        }

    }

    private IEnumerator Laughing()
    {
        animator.Play("BossStartLaughPlaque");
        yield return new WaitForSeconds(laughTransitionAnimation.length);
        teethCollider.enabled = true;
    }

    public void StopLaughing()
    {
        if (laughing)
        {
            teethCollider.enabled = false;
            animator.Play("BossEndLaughPlaque");
            gasShootObject.SetActive(false);
            StartCoroutine(FadeRoomGas());
            AudioManager.Instance.Stop("Laugh");
            laughing = false;
            Debug.Log("Laughing Ended");
        }
    }

    private void OnDeath(object sender, System.EventArgs args)
    {
        winScreen.SetActive(true);
        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.Play("Fresh and Shiny");
        Time.timeScale = 0;
        player.DisableInput();
        Debug.Log("Boss Died");
    }

    public IEnumerator ShowValve()
    {
        AudioManager.Instance.Play("Valve Appears");
        valve.GetComponent<Collider2D>().enabled = false;
        valve.SetActive(true);
        Quaternion targetQuaternion = Quaternion.Euler(valveTargetRotation);
        while(valve.transform.rotation != targetQuaternion)
        {
            valve.transform.rotation = Quaternion.Lerp(valve.transform.rotation, targetQuaternion, valveRotationSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        valve.GetComponent<Collider2D>().enabled = true;
    }

    public IEnumerator ShowRoomGas()
    {
        float gasAmount = 0;
        gasMaterial.SetFloat("_AlphaOne", gasAmount);
        gasRoomObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        while(gasAmount < gasMaxAlpha - 0.02f)
        {
            gasAmount = Mathf.Lerp(gasAmount, gasMaxAlpha, gasFillSpeed * Time.fixedDeltaTime);
            gasMaterial.SetFloat("_AlphaOne", gasAmount);
            yield return null;
        }
    }

    public IEnumerator FadeRoomGas()
    {
        float gasAmount = gasMaxAlpha;
        while(gasAmount > 0.02f)
        {
            gasAmount = Mathf.Lerp(gasAmount, 0, gasFillSpeed * Time.fixedDeltaTime);
            gasMaterial.SetFloat("_AlphaOne", gasAmount);
            yield return null;
        }
        gasAmount = 0;
        gasMaterial.SetFloat("_AlphaOne", gasAmount);
        gasRoomObject.SetActive(false);
    }

    public void HideValve()
    {
        StartCoroutine(HideValveCo());
    }

    private IEnumerator HideValveCo()
    {
        AudioManager.Instance.Play("Valve Hit");
        valve.GetComponent<Collider2D>().enabled = false;
        Vector3 oppositeRot = valveTargetRotation;
        oppositeRot.z = 0;
        Quaternion targetQuaternion = Quaternion.Euler(oppositeRot);
        while(valve.transform.rotation != targetQuaternion)
        {
            valve.transform.rotation = Quaternion.Lerp(valve.transform.rotation, targetQuaternion, valveRotationSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        valve.SetActive(false);
    }
}
