using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private IAttack attackManager;

    void Start()
    {
        if (TryGetComponent(out HealthComponent health)) 
        {
            health.OnDeath += OnDeath;
        }
    }

    void Update()
    {
        
    }

    private void OnDeath(object sender, System.EventArgs args)
    {
        // Boss has died show complete scene
        Debug.Log("Boss Died");
    }
}
