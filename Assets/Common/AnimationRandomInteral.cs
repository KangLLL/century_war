using UnityEngine;
using System.Collections;

public class AnimationRandomInteral : MonoBehaviour
{
    [SerializeField]
    private int m_MinTimeTick;
    [SerializeField]
    private int m_MaxTimeTick;

    private int m_CurrentTimeTick;

    private tk2dSpriteAnimator m_SpriteAnimator;
    // Use this for initialization
    void Awake()
    {
        m_SpriteAnimator = this.GetComponent<tk2dSpriteAnimator>();
        m_CurrentTimeTick = Random.Range(m_MinTimeTick, m_MaxTimeTick);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_CurrentTimeTick <= 0)
        {
            m_SpriteAnimator.Play();
            m_CurrentTimeTick = Random.Range(m_MinTimeTick, m_MaxTimeTick);
        }
        m_CurrentTimeTick--;
    }
}
