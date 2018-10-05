using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class IntroConstructBuilderHut : NewbieGuide
{
    public override void OnIntroConstructBuilderHut()
    {
        if (LogicController.Instance.GetBuildingCount(BuildingType.BuilderHut) > 1)
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
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);

        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[12.1f]);
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
                base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[20], true);
                base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[20], ArrowOffsetType.UICompositeMenu, ArrowDirection.Right);
                //construct building window : buy resource button
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[20]);
                UISelectMenu uiSelectMenu = NewbieGuideManager.Instance.WindowUIController[20].GetComponent<UISelectMenu>();
                uiSelectMenu.enabled = false;
                dynamicInvokeGuide.Click += () =>
                {
                    base.ActiveCollider(UIManager.Instance.UIConstructBuilding.gameObject, true);
                    uiSelectMenu.enabled = true;
                    uiSelectMenu.OnClick();

                    base.SetRollPanelLocalPosition(NewbieGuideManager.Instance.WindowUIController[21], 0);

                    base.ActiveCollider(UIManager.Instance.UIWindowBuyBuilding.gameObject, false);
                    base.ActiveColliderSelf(UIManager.Instance.UIWindowBuyBuilding.gameObject, true);
                    base.ActiveCollider(NewbieGuideManager.Instance.WindowUIController[15], true);
                    base.ActiveBuildingInfoCollider(NewbieGuideManager.Instance.WindowUIController[15], false);
                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.WindowUIController[15], ArrowOffsetType.UIBuyBuildingButton, ArrowDirection.Left);
                    dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.WindowUIController[15]);
                    UIBuyBuilding uiBuyBuilding = NewbieGuideManager.Instance.WindowUIController[15].GetComponent<UIBuyBuilding>();
                    uiBuyBuilding.enabled = false;
                    UIDragPanelContents uiDragPanelContents = NewbieGuideManager.Instance.WindowUIController[15].GetComponent<UIDragPanelContents>();
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
                                buildingBehavior.enabled = false;
                                base.ActiveSceneLayerUICameraInput(false);
                                NewbieGuideManager.Instance.Waiting(90, () =>
                                {
                                    NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                                    NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[12.2f]);

                                    base.SetAllMainUIColorGray();
                                    base.SetAllBuildingColorUnHightlight();
                                    base.UnHightlightController(SceneManager.Instance.AgeMap, true);
                                    NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
                                    NewbieGuideManager.Instance.MainUIController[11].SetActive(false);
                                    base.ColorFullCotroller(NewbieGuideManager.Instance.MainUIController[10], false);
                                    base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[10], ArrowOffsetType.UIBuilderBar, ArrowDirection.Up);
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
                    };
                };
            };
        };
    }
    public override void OnConstructBuilderHut()
    {
        
    }
}
