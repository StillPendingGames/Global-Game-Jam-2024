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

	[SerializeField][Range(3, 100)] private int m_maxHealth = 10;
	private int m_health;
	public int Health {
		get { return m_health; }
	}

	public event System.EventHandler OnDeath;
	public event System.EventHandler<HealthData> OnChange;

	private void Awake() {
		m_health = m_maxHealth;
	}

	public void TakeDamge(int damage) {
		if (m_health <= 0) return;
		m_health = Mathf.Max(m_health - damage, 0);

		HealthData healthData = new HealthData(m_health, m_maxHealth);
		OnChange.Invoke(this, healthData);

		if (m_health <= 0) {
			OnDeath.Invoke(this, System.EventArgs.Empty);
		}
	}
}