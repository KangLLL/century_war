﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class IntroConstructArmyCamp : NewbieGuide
{
    public override void OnIntroConstructArmyCamp()
    {
        if (base.ConditionExistBuilding(BuildingType.ArmyCamp))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[6].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);


        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[7.1f]);
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () =>
        {
            base.SetAllMainUIColorFull(false);
            base.SetAllBuildingColorHightlight(false);
            base.HightlightController(SceneManager.Instance.AgeMap, false);
            base.ActiveCollider(CameraManager.Instance.gameObject, false);
            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
            base.HightlightController(NewbieGuideManager.Instance.MainUIController[3]);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[3], ArrowOffsetType.UIBuildingButton, ArrowDirection.Down);
            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.MainUIController[3]);
            dynamicInvokeGuide.Click += () =>
            {
                base.ActiveCollider(NewbieGuideManager.Instance.MainUIController[3], false);
                base.ActiveCollider(CameraManager.Instance.gameObject, true);
                //base.UnHightlightController(NewbieGuideManager.Instance.MainUIController[3]); 
                NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false); 
                base.ActiveCollider(UIManager.Instance.UIConstructBuilding.gameObject, false);
                base.ActiveColliderSelf(UIManager.Instance.UIConstructBuilding.gameObject, true);
                base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[8], true);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[8], ArrowOffsetType.UIMilitaryMenu, ArrowDirection.Right);
                //construct building window : buy resource button
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[8]);
                UISelectMenu uiSelectMenu = NewbieGuideManager.Instance.WindowUIController[8].GetComponent<UISelectMenu>();
                uiSelectMenu.enabled = false;
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveCollider(UIManager.Instance.UIConstructBuilding.gameObject, true);
                    uiSelectMenu.enabled = true;
                    uiSelectMenu.OnClick();

                    base.SetRollPanelLocalPosition(NewbieGuideManager.Instance.WindowUIController[10], 1);

                    base.ActiveCollider(UIManager.Instance.UIWindowBuyBuilding.gameObject, false);
                    base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyBuilding.gameObject, true);
                    base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[9], true);
                    base.ActiveBuildingInfoCollider(NewbieGuideManager.Instance.WindowUIController[9], false);
                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[9], ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[9]); 
                    UIBuyBuilding uiBuyBuilding = NewbieGuideManager.Instance.WindowUIController[9].GetComponent<UIBuyBuilding>();
                    uiBuyBuilding.enabled = false;
                    UIDragPanelContents uiDragPanelContents = NewbieGuideManager.Instance.WindowUIController[9].GetComponent<UIDragPanelContents>();
                    uiDragPanelContents.enabled = false;
                    dynamicInvokeGuide.Click += () =>
                    {
                        base.ActiveCollider(UIManager.Instance.UIWindowBuyBuilding.gameObject, true); 

                        uiBuyBuilding.enabled = true;
                        uiDragPanelContents.enabled = true;
                        uiBuyBuilding.OnClick();
                        if (!uiBuyBuilding.CheckLock())
                            return;
                        BuildingBehavior buildingBehavior = SceneManager.Instance.BuildingBehaviorTemporary;
                        NewbieGuideManager.Instance.TriggerCondition(() => { return buildingBehavior.ButtonOk != null; }, 
                        () =>
                        {
                            base.UnHightlightController(buildingBehavior.ButtonCancel);
                            base.CreateSceneGuideArrow(buildingBehavior.ButtonOk, ArrowOffsetType.SceneBuildingOkButton, ArrowDirection.Down);
                            dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.ButtonOk);
                            dynamicInvokeGuide.Click += () =>
                            {
                                base.ActiveCollider(UIManager.Instance.UIWindowSelectBuilder.gameObject, false);
                                UIManager.Instance.UIWindowSelectBuilder.gameObject.GetComponent<BoxCollider>().enabled = true;
                                UIManager.Instance.UIWindowSelectBuilder.WindowEvent += () =>
                                {
                                    base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[2], true);
                                    UIItemBuilder uiItemBuilder = NewbieGuideManager.Instance.WindowUIController[2].GetComponent<UIItemBuilder>();
                                    uiItemBuilder.enabled = false;
                                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[2], ArrowOffsetType.UISelectBuilderButton1, ArrowDirection.Left);
                                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[2]);
                                    dynamicInvokeGuide.Click += () =>
                                    {
                                        base.ActiveCollider(UIManager.Instance.UIWindowSelectBuilder.gameObject, true);
                                        uiItemBuilder.enabled = true;
                                        uiItemBuilder.OnClick();
                                        base.ResetAll();
                                        NewbieGuideManager.Instance.InvokeNextGuide();
                                    };
                                };
                            };
                        });
                    };
                };
            };
        };
    }
    public override void OnConstructArmyCamp()
    {
        if (!(LogicController.Instance.GetBuildingCount(BuildingType.ArmyCamp) == 1 && base.ConditionExistBuildingLevel(BuildingType.ArmyCamp, 0)))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[6].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIActive(false);
        base.SetAllBuildingActive(false);
        BuildingLogicData buildingLogicData = null;
        BuildingBehavior buildingBehavior = null;
        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.ArmyCamp);
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            for (int i = 0; i < buildingLogicDataList.Count; i++)
            {
                if (buildingLogicDataList[i].BuildingType == BuildingType.ArmyCamp && buildingLogicDataList[i].CurrentBuilidngState != BuildingEditorState.Normal)
                {
                    buildingLogicData = buildingLogicDataList[i];
                    break;
                }
            } 
            if (buildingLogicData != null)
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column);
            return  buildingBehavior != null;
        },
        () =>
        {
            base.ActiveCollider(buildingBehavior.gameObject, false);
            base.ActiveCollider(CameraManager.Instance.gameObject, false);
            switch (buildingLogicData.CurrentBuilidngState)
            {
                case BuildingEditorState.Update:
                    this.OnUpdate(buildingBehavior);
                    this.OnReadyForUpdate(buildingBehavior);
                    break;
                case BuildingEditorState.ReadyForUpdate:
                    this.OnReadyForUpdate(buildingBehavior);
                    break;
            }
        });
    }
    void OnUpdate(BuildingBehavior buildingBehavior)
    {
        base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
        BuildingLogicData buildingLogicData = buildingBehavior.BuildingLogicData;
        NewbieGuideManager.Instance.TriggerCondition(() => { return buildingLogicData.CurrentBuilidngState == BuildingEditorState.Update; }, 
        () =>
        {
            SceneManager.Instance.PickableObjectCurrentSelect = buildingBehavior;
            UIManager.Instance.ShowPopupBtnByCurrentSelect();
            base.ActiveCollider(NewbieGuideManager.Instance.PopupController[0], false);
            base.ActiveCollider(NewbieGuideManager.Instance.PopupController[1], true);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.PopupController[1], ArrowOffsetType.UIPopupButtonImmediately, ArrowDirection.Down);
            DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.PopupController[1]);
            dynamicInvokeGuide.Click += () =>
            {
                UIManager.Instance.UIWindowCostPrompt.WindowEvent += () => base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[5], ArrowOffsetType.UICostGemImmediatelyButton, ArrowDirection.Down);
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[5]);
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveSceneLayerUICameraInput(false);
                    NewbieGuideManager.Instance.ClearEventQueue();
                    NewbieGuideManager.Instance.Waiting(90, () =>
                    {
                        base.ResetAll();
                        NewbieGuideManager.Instance.InvokeNextGuide();
                    });
                };
            };
        });
    }
    void OnReadyForUpdate(BuildingBehavior buildingBehavior)
    {
        base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
        BuildingLogicData buildingLogicData = buildingBehavior.BuildingLogicData;
        NewbieGuideManager.Instance.TriggerCondition(() => { return buildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate; }, 
        () =>
        {
            base.ActiveCollider(buildingBehavior.gameObject, true);
            base.ActiveCollider(CameraManager.Instance.gameObject, true);
            if (UIManager.Instance.UIWindowCostPrompt.gameObject.activeSelf)
            {
                UIManager.Instance.UIWindowCostPrompt.HideWindow();
            }
            base.RemoveDynamicGuide(NewbieGuideManager.Instance.PopupController[1]);
            base.RemoveDynamicGuide(NewbieGuideManager.Instance.WindowUIController[5]);
            base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneConstructBuilding, ArrowDirection.Down);
            DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
            dynamicInvokeGuide.Click += () =>
            {
                base.ActiveSceneLayerUICameraInput(false);
                NewbieGuideManager.Instance.Waiting(90, () =>
                {
                    base.ResetAll();
                    NewbieGuideManager.Instance.InvokeNextGuide();
                });
            };
        });
    }
}
