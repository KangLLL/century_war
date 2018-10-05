using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class GoldMineBehavior : BuildingCommon
{
    const string ANIMATION_PREFAB = "BuildingBackgroundAnchor/BuildingBackground/AnimatedSprite";
    Transform animatedSpriteTrans;
    protected override void Start()
    { 
        base.Start();
        this.animatedSpriteTrans = this.transform.FindChild(ANIMATION_PREFAB);
    }
    void Update()
    {
        this.ShowProgress();
        base.OnReadyForUpgradeFx();
        this.OnActiveGoldMineAnimation();
    }
    protected override void ShowProgress()
    {
        base.ShowProgress();
    }


    void OnActiveGoldMineAnimation()
    {
        if (base.BuildingBehavior.Created)
            if (base.BuildingLogicData.BuildingType == BuildingType.GoldMine)
                if (this.animatedSpriteTrans != null)
                    this.animatedSpriteTrans.gameObject.SetActive(base.BuildingLogicData.CurrentBuilidngState != BuildingEditorState.Update);
    }
}
