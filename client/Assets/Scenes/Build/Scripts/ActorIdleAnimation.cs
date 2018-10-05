using UnityEngine;
using System.Collections;
using System;

public class ActorIdleAnimation : MonoBehaviour 
{
 
    BuildingIdleAnimation m_BuildingIdleAnimation = BuildingIdleAnimation.IdleRightDown;
    tk2dSpriteAnimator m_SpriteAnimator;
    [SerializeField]int m_TimeTickInterval = 100;
    int m_CurrentTick;
    
	// Use this for initialization
 
    void Start()
    {
		m_SpriteAnimator = this.GetComponent<tk2dSpriteAnimator>();
        if (m_SpriteAnimator != null)
            m_SpriteAnimator.Play(this.m_BuildingIdleAnimation.ToString());
    }
	
	// Update is called once per frame
	void Update () {
        this.PlayIdleAnimation();
	
	}
    void PlayIdleAnimation()
    {
        if (m_SpriteAnimator != null)
        {
            this.m_CurrentTick++;
            if (this.m_CurrentTick >= this.m_TimeTickInterval)
            {
                this.m_CurrentTick = 0;
                System.Random random = new System.Random(this.GetHashCode() + (int)Time.time);
                int randomValue = random.Next(-1, 2);
                int resultValue = (int)this.m_BuildingIdleAnimation + randomValue;
                if (resultValue > Enum.GetValues(typeof(BuildingIdleAnimation)).Length)
                    resultValue = 1;
                if (resultValue < 1)
                    resultValue = Enum.GetValues(typeof(BuildingIdleAnimation)).Length;
                m_BuildingIdleAnimation = (BuildingIdleAnimation)resultValue;
                m_SpriteAnimator.Play(this.m_BuildingIdleAnimation.ToString());
            }

        }

    }
}
