using UnityEngine;
using System.Collections;

public class StoreCommon : MonoBehaviour {
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
    const string ANIMATION_PREFAB = "BuildingBackgroundAnchor/BuildingBackground/AnimatedSprite";
    protected Transform animatedSpriteTrans;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected virtual void Store(int capacity)
    {
        
    }
    protected void GetAnimationComponent()
    {
        this.animatedSpriteTrans = this.transform.FindChild(ANIMATION_PREFAB);
        if (animatedSpriteTrans != null)
        {
            StoreAnimation storeAnimation = animatedSpriteTrans.GetComponent<StoreAnimation>();
            if (storeAnimation != null) 
                storeAnimation.BuildingLogicData = this.BuildingLogicData;
        }
    }
}
