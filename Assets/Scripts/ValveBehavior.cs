using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "water") 
        {
            BossController.Instance.StartLaugh();
        } 
    }
}
