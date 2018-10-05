using UnityEngine;
using System.Collections;
using CommonUtilities;

public class UIBuyResourceItem : MonoBehaviour, IUIItem
{
    [SerializeField] ResourceType m_ResourceType;
    [SerializeField] UILabel[] m_UILabel;//0 = title ;1 = price; 2 = buy context;
    [SerializeField] UISprite m_UISprite;//icon
    [SerializeField] float m_BuyPercentage;
    int m_Price;
    int m_BuyResourceCount;
    string m_Title;
    string m_Context;
    public void SetItemData()
    {
        int buyMax = 0;
        int buyCount = 0; 
        switch(m_ResourceType)
        {
            case ResourceType.Gold:
                buyMax = LogicController.Instance.PlayerData.GoldMaxCapacity - LogicController.Instance.PlayerData.CurrentStoreGold;
                buyCount = Mathf.RoundToInt(m_BuyPercentage / 100 * LogicController.Instance.PlayerData.GoldMaxCapacity);
                m_BuyResourceCount = buyCount > buyMax ? buyMax : buyCount;
                m_Price = MarketCalculator.GetGoldCost(m_BuyResourceCount);
                m_UILabel[0].text = m_BuyPercentage < 100 ? string.Format(StringConstants.PROMT_FILL_PERCENTAGE_STORAGE, StringConstants.RESOURCE_GOLD, m_BuyPercentage) : string.Format(StringConstants.PROMT_FILL_STORAGE, StringConstants.RESOURCE_GOLD);
                m_UILabel[1].text = m_Price.ToString() + ClientSystemConstants.EXPRESSION_ICON_DICTIONARY[3];
                m_UILabel[1].color = LogicController.Instance.PlayerData.CurrentStoreGem >= this.m_Price ? Color.white : Color.red;
                m_UILabel[2].text = m_BuyResourceCount + StringConstants.RESOURCE_GOLD;
                m_Title = string.Format(StringConstants.PROMT_BUY_RESOURCE, StringConstants.RESOURCE_GOLD);
                m_Context = string.Format(StringConstants.PROMT_CONFORM_BUY_RESOURCE, m_BuyResourceCount, StringConstants.RESOURCE_GOLD);
                break;
            case ResourceType.Food:
                buyMax = LogicController.Instance.PlayerData.FoodMaxCapacity - LogicController.Instance.PlayerData.CurrentStoreFood;
                buyCount = Mathf.RoundToInt(m_BuyPercentage / 100 * LogicController.Instance.PlayerData.FoodMaxCapacity);
                m_BuyResourceCount = buyCount > buyMax ? buyMax : buyCount;
                m_Price = MarketCalculator.GetGoldCost(m_BuyResourceCount);
                m_UILabel[0].text = m_BuyPercentage < 100 ? string.Format(StringConstants.PROMT_FILL_PERCENTAGE_STORAGE, StringConstants.RESOURCE_FOOD, m_BuyPercentage) : string.Format(StringConstants.PROMT_FILL_STORAGE, StringConstants.RESOURCE_FOOD);
                m_UILabel[1].text = m_Price.ToString() + ClientSystemConstants.EXPRESSION_ICON_DICTIONARY[3];
                m_UILabel[1].color = LogicController.Instance.PlayerData.CurrentStoreGem >= this.m_Price ? Color.white : Color.red;
                m_UILabel[2].text = m_BuyResourceCount + StringConstants.RESOURCE_FOOD;
                m_Title = string.Format(StringConstants.PROMT_BUY_RESOURCE, StringConstants.RESOURCE_FOOD);
                m_Context = string.Format(StringConstants.PROMT_CONFORM_BUY_RESOURCE, m_BuyResourceCount, StringConstants.RESOURCE_FOOD);
                break;
        }
    }
    void OnClick()
    {
        if (!this.enabled)
            return;

        switch (m_ResourceType)
        {
            case ResourceType.Gold:
                if (LogicController.Instance.PlayerData.CurrentStoreGold >= LogicController.Instance.PlayerData.GoldMaxCapacity)
                {
                    UIErrorMessage.Instance.ErrorMessage(41, StringConstants.RESOURCE_GOLD);
                    return;
                }
                break;
            case ResourceType.Food:
                if (LogicController.Instance.PlayerData.CurrentStoreFood >= LogicController.Instance.PlayerData.FoodMaxCapacity)
                {
                    UIErrorMessage.Instance.ErrorMessage(41, StringConstants.RESOURCE_FOOD);
                    return;
                }
                break;
        }
        if (UIManager.Instance.UIWindowBuyResource.ControlerFocus != null)
            return;
        else
            UIManager.Instance.UIWindowBuyResource.ControlerFocus = gameObject;
        print("UIBuyResourceItem");
        UIManager.Instance.UIWindowFocus = null;
        UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
        UIManager.Instance.UIWindowCostPrompt.Click += () =>
        {
            UIManager.Instance.UIWindowBuyResource.HideWindow();
            if (m_Price > LogicController.Instance.PlayerData.CurrentStoreGem)
            { 
                UIManager.Instance.UIWindowConfirmPrompt.UnRegistDelegate();
                UIManager.Instance.UIWindowConfirmPrompt.ShowWindow(StringConstants.PROMT_GEM_IS_NOT_ENOUGH, StringConstants.PROMT_GET_MORE_GEM, true);
                UIManager.Instance.UIWindowConfirmPrompt.Click += () =>
                {
                    UIManager.Instance.UIWindowFocus = null;
                    UIManager.Instance.UISelectShopMenu.GoShopping();
                };
            }
            else
            {
                switch(m_ResourceType)
                {
                    case ResourceType.Gold:
                        LogicController.Instance.BuyGold(m_BuyResourceCount);
						AudioController.Play("GoldCollect");
                        break;
                    case ResourceType.Food:
                        LogicController.Instance.BuyFood(m_BuyResourceCount);
						AudioController.Play("FoodCollect");
                        break;
                }
            }

			UIManager.Instance.UIWindowFocus = UIManager.Instance.UIWindowCostPrompt.gameObject;
        };
        UIManager.Instance.UIWindowCostPrompt.ShowWindow(this.m_Price, this.m_Context,new Vector3(0,0,-2000), this.m_Title);
        UIManager.Instance.UIWindowCostPrompt.UnRegistWindowEvent();
        UIManager.Instance.UIWindowCostPrompt.WindowCloseEvent += () => UIManager.Instance.UIWindowBuyResource.ControlerFocus = null;


    }
}
