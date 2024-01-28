using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTarget : MonoBehaviour
{
    public float health;
    public GameObject waterParticle;
    
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            SimpleObjectPool.Despawn(collision.gameObject);
            GameObject g = Instantiate(waterParticle, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            health--;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
