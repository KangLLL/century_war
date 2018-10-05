using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using System;
public class IntroConstructGoldMine : NewbieGuide
{ 
    public override void OnIntroConstructGoldMine()
    {
        if (base.ConditionExistBuilding(BuildingType.GoldMine))
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
        base.HightlightController(NewbieGuideManager.Instance.MainUIController[3]);
        
        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(false);
        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[3.1f]);

        base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[3], ArrowOffsetType.UIBuildingButton, ArrowDirection.Down);
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.MainUIController[3]);
        dynamicInvokeGuide.Click += () =>
        {
            
            base.UnHightlightController(NewbieGuideManager.Instance.MainUIController[3]); 
            NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false);
            base.ColorGrayCotroller(UIManager.Instance.UIConstructBuilding.gameObject);
            base.ActiveColliderSelf(UIManager.Instance.UIConstructBuilding.gameObject, true);
            base.HightlightController(NewbieGuideManager.Instance.WindowUIController[0]);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[0], ArrowOffsetType.UIResourceMenu, ArrowDirection.Left);
            //construct building window : buy resource button
            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[0]);
            UISelectMenu uiSelectMenu = NewbieGuideManager.Instance.WindowUIController[0].GetComponent<UISelectMenu>();
            uiSelectMenu.enabled = false;
            dynamicInvokeGuide.Click += () =>
            {
                base.UnHightlightController(NewbieGuideManager.Instance.WindowUIController[0]);
                base.ColorFullCotroller(UIManager.Instance.UIConstructBuilding.gameObject);
               
                uiSelectMenu.enabled = true;
                uiSelectMenu.OnClick();
             
                base.SetRollPanelLocalPosition(NewbieGuideManager.Instance.WindowUIController[7], 0); 
                base.ColorGrayCotroller(UIManager.Instance.UIWindowBuyBuilding.gameObject); 
                base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyBuilding.gameObject, true);
                base.ColorFullCotroller(NewbieGuideManager.Instance.WindowUIController[1]);
                base.ActiveBuildingInfoCollider(NewbieGuideManager.Instance.WindowUIController[1], false);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[1], ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[1]);
                UIBuyBuilding uiBuyBuilding = NewbieGuideManager.Instance.WindowUIController[1].GetComponent<UIBuyBuilding>();
                uiBuyBuilding.enabled = false;
                UIDragPanelContents uiDragPanelContents = NewbieGuideManager.Instance.WindowUIController[1].GetComponent<UIDragPanelContents>();
                uiDragPanelContents.enabled = false;
                dynamicInvokeGuide.Click += () =>
                {
                    base.DestroyGuideArrow();
                    base.ColorGrayCotroller(NewbieGuideManager.Instance.WindowUIController[1]);
                    base.ColorFullCotroller(UIManager.Instance.UIWindowBuyBuilding.gameObject);

                    base.HightlightController(SceneManager.Instance.AgeMap, true);
                    base.SetAllBuildingColorHightlight();
                    base.SetAllMainUIColorFull(false);

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
                            base.UnHightlightController(SceneManager.Instance.AgeMap, true);
                            base.SetAllBuildingColorUnHightlight();
                            base.SetAllMainUIColorGray();
                            NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[3.2f]);
                            NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.BottomLeft, true);
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
                                    base.HightlightController(SceneManager.Instance.AgeMap, true);
                                    base.SetAllBuildingColorHightlight();
                                    base.SetAllMainUIColorFull();
                                    base.ActiveCollider(UIManager.Instance.UIWindowSelectBuilder.gameObject, true);
                                    NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
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
    }
    public override void OnConstructGoldMine()
    { 
        if (!(LogicController.Instance.GetBuildingCount(BuildingType.GoldMine) == 1 && base.ConditionExistBuildingLevel(BuildingType.GoldMine, 0)))
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
        BuildingLogicData buildingLogicData = null;
        BuildingBehavior buildingBehavior = null;
        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.GoldMine);
        NewbieGuideManager.Instance.TriggerCondition(
        () => 
        {
            for (int i = 0; i < buildingLogicDataList.Count; i++)
            {
                if (buildingLogicDataList[i].BuildingType == BuildingType.GoldMine && buildingLogicDataList[i].CurrentBuilidngState != BuildingEditorState.Normal)
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
            NewbieGuideManager.Instance.TriggerCondition(() => { return buildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate; },
            () => 
            {
                base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
                //base.ActiveCollider(buildingBehavior.gameObject, true);
                base.HightlightController(buildingBehavior.gameObject, true);
                
                NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[3.3f]);
                NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.BottomLeft, true);
                NewbieGuideManager.Instance.UIWindowGuide.SetClickState(false);
                base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneConstructBuilding, ArrowDirection.Down);

                DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(buildingBehavior.gameObject);
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveSceneLayerUICameraInput(false);
                    //NewbieGuideManager.Instance.TriggerCondition(() => { return (buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column)) != null; }, () => { base.ActiveCollider(buildingBehavior.gameObject, false); });
                    NewbieGuideManager.Instance.Waiting(90, () =>
                    { 
                        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[3.4f]);
                        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(true);
                        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                        base.ColorFullCotroller(NewbieGuideManager.Instance.MainUIController[0], false);
                        base.ColorGrayCotroller(NewbieGuideManager.Instance.MainUIBarLabel[0].gameObject);
                        base.ColorGrayCotroller(NewbieGuideManager.Instance.MainUIBarLabel[1].gameObject);
                        base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[0], ArrowOffsetType.UIGoldBar, ArrowDirection.Up);
                        dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
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
            });
        });
    }

   
}
