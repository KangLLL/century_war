using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Collections.Generic;

public class IntroUpgradeCityHall : NewbieGuide
{
    public override void OnIntroUpgradeCityHall()
    {
        if (!base.ConditionExistBuildingLevel(BuildingType.CityHall, 1) || !base.ConditionExistBuildingState(BuildingType.CityHall, BuildingEditorState.Normal))
        { 
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        } 
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);
        BuildingLogicData buildingLogicData = LogicController.Instance.GetBuildings(BuildingType.CityHall)[0];
        BuildingBehavior buildingBehavior = null;
        NewbieGuideManager.Instance.TriggerCondition(() => { return (buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column)) != null; },
        () =>
        {
            base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
            base.HightlightController(buildingBehavior.gameObject);
            base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneSelectBuilding, ArrowDirection.Down);
            buildingBehavior.enabled = false;
           
            DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
            dynamicInvokeGuide.Click += () =>
            {
                buildingBehavior.enabled = true;
                buildingBehavior.OnClick();
                buildingBehavior.enabled = false;
                base.ActiveCollider(buildingBehavior.gameObject, false);

                base.ActiveCollider(CameraManager.Instance.gameObject, false);
                base.ActiveCollider(NewbieGuideManager.Instance.PopupController[0], false);
                base.ActiveCollider(NewbieGuideManager.Instance.PopupController[2], true);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.PopupController[2], ArrowOffsetType.UIPopupButtonUpgrade, ArrowDirection.Down);
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.PopupController[2]);
                dynamicInvokeGuide.Click += () =>
                {
                    base.UnHightlightController(NewbieGuideManager.Instance.PopupController[2]);
                    base.ActiveCollider(UIManager.Instance.UIWindowUpagradeBuilding.gameObject, false);
                    UIManager.Instance.UIWindowUpagradeBuilding.WindowEvent += () =>
                    {
                        base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[16].gameObject, true);
                        UIItemBuilder uiItemBuilder = NewbieGuideManager.Instance.WindowUIController[16].GetComponent<UIItemBuilder>();
                        uiItemBuilder.enabled = false;
                        base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[16], ArrowOffsetType.UISelectBuilderButton2, ArrowDirection.Down);
                        dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[16]);
                        dynamicInvokeGuide.Click += () =>
                        {
                            uiItemBuilder.enabled = true;
                            uiItemBuilder.OnClick();
                            base.ActiveCollider(UIManager.Instance.UIWindowUpagradeBuilding.gameObject, true);
                            base.ResetAll();
                            NewbieGuideManager.Instance.InvokeNextGuide();
                        };
                    };
                };
            };
        });
    }
    public override void OnUpgradeCityHall()
    {
        if (!base.ConditionExistBuildingLevel(BuildingType.CityHall, 1) || base.ConditionExistBuildingState(BuildingType.CityHall, BuildingEditorState.Normal))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIActive(false);
        base.SetAllBuildingActive(false);

        BuildingLogicData buildingLogicData = null;
        BuildingBehavior buildingBehavior = null;
        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.CityHall);
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            for (int i = 0; i < buildingLogicDataList.Count; i++)
            {
                if (buildingLogicDataList[i].BuildingType == BuildingType.CityHall && buildingLogicDataList[i].CurrentBuilidngState != BuildingEditorState.Normal)
                {
                    buildingLogicData = buildingLogicDataList[i];
                    break;
                }
            } 
            if (buildingLogicData != null)
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column);
            return buildingBehavior != null;
        },
        () =>
        {
            base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
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
                    //base.UnHightlightController(UIManager.Instance.UIWindowCostPrompt.gameObject);
                    NewbieGuideManager.Instance.Waiting(90, () =>
                    {
                        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[11.1f]);
                        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(true);
                        dynamicInvokeGuide = AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
                        dynamicInvokeGuide.Click += () =>
                        {
                            NewbieGuideManager.Instance.Waiting(5, () =>
                            {
                                base.ResetAll();
                                NewbieGuideManager.Instance.InvokeNextGuide();
                            });
                        };
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
            buildingBehavior.enabled = true;
            base.ActiveCollider(CameraManager.Instance.gameObject, true);
            if (UIManager.Instance.UIWindowCostPrompt.gameObject.activeSelf)
            {
                UIManager.Instance.UIWindowCostPrompt.HideWindow();
            }
            base.RemoveDynamicGuide(NewbieGuideManager.Instance.PopupController[1]);
            base.RemoveDynamicGuide(NewbieGuideManager.Instance.WindowUIController[5]);
            base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneBuildingOkButton, ArrowDirection.Down);
            DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
            dynamicInvokeGuide.Click += () =>
            {
                buildingBehavior.enabled = false;
                base.ActiveSceneLayerUICameraInput(false);
                NewbieGuideManager.Instance.Waiting(90, () =>
                {
                    NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[11.1f]);
                    NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                    NewbieGuideManager.Instance.UIWindowGuide.SetClickState(true);
                    dynamicInvokeGuide = AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
                    dynamicInvokeGuide.Click += () =>
                    {
                        base.ActiveSceneLayerUICameraInput(false);
                        NewbieGuideManager.Instance.Waiting(5, () =>
                        {
                            base.ResetAll();
                            NewbieGuideManager.Instance.InvokeNextGuide();
                        });
                    };
                });
            };
        });
    }
}
