using UnityEngine;
using System.Collections;

public class IntroTask : NewbieGuide
{
    public override void OnIntroTask()
    {
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(true);
        NewbieGuideManager.Instance.MainUIController[4].SetActive(false);
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);
        base.HightlightController(NewbieGuideManager.Instance.MainUIController[7],false);
        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[14.1f]);

        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () =>
        {
            NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
            base.ActiveCollider(NewbieGuideManager.Instance.MainUIController[7], true);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[7], ArrowOffsetType.UITaskButton, ArrowDirection.Down);
            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.MainUIController[7]);
            dynamicInvokeGuide.Click += () =>
            {
                base.DestroyGuideArrow();
                NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
                NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[14.2f]);
                dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);

                dynamicInvokeGuide.Click  += ()=> NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[14.3f]);
                dynamicInvokeGuide.ClickNext.Enqueue(() => { NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[14.4f]); });
                dynamicInvokeGuide.ClickNext.Enqueue(() =>
                {
                    NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
                    base.ResetAll();
                    NewbieGuideManager.Instance.InvokeNextGuide();
                    LogicController.Instance.CompleteNewbieGuide();
                });

            };
            
        };
    }
}
