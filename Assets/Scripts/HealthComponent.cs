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
		HealthData healthData = new HealthData(health, maxHealth);
		OnChange?.Invoke(this, healthData);

		if (health <= 0) {
			OnDeath?.Invoke(this, System.EventArgs.Empty);
		}
	}
}