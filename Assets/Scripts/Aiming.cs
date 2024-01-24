using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;

    public void Aim(Vector2 direction)
    {
        Debug.Log(direction);
        Vector2 aimDirection = direction.normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        objectToRotate.rotation = Quaternion.Euler(0, 0, angle);
    }
}
