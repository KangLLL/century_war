using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class UIPopupBtnRemoveObject : MonoBehaviour {
    [SerializeField] GameObject[] m_CostObject;//0 = gold; 1 = food; 2 = oil ;3 = gem;
    [SerializeField] UILabel[] m_CostUILabel;//0 = gold; 1 = food; 2 = oil ;3 = gem;
    public RemovableObjectLogicData RemovableObjectLogicData { get; set; }
    public void SetBtnData()
    {
        if (this.RemovableObjectLogicData != null)
        {
            foreach (CostType cost in Enum.GetValues(typeof(CostType)))
            {
                switch (cost)
                {
                    case CostType.Gold:
                        this.SetBtnState(CostType.Gold, this.RemovableObjectLogicData.GoldCost > 0, LogicController.Instance.PlayerData.CurrentStoreGold >= this.RemovableObjectLogicData.GoldCost, this.RemovableObjectLogicData.GoldCost);
                        break;
                    case CostType.Food:
                        this.SetBtnState(CostType.Food, this.RemovableObjectLogicData.FoodCost > 0, LogicController.Instance.PlayerData.CurrentStoreFood >= this.RemovableObjectLogicData.FoodCost,this.RemovableObjectLogicData.FoodCost);
                        break;
                    case CostType.Oil:
                        this.SetBtnState(CostType.Oil, this.RemovableObjectLogicData.OilCost > 0, LogicController.Instance.PlayerData.CurrentStoreOil >= this.RemovableObjectLogicData.OilCost, this.RemovableObjectLogicData.OilCost);
                        break;
                    case CostType.Gem:
                        this.SetBtnState(CostType.Gem, this.RemovableObjectLogicData.GemCost > 0, LogicController.Instance.PlayerData.CurrentStoreGem >= this.RemovableObjectLogicData.GemCost, this.RemovableObjectLogicData.GemCost);
                        break;
                }

            } 
        }
    }
    void SetBtnState(CostType coatType, bool requsetCost, bool isHas ,int costValue)
    {
        m_CostObject[(int)coatType].SetActive(requsetCost);
        if (requsetCost)
        {
            m_CostUILabel[(int)coatType].color = isHas ? Color.white : Color.red;
            m_CostUILabel[(int)coatType].text = costValue.ToString();
        }
    }
 
}
