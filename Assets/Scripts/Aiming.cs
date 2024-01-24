using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;

    public void AimWithMouse(Vector2 direction)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(direction);
        Vector2 difference = mousePos - transform.position;
        Aim(difference);
    }

    public void Aim(Vector2 direction)
    {
        Vector2 inputDirection = new Vector2(direction.x, direction.y).normalized;
        float angleRad = (Mathf.Atan2(inputDirection.y, inputDirection.x)) * Mathf.Rad2Deg;
        float quantizedAngle = Mathf.Round(angleRad / 45.0f) * 45.0f;
        objectToRotate.rotation = Quaternion.Euler(0, 0, quantizedAngle);        
    }

}
