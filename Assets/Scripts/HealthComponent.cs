using UnityEngine;

public class HealthComponent : MonoBehaviour {

	[SerializeField][Range(3, 100)] private float m_maxHealth = 10;
	private float m_health;
	public float Health {
		get { return m_health; }
	}

	public event System.EventHandler OnDeath;
	public event System.EventHandler<float> OnUpdate;

	private void Awake() {
		m_health = m_maxHealth;
	}

	public void TakeDamge(float damage) {
		m_health -= damage;
		OnUpdate.Invoke(this, Mathf.Max(m_health / m_maxHealth, 0));
		if (m_health <= 0) {
			OnDeath.Invoke(this, System.EventArgs.Empty);
		}
	}
}