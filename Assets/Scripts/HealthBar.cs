using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthComponent targetHealth;
    [SerializeField] private List<Image> healthIcons;

    // Start is called before the first frame update
    void Awake()
    {
        targetHealth.OnChange += OnHealthUpdate;
    }


    void OnHealthUpdate(object sender, HealthData data)
    {
        for (int i = data.current; i < healthIcons.Count; i++)
        {
            if (!healthIcons[i].enabled) return;
            healthIcons[i].enabled = false;
        }
    }
}
