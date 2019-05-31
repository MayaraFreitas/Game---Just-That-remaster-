using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform m_Target;

    private void LateUpdate()
    {

        if (m_Target)
        {
            transform.position = m_Target.position;
        }
    }
}
