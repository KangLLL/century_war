using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using System.Linq;
public class IntroPropsStorage : NewbieGuide
{
    public override void OnIntroPropsStorage()
    {
        if (base.ConditionExistBuilding(BuildingType.PropsStorage))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);

        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.3f]);
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
                NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false);
                base.ActiveCollider(UIManager.Instance.UIConstructBuilding.gameObject, false);
                base.ActiveColliderSelf(UIManager.Instance.UIConstructBuilding.gameObject, true);
                base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[20], true);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[20], ArrowOffsetType.UIDefenseMenu, ArrowDirection.Left);
                //construct building window : buy resource button
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[20]);
                UISelectMenu uiSelectMenu = NewbieGuideManager.Instance.WindowUIController[20].GetComponent<UISelectMenu>();
                uiSelectMenu.enabled = false;
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveCollider(UIManager.Instance.UIConstructBuilding.gameObject, true);
                    uiSelectMenu.enabled = true;
                    uiSelectMenu.OnClick();

                    base.SetRollPanelLocalPosition(NewbieGuideManager.Instance.WindowUIController[21], 2);

                    base.ActiveCollider(UIManager.Instance.UIWindowBuyBuilding.gameObject, false);
                    base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyBuilding.gameObject, true);
                    base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[22], true);
                    base.ActiveBuildingInfoCollider(NewbieGuideManager.Instance.WindowUIController[22], false);
                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[22], ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[22]);
                    UIBuyBuilding uiBuyBuilding = NewbieGuideManager.Instance.WindowUIController[22].GetComponent<UIBuyBuilding>();
                    uiBuyBuilding.enabled = false;
                    UIDragPanelContents uiDragPanelContents = NewbieGuideManager.Instance.WindowUIController[22].GetComponent<UIDragPanelContents>();
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
    public override void OnConstructPropsStorage()
    {
        if (!(LogicController.Instance.GetBuildingCount(BuildingType.PropsStorage) == 1 && base.ConditionExistBuildingLevel(BuildingType.PropsStorage, 0)))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        base.SetAllMainUIActive(false);
        base.SetAllBuildingActive(false);
        BuildingLogicData buildingLogicData = null;
        BuildingBehavior buildingBehavior = null;

        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.PropsStorage);
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            for (int i = 0; i < buildingLogicDataList.Count; i++)
            {
                if (buildingLogicDataList[i].BuildingType == BuildingType.PropsStorage && buildingLogicDataList[i].CurrentBuilidngState != BuildingEditorState.Normal)
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
    public override void OnIntroPlant()
    {
        if (LogicController.Instance.AllProps.Count > 0  || base.ConditionPlant(RemovableObjectType.QualitySmallCactus))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);

        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.4f]);
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () =>
        {
            base.SetAllMainUIColorFull(false);
            base.SetAllBuildingColorHightlight(false);
            base.HightlightController(SceneManager.Instance.AgeMap, false);
            base.ActiveCollider(CameraManager.Instance.gameObject, false);
            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
            base.HightlightController(NewbieGuideManager.Instance.MainUIController[4]);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[4], ArrowOffsetType.UIBuildingButton, ArrowDirection.Down);
            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.MainUIController[4]);
            dynamicInvokeGuide.Click +=()=>
            {
                base.ActiveCollider(NewbieGuideManager.Instance.MainUIController[4], false);
                base.ActiveCollider(CameraManager.Instance.gameObject, true);
                //NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false);
                base.ActiveCollider(UIManager.Instance.UIWindowShop.gameObject, false);
                base.ActiveColliderSelf(UIManager.Instance.UIWindowShop.gameObject, true);
                base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[24], true);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[24], ArrowOffsetType.UIPlantMenu, ArrowDirection.Left);
                //construct plant window : buy plant button
                 dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[24]);
                UISelectShopMenu uiSelectShopMenu = NewbieGuideManager.Instance.WindowUIController[24].GetComponent<UISelectShopMenu>();
                uiSelectShopMenu.enabled = false;
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveCollider(UIManager.Instance.UIWindowShop.gameObject, true);
                    uiSelectShopMenu.enabled = true;
                    uiSelectShopMenu.OnClick();
                    NewbieGuideManager.Instance.TriggerCondition(() => { return UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem != null; },
                    () => 
                    {
                        base.SetRollPanelLocalPosition(UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem.gameObject, 1);

                        base.ActiveCollider(UIManager.Instance.UIWindowBuyTree.gameObject, false);
                        base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyTree.gameObject, true);
                        base.ActiveCollider(UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem.gameObject, true);
                        //base.ActiveBuildingInfoCollider(UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem.gameObject, false);
                        base.CreateUIGuideArrow(UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem.gameObject, ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                        dynamicInvokeGuide = base.AddDynamicGuide(UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem.gameObject);

                        UIBuyTreeItem uiBuyTreeItem = UIManager.Instance.UIWindowBuyTree.UIBuyTreeModule.UIBuyTreeItem;
                        uiBuyTreeItem.enabled = false;
                        UIDragPanelContents uiDragPanelContents = uiBuyTreeItem.GetComponent<UIDragPanelContents>();
                        uiDragPanelContents.enabled = false;
                        dynamicInvokeGuide.Click += () =>
                        {
                            base.ActiveCollider(UIManager.Instance.UIWindowBuyTree.gameObject, true);
                            uiBuyTreeItem.enabled = true;
                            uiDragPanelContents.enabled = true;
                            uiBuyTreeItem.OnClick();

                            base.ActiveCollider(UIManager.Instance.UIWindowBuyTreeChild.gameObject, false);
                            UIManager.Instance.UIWindowBuyTreeChild.gameObject.GetComponent<BoxCollider>().enabled = true;

                            base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[25], true);

                            base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[25], ArrowOffsetType.UIButtonConformPlant, ArrowDirection.Down);
                            NewbieGuideManager.Instance.WindowUIController[25].GetComponent<ButtonListener>().enabled = false;
                             
                            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[25]);
                            dynamicInvokeGuide.Click += () =>
                            {
                                UIManager.Instance.UIWindowBuyTreeChild.OnConformBuyPlant();
                             
                                RemovableObjectBehavior removableObjectBehavior = (RemovableObjectBehavior)SceneManager.Instance.BuildingBehaviorTemporary;
                              
                                NewbieGuideManager.Instance.TriggerCondition(() => {return removableObjectBehavior.ButtonOk != null; },
                                () =>
                                {
                                    NewbieGuideManager.Instance.WindowUIController[25].GetComponent<ButtonListener>().enabled = true;
                                    removableObjectBehavior.ButtonOk.GetComponent<ButtonListener>().enabled = false;
                                  
                                    base.UnHightlightController(removableObjectBehavior.ButtonCancel);
                                    base.CreateSceneGuideArrow(removableObjectBehavior.ButtonOk, ArrowOffsetType.SceneBuildingOkButton, ArrowDirection.Down);
                                    dynamicInvokeGuide = base.AddDynamicGuide(removableObjectBehavior.ButtonOk);
                                    dynamicInvokeGuide.Click += () =>
                                    {
                                        removableObjectBehavior.RemovableObjectCommon.OnConstructBuildingByNewbieGuide();
                                        base.ActiveCollider(removableObjectBehavior.gameObject, false); 
                                         NewbieGuideManager.Instance.Waiting(20, () =>
                                         {
                                            base.ActiveCollider(UIManager.Instance.UIWindowBuyTreeChild.gameObject, true);
                                            base.ResetAll();
                                            NewbieGuideManager.Instance.InvokeNextGuide();
                                         });
                                    };
                                });
                            };

                        };

                    });
                };
            };
        };
    }

    public override void OnIntroRemoveObstacle()
    {
        Debug.Log("OnIntroRemoveObstacle 1");
        if (LogicController.Instance.AllProps.Count > 0 || !base.ConditionPlant(RemovableObjectType.QualitySmallCactus) || base.ConditionRemovableObectState(RemovableObjectType.QualitySmallCactus, RemovableObjectEditorState.Normal))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        Debug.Log("OnIntroRemoveObstacle 2");
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);

        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.41f]);
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () =>
        {
            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
            // BuildingLogicData buildingLogicData = LogicController.Instance.GetBuildings(BuildingType.Barracks)[0];
            RemovableObjectLogicData removableObjectLogicData = (new List<RemovableObjectLogicData>(LogicController.Instance.AllRemovableObjects.Where(a => a.ObjectType == RemovableObjectType.QualitySmallCactus)))[0];
            RemovableObjectBehavior removableObjectBehavior = null;
            //NewbieGuideManager.Instance.TriggerCondition(() => { return (buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column)) != null; },
            NewbieGuideManager.Instance.TriggerCondition(() => { return (removableObjectBehavior = (RemovableObjectBehavior)SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(removableObjectLogicData.BuildingPosition.Row, removableObjectLogicData.BuildingPosition.Column)) != null; },
            () =>
            {
                base.CameraFollowTarget(removableObjectBehavior.BuildingAnchor);
                base.HightlightController(removableObjectBehavior.gameObject);
                base.CreateSceneGuideArrow(removableObjectBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneSelectBuilding, ArrowDirection.Down);
                removableObjectBehavior.enabled = false;
                dynamicInvokeGuide = base.AddDynamicGuide(removableObjectBehavior.gameObject);
                dynamicInvokeGuide.Click += () =>
                {
                    removableObjectBehavior.enabled = true;
                    removableObjectBehavior.OnClick();
                    removableObjectBehavior.enabled = false;
                    Debug.Log("=====================");
                    base.ActiveCollider(removableObjectBehavior.gameObject, false);
                    base.ActiveCollider(CameraManager.Instance.gameObject, false);
                    base.ActiveCollider(NewbieGuideManager.Instance.PopupController[0], false);
                    base.ActiveCollider(NewbieGuideManager.Instance.PopupController[4], true);
                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.PopupController[4], ArrowOffsetType.UIPopupButtonRemoveObstacel, ArrowDirection.Down);

                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.PopupController[4]);
                    dynamicInvokeGuide.Click += () =>
                    {
                        base.ActiveCollider(UIManager.Instance.UIWindowSelectBuilder.gameObject, false);
                        //UIManager.Instance.UIWindowSelectBuilder.gameObject.GetComponent<BoxCollider>().enabled = true;
                        UIManager.Instance.UIWindowSelectBuilder.WindowEvent += () =>
                        {
                            base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[2], true);
                            UIItemBuilder uiItemBuilder = NewbieGuideManager.Instance.WindowUIController[2].GetComponent<UIItemBuilder>();
                            uiItemBuilder.enabled = false;
                            base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[2], ArrowOffsetType.UISelectBuilderButton1, ArrowDirection.Left);

                            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[2]);
                            dynamicInvokeGuide.Click += () =>
                            {
                                uiItemBuilder.enabled = true;
                                uiItemBuilder.OnClick();
                                base.ActiveCollider(UIManager.Instance.UIWindowSelectBuilder.gameObject, true);
                                base.ResetAll();
                                NewbieGuideManager.Instance.InvokeNextGuide();
                                base.ActiveUILayerUICameraInput(false);
                            };
                        };
                    };
                };
            });
        };
    }

    public override void OnRemoveObstacle()
    {
        if (LogicController.Instance.AllProps.Count > 0 || !base.ConditionPlant(RemovableObjectType.QualitySmallCactus) || !base.ConditionRemovableObectState(RemovableObjectType.QualitySmallCactus, RemovableObjectEditorState.Normal))
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        NewbieGuideManager.Instance.MainUIController[7].SetActive(false);

        base.SetAllMainUIActive(false);
        base.SetAllBuildingActive(false);

        RemovableObjectLogicData removableObjectLogicData = null;
        RemovableObjectBehavior removableObjectBehavior = null;

        List<RemovableObjectLogicData> removableObjectLogicDataList = LogicController.Instance.AllRemovableObjects;
        NewbieGuideManager.Instance.TriggerCondition(
        () =>
        {
            removableObjectLogicDataList = new List<RemovableObjectLogicData>(LogicController.Instance.AllRemovableObjects.Where(a => a.ObjectType.Equals(RemovableObjectType.QualitySmallCactus) && !a.EditorState.Equals(RemovableObjectEditorState.Normal)));
            if (removableObjectLogicDataList.Count > 0)
                removableObjectLogicData = removableObjectLogicDataList[0];

            if (removableObjectLogicData != null)
                removableObjectBehavior = (RemovableObjectBehavior)SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(removableObjectLogicData.BuildingPosition.Row, removableObjectLogicData.BuildingPosition.Column);
            return removableObjectBehavior != null;
        },
        () =>
        {
            base.CameraFollowTarget(removableObjectBehavior.BuildingAnchor);
            base.ActiveCollider(removableObjectBehavior.gameObject, false);
            //base.ActiveCollider(CameraManager.Instance.gameObject, false);
            NewbieGuideManager.Instance.TriggerCondition(() => { return removableObjectLogicData.EditorState == RemovableObjectEditorState.ReadyForComplete; },
            () =>
            {
                base.ActiveUILayerUICameraInput(true);
                base.ActiveCollider(removableObjectBehavior.gameObject, true);
                removableObjectBehavior.enabled = true;
                base.CameraFollowTarget(removableObjectBehavior.BuildingAnchor);
                base.HightlightController(removableObjectBehavior.gameObject, true);

                base.CreateSceneGuideArrow(removableObjectBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneConstructBuilding, ArrowDirection.Down);

                DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(removableObjectBehavior.gameObject);
                dynamicInvokeGuide.Click += () =>
                {
                    removableObjectBehavior.enabled = false;
                    base.ActiveSceneLayerUICameraInput(false);
                    NewbieGuideManager.Instance.Waiting(90, () =>
                    {
                        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.5f]);
                        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(true);
                        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                        dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
                        dynamicInvokeGuide.Click += () =>
                        {
                            //===========================
                            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
                            BuildingLogicData buildingLogicData = LogicController.Instance.GetBuildings(BuildingType.PropsStorage)[0];
                            BuildingBehavior buildingBehavior = null;
                            NewbieGuideManager.Instance.TriggerCondition(() => { return (buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column)) != null; },
                            () =>
                            {
                                base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
                                base.HightlightController(buildingBehavior.gameObject);
                                base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneSelectBuilding, ArrowDirection.Down);
                                buildingBehavior.enabled = false;
                                dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
                                dynamicInvokeGuide.Click += () =>
                                {
                                    buildingBehavior.enabled = true;
                                    buildingBehavior.OnClick();
                                    buildingBehavior.enabled = false;
                                    base.ActiveCollider(buildingBehavior.gameObject, false);
                                    base.ActiveCollider(CameraManager.Instance.gameObject, false);
                                    base.ActiveCollider(NewbieGuideManager.Instance.PopupController[0], false);
                                    base.ActiveCollider(NewbieGuideManager.Instance.PopupController[5], true);
                                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.PopupController[5], ArrowOffsetType.UIPopupButtonPropsStorage, ArrowDirection.Down);
                                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.PopupController[5]);
                                    dynamicInvokeGuide.Click += () =>
                                    {
                                        base.ActiveCollider(UIManager.Instance.UIWindowPropsStorage.gameObject, false);
                                        UIManager.Instance.UIWindowPropsStorage.WindowEvent += () =>
                                        {
                                            buildingBehavior.enabled = true;
                                            base.ActiveCollider(UIManager.Instance.UIWindowPropsStorage.gameObject, false);
                                            GameObject cell = NewbieGuideManager.Instance.CellGrid[0].transform.GetChild(0).gameObject;
                                            base.CreateUIGuideArrow(cell, ArrowOffsetType.UIPropsCell, ArrowDirection.Down);
                                        };

                                        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.BottomLeft, true);
                                        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.6f]);
                                        dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);

                                        dynamicInvokeGuide.Click += () => NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[13.7f]);
                                        dynamicInvokeGuide.ClickNext.Enqueue(() =>
                                        {
                                            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
                                            base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[23], ArrowOffsetType.UICloseWindowPropStorage, ArrowDirection.Right);
                                            base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[23].gameObject, true);
                                            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[23].gameObject);
                                            dynamicInvokeGuide.Click += () =>
                                            {
                                                base.ActiveCollider(UIManager.Instance.UIWindowPropsStorage.gameObject, true);
                                                SceneManager.Instance.UnSelectBuilding();
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
                        };
                    });
                };
            });

        });
    }

}

 
