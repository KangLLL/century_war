using UnityEngine;
using System.Collections;
using System;
public class UIPopupBtnUpgrade : MonoBehaviour {
    [SerializeField] UILabel[] m_TextCost;//0 = gold; 1 = food; 2 = oil ;3 = gem;
    public BuildingLogicData BuildingLogicData { get; set; }
    protected Vector3 m_IniLocalPosition = new Vector3(32, 35, -3);
    protected Vector3 m_OffsetLocalPosition = new Vector3(0, 20, 0);
    public void SetCostItemData()
    {  
        if (this.BuildingLogicData != null)
        {
            int[] costValue = SystemFunction.ConverTObjectToArray<int>(this.BuildingLogicData.UpgradeGold, this.BuildingLogicData.UpgradeFood, this.BuildingLogicData.UpgradeOil, this.BuildingLogicData.UpgradeGem);
            int[] userHasValue = SystemFunction.ConverTObjectToArray<int>(LogicController.Instance.PlayerData.CurrentStoreGold, LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil, LogicController.Instance.PlayerData.CurrentStoreGem);
            
            for (int i = 0, j = 0; i < m_TextCost.Length; i++)
            {
                if (costValue[i] > 0)
                {
                    m_TextCost[i].transform.parent.gameObject.SetActive(true);
                    m_TextCost[i].text = costValue[i].ToString();
                    m_TextCost[i].color = costValue[i] <= userHasValue[i] ? new Color(1, 1, 1, 1) : new Color(1, 0, 0, 1);
                    m_TextCost[i].transform.parent.localPosition = m_IniLocalPosition + j * m_OffsetLocalPosition;
                    j++; 
                }
                else
                {
                    m_TextCost[i].transform.parent.gameObject.SetActive(false);
                }
            } 
        }
    }
 
}
