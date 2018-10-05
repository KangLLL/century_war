using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
public class UIPopupBtnAccelerate : MonoBehaviour {
    [SerializeField] UILabel[] m_UILabel;//0 = cost gem; 1 = remaining time  ;2 = accelerate Text
    [SerializeField] UISprite[] m_UISprite;//0 = bakcground ;1 = Icon gem ;2 = Icon
    public BuildingLogicData BuildingLogicData { get; set; }
    public AccelerateType AccelerateType { get; set; }
	// Update is called once per frame
	void Update () 
    {
        if (this.BuildingLogicData != null)
            this.SetBtnData();
	}
    public void SetBtnData()
    {
        switch (this.AccelerateType)
        { 
               
            case AccelerateType.Resource:
                this.SetItemData(CommonUtilities.MarketCalculator.GetResourceAccelerateCostGem(this.BuildingLogicData.BuildingType, this.BuildingLogicData.Level), this.BuildingLogicData.RemainResourceAccelerateTime);
                break;
            case AccelerateType.Army:
                this.SetItemData(ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateCostGem, this.BuildingLogicData.RemainArmyAccelerateTime);
                break;
            case AccelerateType.Item:
                break;
        }
    }
    void SetItemData(int costGem,int remainingTime)
    {
        m_UILabel[0].text = costGem.ToString(); 
        if (remainingTime >= 0)
        {
            m_UILabel[0].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            m_UILabel[1].text = SystemFunction.TimeSpanToString(remainingTime);
            m_UILabel[2].text = StringConstants.PROMT_ACCELERATE_NOW;
            m_UISprite[0].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            m_UISprite[1].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            m_UISprite[2].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            m_UILabel[0].color = Color.white;
            m_UILabel[1].text = "";
            m_UILabel[2].text = StringConstants.PROMT_ACCELERATE;
            m_UISprite[0].color = Color.white;
            m_UISprite[1].color = Color.white;
            m_UISprite[2].color = Color.white;
        }
    }
}
