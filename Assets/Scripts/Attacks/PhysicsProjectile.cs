using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a class instead of a struct because I want it to be passed over by reference instead of by value
// As per the flyweight pattern to condense a bunch of useless data down into
[System.Serializable]
public class PhysicsProjectileData {
    public int damage = 1;
    public float force = 700;
    public float maxLifetime = 6;
    public string targetTag = "Enemy";
}

public class PhysicsProjectile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rendererForSprite;
    [SerializeField] private GameObject hitParticle;
    private PhysicsProjectileData data;
    private float lifetime = 0;
    private Vector3 initalSize; 

    private void SetupSpawnable(SimpleObjectPool.IPoolEvents events) {
        events.OnSpawn += OnRespawn;
    }

    private void OnRespawn(object sender, bool initalSpawn) {
        if (initalSpawn) 
        {
            initalSize = transform.localScale;
            if (rendererForSprite == null) 
            {
                rendererForSprite = GetComponent<SpriteRenderer>();
            }
        }
        SetFade(1);
    }

    public void SetupProjectile(PhysicsProjectileData projectileData, Vector3 direction) 
    {
        data = projectileData;
        if (gameObject.TryGetComponent(out Rigidbody2D rigidbody)) {
            rigidbody.AddForce(direction * data.force);
            lifetime = data.maxLifetime;
        }
    }

    void Update() 
    {
        lifetime -= Time.deltaTime;
        if (lifetime > 1) return;
        SetFade(lifetime);

        if (lifetime <= 0) {
            SimpleObjectPool.Despawn(gameObject);
        }
    }

    private void SetFade(float progress) {
        progress = Mathf.Clamp01(progress);
        // transform.localScale = initalSize * progress;
        
        Color color = rendererForSprite.color;
        color.a = progress;
        rendererForSprite.color = color;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        CollisionCheck(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionCheck(other.gameObject);
    }

    private void CollisionCheck(GameObject givenObject)
    {
        if (givenObject.tag == "KillBox") 
        {
            SimpleObjectPool.Despawn(gameObject);
            return;
        }
        // else if (other.gameObject.tag == "Ground") 
        // {
        //     // Disable Damage when after water hits the ground potentially
        // }
        else if (givenObject.tag != data.targetTag) 
        {
            return;
        }

        if (givenObject.TryGetComponent(out HealthComponent health)) 
        {
            health.TakeDamage(data.damage);
            AudioManager.Instance.Play("Water Pick Hit");
            SimpleObjectPool.Despawn(gameObject);
            GameObject feedback = SimpleObjectPool.Spawn(hitParticle, transform.position, transform.rotation);
            feedback.GetComponent<MonoBehaviour>().StartCoroutine(DelayDespawn(feedback, 3));
        }
    }

    private IEnumerator DelayDespawn(GameObject obj, float delay = 1f) 
    {
        if (delay < 1) delay = 1;
        yield return new WaitForSeconds(delay);
        SimpleObjectPool.Despawn(obj);
    }
}
