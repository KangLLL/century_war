using UnityEngine;
using System.Collections;

public class Introduction : NewbieGuide
{
    public override void OnIntroduction()
    { 
        //0 = gold,food,gem; 1 = buy builidng;2 = shopping ;3 = exp,honour,task;4 = email,attack;5 = builder,shield ;6 = gold;
        if (!LogicController.Instance.PlayerData.IsNewbie)
            return;
        if (LogicController.Instance.PlayerData.IsRegisterSuccessful)
        {
            NewbieGuideManager.Instance.InvokeNextGuide();
            return;
        }
        NewbieGuideManager.Instance.MainUIControllerParent[0].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[1].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[2].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[3].SetActive(false);
        NewbieGuideManager.Instance.MainUIControllerParent[4].SetActive(false);
        base.SetAllMainUIActive(false);
        base.SetAllBuildingActive(false);
        base.ActiveCollider(CameraManager.Instance.gameObject, false);
        NewbieGuideManager.Instance.Waiting(60, () =>
        {
            NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.Center, true);
            base.SetAllMainUIColorGray();
            base.SetAllBuildingColorUnHightlight();
            base.UnHightlightController(SceneManager.Instance.AgeMap, true);
        });
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[1.1f]);
        //NewbieGuideManager.Instance.UIWindowGuide.Click += () =>
        //{
        //    base.ResetAll();
        //    NewbieGuideManager.Instance.InvokeNextGuide(); 
        //};
        DynamicInvokeGuide dynamicInvokeGuide = base.AddDynamicGuide(NewbieGuideManager.Instance.UIWindowGuide.gameObject);
        dynamicInvokeGuide.Click += () =>
        {
            base.ResetAll();
            NewbieGuideManager.Instance.InvokeNextGuide(); 
        };
    }
}
