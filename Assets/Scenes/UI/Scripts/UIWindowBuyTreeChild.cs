using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class UIWindowBuyTreeChild : UIWindowCommon
{
    [SerializeField] UISprite m_UISprite;//plant icon;
    [SerializeField] UILabel m_UILabel;//gemPrice;
    [SerializeField] UILabel m_UILabelName;// plant name
    [SerializeField] UIPlantRewardPropModule m_UIPlantRewardPropModule;
    public ProductRemovableObjectConfigData ProductRemovableObjectConfigData { get; set; }
    public RemovableObjectConfigData RemovableObjectConfigData { get; set; }
    void Awake()
    {
        this.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        base.ShowWindowImmediatelySimplify();
        this.SetWindowItem();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediatelySimplify();
    }
    void SetWindowItem()
    {
        m_UISprite.spriteName = this.ProductRemovableObjectConfigData.IconName;
        m_UISprite.MakePixelPerfect();
        m_UILabel.text = this.ProductRemovableObjectConfigData.GemPrice.ToString();
        m_UILabelName.text = string.Format(StringConstants.PROMT_REMOVE_PLANT, StringConstants.LEFT_PARENTHESES + this.RemovableObjectConfigData.Name + StringConstants.RIGHT_PARENTHESES);
        m_UILabel.color = LogicController.Instance.PlayerData.CurrentStoreGem >= this.ProductRemovableObjectConfigData.GemPrice ? Color.white : Color.red;
        m_UIPlantRewardPropModule.SetModulData(new List<KeyValuePair<PropsType, int>>(this.RemovableObjectConfigData.GeneratePropsRate));
    }
    //Button message
    public void OnConformBuyPlant()
    {
        print("OnConformBuyPlant");
        //要实现部分按键多点的问题
        if (!this.enabled)
            return;
        //if (UIManager.Instance.UIWindowBuyBuilding.ControlerFocus != null) 
        //    return; 
        //else
        //    UIManager.Instance.UIWindowBuyBuilding.ControlerFocus = this.gameObject; 
        UIManager.Instance.UIWindowBuyTree.HideWindow();

        SceneManager.Instance.UnSelectBuilding();
        SceneManager.Instance.EnableCreateWallContinuation = false;
        SceneManager.Instance.DestroyTemporaryBuildingBehavior();
        SceneManager.Instance.ConstructObstacle(this.RemovableObjectConfigData, this.ProductRemovableObjectConfigData.RemovableObjectType, this.ProductRemovableObjectConfigData);

        //UIErrorMessage.Instance.ErrorMessage(36);
 
    }
    
}
