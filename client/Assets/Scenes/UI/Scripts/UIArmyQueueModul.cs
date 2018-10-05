using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommonUtilities;
public class UIArmyQueueModul : MonoBehaviour {
    //[SerializeField] UIUpgradeProgressBar[] m_ArmyIconProgressBar;
    [SerializeField] UIGridExt m_UIGridExt;
    [SerializeField] UIArmyItem m_UIArmyItem;
    [SerializeField] UILabel m_UILabelRemainingTime;
    [SerializeField] UILabel m_UILabelCostGem;
    [SerializeField] UISprite[] m_UISpriteButton;//Immediately button ;0=bk color ; 1=gem color; 2=bk gray; 3=gem gray
    public BuildingLogicData BuildingLogicData { get; set; }
    bool m_EnableProduceQuene = false;
    public bool EnableProduceQuene { get { return m_EnableProduceQuene; } set { m_EnableProduceQuene = value; if (m_EnableProduceQuene == false)ClearProduceQueue(); } }
   // Dictionary<ArmyType, UIArmyItem> m_ArmyQueue = new Dictionary<ArmyType, UIArmyItem>();
    List<UIArmyItem> m_ArmyQueue = new List<UIArmyItem>();
    List<KeyValuePair<ArmyType, List<ArmyIdentity>>> m_ArmyList;
    
	// Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       this.OnProduce();
    }
 
    void OnProduce()
    {
        if (!this.EnableProduceQuene)
            return;
        m_ArmyList = this.BuildingLogicData.ArmyProducts;
        if (m_ArmyList == null) 
            return;
        if (m_ArmyList.Count > m_ArmyQueue.Count)
        {  
            AddProduceQueue(m_ArmyList.Count - m_ArmyQueue.Count);
        }
        if (m_ArmyQueue.Count > m_ArmyList.Count)
        {
            RemoveAtProduceQueue(m_ArmyQueue.Count - m_ArmyList.Count);
        }
        if (m_ArmyList.Count > 0)
        { 
            //m_ArmyQueue[0].ArmyIdentity = m_ArmyList[0].Value[0];
            //m_ArmyQueue[0].SetArmyItemData(m_ArmyList[0].Value.Count,true);
            for (int i = 0, count = m_ArmyList.Count; i < count; i++)
            {//盲目查询，效率较低
                m_ArmyQueue[i].ArmyIdentity = m_ArmyList[i].Value[0];

                m_ArmyQueue[i].SetArmyItemData(m_ArmyList[i].Value.Count, i == 0 && !this.BuildingLogicData.IsArmyProduceBlock, i == 0 && this.BuildingLogicData.IsArmyProduceBlock);
            }
        }
      this.ActiveQueue();
      int remainingTime = this.BuildingLogicData.ArmyProductsRemainingTime;//LogicController.Instance.GetFactoryArmyProductsRemainingTime(this.BuildingLogicObject.BuildingIdentity);
      m_UILabelRemainingTime.text = SystemFunction.TimeSpanToString(remainingTime);
      int gemCost =  MarketCalculator.GetProduceTimeCost(remainingTime);
      m_UILabelCostGem.text = gemCost.ToString();
      m_UILabelCostGem.color = LogicController.Instance.PlayerData.CurrentStoreGem < gemCost ? Color.red : Color.white;
    }
    public void ActiveQueue()
    {
        //print("this.BuildingLogicObject.ArmyProducts  1=" + this.BuildingLogicObject.ArmyProducts);
        if (this.BuildingLogicData.ArmyProducts != null)
        {
            //print("this.BuildingLogicObject.ArmyProducts  2=" + this.BuildingLogicObject.ArmyProducts);
            if (this.BuildingLogicData.ArmyProducts.Count > 0)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.ClearProduceQueue();
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
            
    }
    void AddProduceQueue(int count)
    {
        for (int i = 0; i < count; i++)
        {
            UIArmyItem uiArmyItem = (GameObject.Instantiate(m_UIArmyItem.gameObject) as GameObject).GetComponent<UIArmyItem>();
            uiArmyItem.transform.parent = m_UIGridExt.transform;
            uiArmyItem.transform.localScale = m_UIGridExt.transform.localScale;
            uiArmyItem.transform.localPosition = new Vector3(0, 0, -10);
            m_ArmyQueue.Add(uiArmyItem);
        }
        this.m_UIGridExt.Reposition();
        this.SetQueueData();
    }
    void RemoveAtProduceQueue(int count)
    {
        for(int i=0;i<count;i++)
        {
            DestroyImmediate(m_ArmyQueue[0].gameObject);
            m_ArmyQueue.RemoveAt(0);
        }
        this.m_UIGridExt.Reposition();
        this.SetQueueData();
    }
    void SetQueueData()
    {
        if (this.m_ArmyList != null)
        {
            if (this.m_ArmyList.Count == this.m_ArmyQueue.Count)
            {
                for (int i = 0; i < m_ArmyList.Count; i++)
                {
                    m_ArmyQueue[i].ArmyIdentity = m_ArmyList[i].Value[0];
                    m_ArmyQueue[i].BuildingIdentity = this.BuildingLogicData.BuildingIdentity;
                    m_ArmyQueue[i].SetArmyItemData(m_ArmyList[i].Value.Count, false,false);
                }
            }
        }
    }
    public void ClearProduceQueue()
    { 
        //foreach (UIArmyItem uiArmyItem in m_ArmyQueue)
        //{
        //    Destroy(uiArmyItem.gameObject);
        //}
        while (m_ArmyQueue.Count > 0)
        {
            Destroy(m_ArmyQueue[0].gameObject);
            m_ArmyQueue.RemoveAt(0);
        }
        //m_ArmyQueue.Clear();
    }
    void OnImmediatelyArmyProduce()
    {
        if (UIManager.Instance.UIWindowBuyArmy.ControlerFocus != null)
            return;
        //this.BuildingLogicObject.ArmyProducts 
        if (this.BuildingLogicData.ArmyProducts != null)
        { 
            if (this.BuildingLogicData.ArmyProducts.Count > 0)
            { 
                if (LogicController.Instance.TotalArmyCapacity <= LogicController.Instance.CampsTotalCapacity)
                {
                    int remainingTime = this.BuildingLogicData.ArmyProductsRemainingTime;//LogicController.Instance.GetFactoryArmyProductsRemainingTime(this.BuildingLogicObject.BuildingIdentity);
                    int gemCost = MarketCalculator.GetProduceTimeCost(remainingTime);
                    UIManager.Instance.UIWindowBuyArmy.HideWindow();
                    UIManager.Instance.UIWindowCostPrompt.ShowWindow(gemCost, string.Format(StringConstants.PROMPT_GEM_COST, gemCost, StringConstants.COIN_GEM, StringConstants.PROMPT_ARMY_TYPE, StringConstants.PROMPT_TRAIN) + StringConstants.QUESTION_MARK, StringConstants.PROMPT_FINISH_INSTANTLY);
 
                    UIManager.Instance.UIWindowCostPrompt.Click += () =>
                    {
                        if (LogicController.Instance.PlayerData.CurrentStoreGem < gemCost)
                        {
                            print("宝石不足，去商店");
                            UIManager.Instance.UIWindowFocus = null;
                            //UIManager.Instance.UIButtonShopping.GoShopping();
                            UIManager.Instance.UISelectShopMenu.GoShopping();
                        }
                        else
                        { print("立即完成士兵队列!"); LogicController.Instance.FinishProduceArmyInstantly(this.BuildingLogicData.BuildingIdentity); }
                    };
                }
                else
                {
                    UIErrorMessage.Instance.ErrorMessage(1);
                }
            }
        }
    }
    public void SetImmediatelyBtnState(bool state)
    {
        //0=bk color ; 1=gem color; 2=bk gray; 3=gem gray
        m_UISpriteButton[0].alpha = state ? 1 : 0;
        m_UISpriteButton[1].alpha = state ? 1 : 0;
        m_UISpriteButton[2].alpha = state ? 0 : 1;
        m_UISpriteButton[3].alpha = state ? 0 : 1;
    }
    
}
