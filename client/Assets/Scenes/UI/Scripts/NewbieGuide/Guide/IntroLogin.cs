using UnityEngine;
using System.Collections;

public class IntroLogin : NewbieGuide
{
    public override void OnIntroLogin()
    {
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
        base.SetAllMainUIColorGray();
        base.SetAllBuildingColorUnHightlight();
        base.UnHightlightController(SceneManager.Instance.AgeMap, true);
        //NewbieGuideManager.Instance.UIWindowMask.ShowWindow(true, base.PositionLayer[0], false);

        NewbieGuideManager.Instance.UIWindowGuide.ShowWindow(UIAnchor.Side.BottomLeft, true);
        NewbieGuideManager.Instance.UIWindowGuide.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[2.1f]);
        NewbieGuideManager.Instance.UIWindowLogin.NewBieGuide = this;
        NewbieGuideManager.Instance.UIWindowLogin.ShowWindow(base.PositionLayer[2], true);
    }

}
