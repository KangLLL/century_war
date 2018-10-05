using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class ProduceCommon : MonoBehaviour {
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
    public ArmyData ArmyData { get; set; }
    protected float Progress { get; set; }
    protected float RemainingTime { get; set; }
    GameObject m_AccelerateProduceFX;
	// Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.BuildingLogicData != null)
            this.OnAccelerateFX();
	}
    protected virtual void ProduceProgress(int capacity, int ProduceCapacity, int efficiency, int ProduceWorkload)
    { 
    }
    void WorkloadCalculate(float deltaTime)
    {
        //this.ArmyData.ProduceRemainingWorkload -= this.BuildingData.ProduceArmyEfficiency * deltaTime;
        //this.Progress = (this.ArmyData.UpgradeWorkload - this.ArmyData.UpgradeRemainingWorkload) / this.ArmyData.UpgradeWorkload;
        //this.RemainingTime = this.ArmyData.UpgradeRemainingWorkload / this.ArmyData.;
    }
    protected virtual void AccelerateProduce()
    {
        if (this.BuildingLogicData.RemainArmyAccelerateTime < 0)
        {
            int costGem = ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateCostGem;
            
            string resourceContext = string.Format(StringConstants.PROMPT_ACCELERATE_ARMY, SystemFunction.TimeSpanToString(ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateLastTime), ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateScale);
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costGem, resourceContext, StringConstants.PROMT_IS_ACCELERATE);
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (LogicController.Instance.PlayerData.CurrentStoreGem < costGem)
                {
                    print("宝石不足，去商店");
                    UIManager.Instance.UIWindowFocus = null;
                    //UIManager.Instance.UIButtonShopping.GoShopping();
                    UIManager.Instance.UISelectShopMenu.GoShopping();
                }
                else
                {
                    print("加速产兵");
                    LogicController.Instance.AddArmyAccelerate(this.BuildingLogicData.BuildingIdentity);
                }
            };
        }
    }
    void OnAccelerateFX()
    {
        if (this.BuildingLogicData.RemainArmyAccelerateTime >= 0)
        {
            if (this.m_AccelerateProduceFX == null)
            {
                this.m_AccelerateProduceFX = SceneManager.Instance.CreateAccelerateFX(this.BuildingBehavior);
            }
        }
        else
        {
            if (this.m_AccelerateProduceFX != null)
                Destroy(this.m_AccelerateProduceFX);
        }
    }
}
