using UnityEngine;
using System.Collections;

public class UIItemBuilderFX : MonoBehaviour
{
    [SerializeField] TweenColor[] m_TweenColor;
    void OnEnable()
    {
        for (int i = 0; i < m_TweenColor.Length; i++)
        {
            m_TweenColor[i].Reset();
        }
    }
 
}
