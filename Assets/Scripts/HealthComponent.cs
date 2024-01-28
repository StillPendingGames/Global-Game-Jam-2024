using System.Collections.Generic;
using UnityEngine;

public class HealthData: System.EventArgs {
	public HealthData(int current, int max) {
		this.current = current;
		this.max = max;
	}

	public int current;
	public int max;
}

public class HealthComponent : MonoBehaviour {

	[SerializeField][Range(3, 100)] private int maxHealth = 10;
	[SerializeField][Range(0, 5)] private float invincibilityDuration = 1f;

	[SerializeField] private new List<SpriteRenderer> renderers;
	private float alphaFlash = 0.5f;
	private float invincibility = 0;

	private int health;
	public int Health 
	{
		get { return health; }
	}

	public event System.EventHandler OnDeath;
	public event System.EventHandler<HealthData> OnChange;

	private void SetupSpawnable(SimpleObjectPool.IPoolEvents events) 
	{
		events.OnSpawn += OnRespawn;
	}

	private void OnRespawn(object sender, bool initalSpawn) 
	{
		health = maxHealth;
	}

	private void Awake() {
		health = maxHealth;
	}

	private void Update() {
		if (invincibility > 0) {
			invincibility -= Time.deltaTime;
		}
	}

	public void TakeDamage(int damage) {
		if (health <= 0 || invincibility > 0) return;
		health = Mathf.Max(health - damage, 0);
		invincibility = invincibilityDuration;
		StartCoroutine(OnHitFlash(invincibilityDuration));

		HealthData healthData = new HealthData(health, maxHealth);
		OnChange?.Invoke(this, healthData);

		if (health <= 0) {
			OnDeath?.Invoke(this, System.EventArgs.Empty);
		}
	}

	public System.Collections.IEnumerator OnHitFlash(float duration) 
	{
		if (renderers.Count <= 0) yield break;

		while (duration > 0) 
		{
			float alpha = renderers[0].color.a == alphaFlash ? 1 : alphaFlash;
			SetAlpha(alpha);
			float pause = duration < 0.1f ? duration : 0.1f;
			yield return new WaitForSeconds(pause);
			duration -= pause;
		}

		SetAlpha(1);
	}

	private void SetAlpha(float alpha) 
	{
		Color color = Color.white;
		color.a = alpha;

		for (int i = 0; i < renderers.Count; i++) 
		{
			renderers[i].color = color;
		}
	}
}