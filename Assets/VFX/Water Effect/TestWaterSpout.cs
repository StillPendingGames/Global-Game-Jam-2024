using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterSpout : MonoBehaviour
{
    public GameObject waterParticle;
    public GameObject originPoint;
   

    public float fireRate;
    public float thrust;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shootWater());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator shootWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            ShootWater();
        }
        
    }

    public void ShootWater()
    {
        GameObject g = Instantiate(waterParticle, originPoint.transform.position, originPoint.transform.rotation);
        g.GetComponent<Rigidbody2D>().AddForce(g.transform.up * thrust);
        //push it... up?
    }
}
