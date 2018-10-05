using UnityEngine;
using System.Collections;

public class Chrysanthemum : MonoBehaviour
{
    private Transform m_Transform;
    
    void Awake()
    {
        m_Transform = this.transform;
    }

    void FixedUpdate()
    {
        m_Transform.Rotate(Vector3.back * 18);
    }
}
