using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TutorialTarget : MonoBehaviour
{
    public float health;
    
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            Destroy(collision.gameObject);
            health--;

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
