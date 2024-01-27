using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleCollisionTest : MonoBehaviour
{
    public GameObject waterVFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject g = Instantiate(waterVFX, transform.position, transform.rotation);

        ParticleSystem p = g.GetComponent<ParticleSystem>();

        p.Play();
    }
}
