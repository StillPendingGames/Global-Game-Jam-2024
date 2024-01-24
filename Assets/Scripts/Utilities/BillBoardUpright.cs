using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardUpright : MonoBehaviour
{
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
    }
}
