using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class StoreAnimation : MonoBehaviour 
{
    public BuildingLogicData BuildingLogicData { get; set; }
    tk2dSpriteAnimator m_SpriteAnimator;
    void Awake()
    {
        this.m_SpriteAnimator = this.GetComponent<tk2dSpriteAnimator>();
    }
	// Update is called once per frame
	void Update () 
    {
        this.OnStoreAnimation();
	}
    void OnStoreAnimation()
    { 
        if (this.BuildingLogicData != null)
        { 
            ResourceType resourceType = ResourceType.Gold;
            switch (this.BuildingLogicData.BuildingIdentity.buildingType)
            {
                case BuildingType.GoldStorage:
                    resourceType = ResourceType.Gold;
                    break;
                case BuildingType.FoodStorage:
                    resourceType = ResourceType.Food;
                    break;
            }
            float percentage = SystemFunction.GetCollectPercentage(this.BuildingLogicData, resourceType);
            
            string animationName = AnimationNameConstants.STORE_PERCENTAGE_20;
            if (percentage > ClientConfigConstants.Instance.Store20Criterion)
            {
                animationName = AnimationNameConstants.STORE_PERCENTAGE_40;
                if (percentage > ClientConfigConstants.Instance.Store40Criterion)
                {
                    animationName = AnimationNameConstants.STORE_PERCENTAGE_60;
                    if (percentage > ClientConfigConstants.Instance.Store60Criterion)
                    {
                        animationName = AnimationNameConstants.STORE_PERCENTAGE_80;
                        if (percentage > ClientConfigConstants.Instance.Store80Criterion)
                        {
                            animationName = AnimationNameConstants.STORE_PERCENTAGE_100;
                        }
                    }
                }
            }
            this.m_SpriteAnimator.Play(animationName);
        }
    }

}
