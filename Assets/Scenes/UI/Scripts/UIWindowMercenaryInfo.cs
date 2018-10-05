using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System.Collections.Generic;

public class UIWindowMercenaryInfo : UIWindowCommon
{
    [SerializeField] GameObject m_ArmyIconRef;
    //[SerializeField] GameObject[] m_ArmyPrefab;//Army Icon
    [SerializeField] PrefabDictionary m_MercenaryDict;//Mercenary Icon
    [SerializeField] UILabel[] m_UILabel;//Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity,UpgradeTime
    [SerializeField] UILabel[] m_UILabelTitle;// Name
    [SerializeField] UIUpgradeProgressBar[] m_UIUpgradeProgressBar;//DPS,HP,Gold,Food,Oil,Gem 
    [SerializeField] Vector3 INITIAL_LOCAL_POSITION = new Vector3(-10, 200, -3);
    [SerializeField] Vector3 OFFSET_LOCAL_POSITION = new Vector3(0, -50, 0);
    public MercenaryType MercenaryType { get; set; }
    // Use this for initialization

    void Awake()
    {
        this.GetTweenComponent();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        base.HideWindow();
    }
    public override void ShowWindow()
    {
        SetWindowItemData();
        base.ShowWindow();
    }
    void SetWindowItemData()
    {
        MercenaryConfigData mercenaryConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(this.MercenaryType);
        //Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity
        
        this.m_UILabel[0].text = ClientSystemConstants.BUILDINGCATEGORY_DICTIONARY[(BuildingCategory)mercenaryConfigData.FavoriteType];
        this.m_UILabel[1].text = ClientSystemConstants.ATTACKTYPE_DICTIONARY[(AttackType)mercenaryConfigData.AttackType];
        this.m_UILabel[2].text = ClientSystemConstants.TARGETTYPE_DICTIONARY[(TargetType)mercenaryConfigData.TargetType];
        this.m_UILabel[3].text = mercenaryConfigData.CapcityCost.ToString();
        this.m_UILabel[4].text = SystemFunction.TimeSpanToString(mercenaryConfigData.ProduceTime);
        this.m_UILabel[5].text = mercenaryConfigData.MoveVelocity.ToString();
        this.m_UILabel[6].text = mercenaryConfigData.Description;
        this.m_UILabelTitle[0].text = mercenaryConfigData.Name;

        while (this.m_ArmyIconRef.transform.childCount > 0)
        {
            Transform tran = this.m_ArmyIconRef.transform.GetChild(0);
            tran.parent = null;
            DestroyImmediate(tran.gameObject);
        }
        //GameObject go = Instantiate(m_ArmyPrefab[(int)this.MercenaryType].gameObject) as GameObject;
        GameObject go = Instantiate(m_MercenaryDict[this.MercenaryType.ToString()]) as GameObject;
        go.transform.parent = this.m_ArmyIconRef.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = this.m_ArmyIconRef.transform.localScale;

        float currentDps = SystemFunction.Division(mercenaryConfigData.AttackValue, mercenaryConfigData.AttackCD);
        ProgressParam paramDps = new ProgressParam() { ProgressCurrent = 1, ProgressNext = currentDps };
        ProgressParam paramHp = new ProgressParam() { ProgressCurrent = 1, ProgressNext = mercenaryConfigData.MaxHP };
        ProgressParam paramCostGold = new ProgressParam() { ProgressCurrent = 1, ProgressNext = mercenaryConfigData.HireCostGold };
        ProgressParam paramCostFood = new ProgressParam() { ProgressCurrent = 1, ProgressNext = mercenaryConfigData.HireCostFood };
        ProgressParam paramCostOil = new ProgressParam() { ProgressCurrent = 1, ProgressNext = mercenaryConfigData.HireCostOil };
        ProgressParam paramCostGem = new ProgressParam() { ProgressCurrent = 1, ProgressNext = mercenaryConfigData.HireCostGem };

        List<ProgressParam> progressParamArray = new List<ProgressParam>(SystemFunction.ConverTObjectToArray<ProgressParam>(paramDps, paramHp));
        List<UIUpgradeProgressBar> progressBarArray = new List<UIUpgradeProgressBar>(SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[1]));
        if (mercenaryConfigData.HireCostGold > 0)
        {
            progressParamArray.Add(paramCostGold);
            progressBarArray.Add(m_UIUpgradeProgressBar[2]);
        }
        if (mercenaryConfigData.HireCostFood > 0)
        {
            progressParamArray.Add(paramCostFood);
            progressBarArray.Add(m_UIUpgradeProgressBar[3]);
        }
        if (mercenaryConfigData.HireCostOil > 0)
        {
            progressParamArray.Add(paramCostOil);
            progressBarArray.Add(m_UIUpgradeProgressBar[4]);
        }
        if (mercenaryConfigData.HireCostGem > 0)
        {
            progressParamArray.Add(paramCostGem);
            progressBarArray.Add(m_UIUpgradeProgressBar[5]);
        }
        for (int i = 0, count = m_UIUpgradeProgressBar.Length; i < count; i++) 
            m_UIUpgradeProgressBar[i].gameObject.SetActive(false);
        for (int i = 0, count = progressBarArray.Count; i < count; i++)
        {
            progressBarArray[i].gameObject.SetActive(true);
            progressBarArray[i].transform.localPosition = INITIAL_LOCAL_POSITION + OFFSET_LOCAL_POSITION * i; 
            progressBarArray[i].SetProgressBar(progressParamArray[i].ProgressCurrent, progressParamArray[i].ProgressNext); 
        }
        
    }
}
