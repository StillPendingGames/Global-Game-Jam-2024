using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private IAttack[] attacks;
    [SerializeField] protected float cooldownBetweenAttacks;
    [SerializeField] private GameObject valve;
    [SerializeField] private Vector3 valveTargetRotation = new Vector3(0f, 0f, 45f);
    [SerializeField] private float valveRotationSpeed = 1.0f;
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
        //currentAttack.StopAttack();
        if (!laughing)
        {
            laughing = true;
            teethCollider.enabled = false;
            StartCoroutine(HideValveCo());
            Debug.Log("Activating Laugh Gas");
            StartCoroutine(Laughing());
        }

    }

    private IEnumerator Laughing()
    {
        animator.Play("BossStartLaugh");
        yield return new WaitForSeconds(laughTransitionAnimation.length);
        teethCollider.enabled = true;
    }

    public void StopLaughing()
    {
        if (laughing)
        {
            teethCollider.enabled = false;
            animator.Play("BossEndLaugh");
            laughing = false;
            Debug.Log("Laughing Ended");
        }
    }

    private void OnDeath(object sender, System.EventArgs args)
    {
        // Boss has died show complete scene
        winScreen.SetActive(true);
        Debug.Log("Boss Died");
    }

    public IEnumerator ShowValve()
    {
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

    public void HideValve()
    {
        StartCoroutine(HideValveCo());
    }

    private IEnumerator HideValveCo()
    {
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
