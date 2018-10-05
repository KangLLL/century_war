using UnityEngine;
using System.Collections;
using CommandConsts;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIArmyItem : MonoBehaviour {
    [SerializeField] UISprite m_UISprite;
    [SerializeField] UISprite[] m_ArmyTypeSpriteIcon;
    [SerializeField] UIUpgradeProgressBar m_UIUpgradeProgressBar;
    [SerializeField] UISlider m_UISliderProgressBar;
    [SerializeField] UILabel m_UILabel;//camps is full
    public ArmyIdentity ArmyIdentity { get; set; }
    public BuildingIdentity BuildingIdentity { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetArmyItemData(int produceCount, bool acitveProgress, bool activeCampIsFullText)
    {

        ArmyLogicData armyLogicObject = LogicController.Instance.GetArmyObjectData(this.ArmyIdentity);
        float remainingTime = (float)armyLogicObject.ProduceRemainingWorkload / ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency;
        float progress = (armyLogicObject.ProduceTotalWorkload - armyLogicObject.ProduceRemainingWorkload) / (float)armyLogicObject.ProduceTotalWorkload;
     
        m_UIUpgradeProgressBar.SetProgressBar(progress, SystemFunction.TimeSpanToString(Mathf.CeilToInt(remainingTime)));
        m_UIUpgradeProgressBar.SetText( "X" + produceCount);
        m_UISprite.spriteName = m_ArmyTypeSpriteIcon[(int)ArmyIdentity.armyType].spriteName;//ClientSystemConstants.ARMY_ICON_COMMON_DICTIONARY[this.ArmyIdentity.armyType];
        m_UISprite.transform.localScale = m_ArmyTypeSpriteIcon[(int)ArmyIdentity.armyType].transform.localScale; 
        this.ActiveProgress(acitveProgress);
        this.ActiveCampIsFullText(activeCampIsFullText);
    }
    void ActiveProgress(bool active)
    {
        m_UISliderProgressBar.gameObject.SetActive(active);
    }
    void ActiveCampIsFullText(bool active)
    {
        m_UILabel.gameObject.SetActive(active);
    }
    void OnCancelProduce()
    {
        AudioController.Play("ButtonClick");
        LogicController.Instance.CancelProduceArmy(this.ArmyIdentity.armyType, this.BuildingIdentity);
    }
}
