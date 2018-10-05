using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class IntroConstructBarracks : NewbieGuide
{
    public override void OnIntroConstructBarracks()
    {
        if (base.ConditionExistBuilding(BuildingType.Barracks))
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
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[8.1f]);
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

                    base.SetRollPanelLocalPosition(NewbieGuideManager.Instance.WindowUIController[10], 0);

                    base.ActiveCollider(UIManager.Instance.UIWindowBuyBuilding.gameObject, false);
                    base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyBuilding.gameObject, true);
                    base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[11], true);
                    base.ActiveBuildingInfoCollider(NewbieGuideManager.Instance.WindowUIController[11], false);
                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[11], ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[11]);
                    UIBuyBuilding uiBuyBuilding = NewbieGuideManager.Instance.WindowUIController[11].GetComponent<UIBuyBuilding>();
                    uiBuyBuilding.enabled = false;
                    UIDragPanelContents uiDragPanelContents = NewbieGuideManager.Instance.WindowUIController[11].GetComponent<UIDragPanelContents>();
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
    public override void OnConstructBarracks()
    {
        if (!(LogicController.Instance.GetBuildingCount(BuildingType.Barracks) == 1 && base.ConditionExistBuildingLevel(BuildingType.Barracks, 0)))
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
        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.Barracks);
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            for (int i = 0; i < buildingLogicDataList.Count; i++)
            {
                if (buildingLogicDataList[i].BuildingType == BuildingType.Barracks && buildingLogicDataList[i].CurrentBuilidngState != BuildingEditorState.Normal)
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

    public override void OnProduceArmy()
    {
        
        if (LogicController.Instance.PlayerData.Honour != 1000 || LogicController.Instance.GetBuildingCount(BuildingType.ArmyCamp)==1 && LogicController.Instance.CurrentAvailableArmyCapacity == LogicController.Instance.CampsTotalCapacity)
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
        base.ActiveCollider(CameraManager.Instance.gameObject, false);
        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.BottomLeft, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[8.2f]);
        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(false);
        BuildingLogicData buildingLogicData = LogicController.Instance.GetBuildings(BuildingType.Barracks)[0]; 
       
        //NewbieGuideManager.Instance.UpdateEventNext.Enqueue(() =>
        BuildingBehavior buildingBehavior = null;
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column);
            return buildingBehavior != null;
        },
        () =>
        {
            base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
            base.ActiveCollider(buildingBehavior.gameObject, true);
            base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneConstructBuilding, ArrowDirection.Down);
            buildingBehavior.enabled = false;
            DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
            dynamicInvokeGuide.Click += () =>
            {
                buildingBehavior.enabled = true;
                buildingBehavior.OnClick();
                buildingBehavior.enabled = false;
                base.ActiveCollider(NewbieGuideManager.Instance.PopupController[0], false);
                base.ActiveCollider(NewbieGuideManager.Instance.PopupController[3], true);
                base.ActiveCollider(buildingBehavior.gameObject, false);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.PopupController[3], ArrowOffsetType.UITPopupButtonTrainTroops, ArrowDirection.Down);
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.PopupController[3]);
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveCollider(CameraManager.Instance.gameObject, true);
                    NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false);
                    base.ActiveCollider(UIManager.Instance.UIWindowBuyArmy.gameObject, false);
                    base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyArmy.gameObject, true);
                    //base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[17], true);
                    //base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[19], false);
                    base.ActiveColliderSelf(NewbieGuideManager.Instance.WindowUIController[17], true);
                    //17 = btn buy army ; 18 = btn buy army Finish now ;19 = btn buy army information
                    UIManager.Instance.UIWindowBuyArmy.WindowEvent += () =>
                    {
                        //base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[17], ArrowOffsetType.UIBuyArmy, ArrowDirection.Down);
                        base.DestroyGuideArrow();
                        BoxCollider boxColliderFinishNow = NewbieGuideManager.Instance.WindowUIController[18].GetComponent<BoxCollider>(); 
                        boxColliderFinishNow.enabled = false; 
                        dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[18]);
                        dynamicInvokeGuide.Click += () =>
                        {
                            UIManager.Instance.UIWindowCostPrompt.WindowEvent += () => base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[5], ArrowOffsetType.UICostGemImmediatelyButton, ArrowDirection.Down);
                            base.ActiveCollider(UIManager.Instance.UIWindowCostPrompt.gameObject, false);
                            base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[5].gameObject, true);
                            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[5].gameObject);
                            dynamicInvokeGuide.Click += () => base.ActiveCollider(UIManager.Instance.UIWindowCostPrompt.gameObject, true);

                        };
                        bool condition = false;
                        NewbieGuideManager.Instance.TriggerCondition(
                        () => 
                        {
                            if (LogicController.Instance.TotalArmyCapacity == LogicController.Instance.CampsTotalCapacity)
                            {
                                boxColliderFinishNow.enabled = true; 
                                if (condition)
                                {
                                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[18], ArrowOffsetType.UIProduceArmyImmediately, ArrowDirection.Up);
                                    condition = false;
                                }
                            }
                            else
                            {
                                if (LogicController.Instance.TotalArmyCapacity < LogicController.Instance.CampsTotalCapacity)
                                {
                                    if (!condition)
                                    {
                                        base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[17], ArrowOffsetType.UIBuyArmy, ArrowDirection.Down);
                                        condition = true;
                                    }
                                }
                                else
                                {
                                    base.DestroyGuideArrow();
                                    condition = true;
                                }
                                boxColliderFinishNow.enabled = false;
                            }
                            return LogicController.Instance.CurrentAvailableArmyCapacity == LogicController.Instance.CampsTotalCapacity;
                        },
                        () => 
                        {
                            if (UIManager.Instance.UIWindowCostPrompt.gameObject.activeSelf)
                            {
                                UIManager.Instance.UIWindowCostPrompt.HideWindow();
                            }
                            base.ActiveCollider(UIManager.Instance.UIWindowCostPrompt.gameObject, true);
                            base.RemoveDynamicGuide(NewbieGuideManager.Instance.WindowUIController[5].gameObject);
                            UIManager.Instance.UIWindowBuyArmy.HideWindow();
                            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
                            NewbieGuideManager.Instance.Waiting(100, () =>
                                {
                                    base.ResetAll();
                                    NewbieGuideManager.Instance.InvokeNextGuide();
                                }
                            );
                        });
                    };
                };
            };
        });
       

    }

}
