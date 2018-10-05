using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
public class UIWindowBuyMercenary : UIWindowCommon
{
    [SerializeField] UILabel[] m_UILabel;//0 = =Total ArmyClamp count
    [SerializeField] GameObject m_MercenaryPrefabParent;
    //[SerializeField] GameObject[] m_MercenaryPrefab;
    [SerializeField] PrefabDictionary m_MercenaryDict;//Mercenary Icon
    [SerializeField] Vector3 m_OffsetLocalPosition;
    List<UIMercenaryItem> m_UIMercenaryItemList = new List<UIMercenaryItem>();
    void Awake()
    {
        this.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        this.SetWindowItemData();
        base.ShowWindow();
    }
    public override void HideWindow()
    {
        base.HideWindow();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void ShowWindows(object param)
    {
        this.ShowWindow();
    }
    void SetWindowItemData()
    {
        this.CleanIcon();
        int i = 0;
        //SortedDictionary<MercenaryType, MercenaryProductLogicData> mercenarySortedDict = new SortedDictionary<MercenaryType, MercenaryProductLogicData>
        List<KeyValuePair<MercenaryType, MercenaryProductLogicData>> mercenaryTypeList = new List<KeyValuePair<MercenaryType, MercenaryProductLogicData>>(base.BuildingLogicData.MercenaryProducts);
        mercenaryTypeList.Sort((a, b) => ClientConfigConstants.Instance.GetMercenaryOrder(a.Key) - ClientConfigConstants.Instance.GetMercenaryOrder(b.Key));
      
        foreach (KeyValuePair<MercenaryType, MercenaryProductLogicData> k in mercenaryTypeList)
        {
            //GameObject go = Object.Instantiate(this.m_MercenaryPrefab[(int)k.Key]) as GameObject;
            GameObject go = Instantiate(m_MercenaryDict[k.Key.ToString()]) as GameObject;
            UIMercenaryItem uiMercenaryItem = go.GetComponent<UIMercenaryItem>();
            m_UIMercenaryItemList.Add(uiMercenaryItem);
            uiMercenaryItem.MercenaryProductLogicData = k.Value;
            uiMercenaryItem.MercenaryType = k.Key;
            uiMercenaryItem.BuildingLogicData = base.BuildingLogicData;
            uiMercenaryItem.SetItemData();
            go.transform.parent = m_MercenaryPrefabParent.transform;
            go.transform.localScale = m_MercenaryPrefabParent.transform.localScale;
            go.transform.localPosition = i * this.m_OffsetLocalPosition;
            i++;
        } 
    }
    void CleanIcon()
    {
        m_UIMercenaryItemList.Clear();
        while (m_MercenaryPrefabParent.transform.childCount > 0)
        {
            Transform trans = m_MercenaryPrefabParent.transform.GetChild(0);
            trans.parent = null;
            Destroy(trans.gameObject);
        }
    }
    public void SetAllMercenaryItem()
    {
        foreach (UIMercenaryItem uiMercenaryItem in m_UIMercenaryItemList)
            uiMercenaryItem.SetItemData();
    }
    public void SetCapacityText()
    {
        m_UILabel[0].text = LogicController.Instance.CurrentAvailableArmyCapacity > LogicController.Instance.CampsTotalCapacity ? "[ff0000]" + LogicController.Instance.CurrentAvailableArmyCapacity + "[-]" + " / " + LogicController.Instance.CampsTotalCapacity : LogicController.Instance.CurrentAvailableArmyCapacity + " / " + LogicController.Instance.CampsTotalCapacity;
    }

	void Update()
	{
		this.SetCapacityText();
	}
}
