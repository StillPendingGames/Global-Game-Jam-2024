using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKillTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject waterFeedbackParticle;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            
            GameObject g = Instantiate(waterFeedbackParticle, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            Destroy(collision.gameObject);

           
        }
    }




}
