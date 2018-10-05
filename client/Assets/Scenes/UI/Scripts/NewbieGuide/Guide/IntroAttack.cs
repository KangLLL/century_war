using UnityEngine;
using System.Collections;

public class IntroAttack : NewbieGuide
{
    public override void OnIntroAttack()
    {
        if (LogicController.Instance.PlayerData.Honour != 1000)
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        BattleData.IsNewbie = true;
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
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.1f]);
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () => NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.2f]);
        dynamicInvokeGuide.ClickNext.Enqueue(() =>
        {
            NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.3f]);
            NewbieGuideManager.Instance.UIWindowGuide.SetClickState(false);
            
            NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(true);
            NewbieGuideManager.Instance.MainUIController[9].SetActive(false);
            base.HightlightController(NewbieGuideManager.Instance.MainUIController[8]);
            base.CreateUIGuideArrow(NewbieGuideManager.Instance.MainUIController[8], ArrowOffsetType.UIAttackButton, ArrowDirection.Down);
            dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.MainUIController[8]);
            dynamicInvokeGuide.Click += () =>
            {
                base.DestroyGuideArrow();
                NewbieGuideManager.Instance.UIWindowGuide.HideWindow();
            };
                
             
        });
    }
 
}
