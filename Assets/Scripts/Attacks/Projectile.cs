using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a class instead of a struct because I want it to be passed over by reference instead of by value
// As per the flyweight pattern to condense a bunch of useless data down into
[System.Serializable]
public class ProjectileData {
    public int damage;
    public int speed;
    public string targetTag;
}

public class Projectile : MonoBehaviour
{
    private ProjectileData m_data;
    private Vector3 m_direction;

    public void SetupProjectile(ProjectileData data, Vector2 direction) 
    {
        m_data = data;
        m_direction = direction;
    }

    private void FixedUpdate()
    {
        // Move Projectile
        transform.position += m_direction * m_data.speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "KillBox") 
        {
            SimpleObjectPool.Despawn(gameObject);
            return;
        } 
        else if (other.gameObject.tag != m_data.targetTag) 
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out HealthComponent health)) 
        {
            health.TakeDamage(m_data.damage);
            SimpleObjectPool.Despawn(gameObject);
        }
    }
}
