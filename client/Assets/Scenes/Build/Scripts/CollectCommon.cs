using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System;
public class CollectCommon : MonoBehaviour {
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
    //float m_Counter;
    //protected bool EnableCollect { get; set; }
    //protected bool IsFull { get; set; }
    //protected int CurrentStore { get; set; }
    const string BUILDING_RESOURCE_ICON = "BuildingScene/Common/ResourceIcon";
	private BuildingEditorState m_CurrentState;
    GameObject m_AccelerateCollectFX;
    tk2dSprite m_ResourceIcon;
    tk2dSprite m_ResourceIconBackground;
    BoxCollider m_BoxCollider;
    Color COLLECT_FULL_COLOR = new Color(1, (float)94 / 255, (float)94 / 255, 1);
	// Use this for initialization
	void Start () {

        this.CreateCollectIcon();
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.BuildingLogicData != null)
        {
            this.m_CurrentState = this.BuildingLogicData.CurrentBuilidngState;
            this.OnAccelerateFX();
            this.OnCollectIconState();
        }
    }

    protected virtual void Collect(BuildingIdentity id, ResourceType type, CollectMethod collectMethod = CollectMethod.Button)
    {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit || this.m_CurrentState != BuildingEditorState.Normal || !this.BuildingBehavior.CheckBuildingCreateStack() )
            return;
        if (this.m_CurrentState == BuildingEditorState.Normal)
        {
            switch (collectMethod)
            {
                case CollectMethod.Building:
                    if (SystemFunction.CheckCollectValidity(this.BuildingLogicData, type) > 0)
                        SceneManager.Instance.CreateCollectFX(this.BuildingBehavior, type, LogicController.Instance.Collect(id, type), this.BuildingLogicData);
                        //this.CreateCollectFX(type, LogicController.Instance.Collect(id, type));
                    break;
                case CollectMethod.Button:
                    if (SystemFunction.CheckCollectValidityByButton(this.BuildingLogicData, type))
                        SceneManager.Instance.CreateCollectFX(this.BuildingBehavior, type, LogicController.Instance.Collect(id, type), this.BuildingLogicData);
                        //this.CreateCollectFX(type, LogicController.Instance.Collect(id, type));
                    break;
            }
        }
    }
    protected virtual void AccelerateCollect()
    {
        if (this.BuildingLogicData.RemainResourceAccelerateTime < 0)
        {
           int costGem = CommonUtilities.MarketCalculator.GetResourceAccelerateCostGem(this.BuildingLogicData.BuildingType, this.BuildingLogicData.Level);
           string resourceContext = string.Format(StringConstants.PROMPT_ACCELERATE_RESOURCE, SystemFunction.TimeSpanToString(ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateLastTime), ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale);
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
                   print("加速收集");
                   LogicController.Instance.AddResourceAccelerate(this.BuildingLogicData.BuildingIdentity);
               }
           };
        }
    }

    //void CreateCollectFX(ResourceType type,int collected)
    //{
    //    SceneManager.Instance.CreateCollectFX(this.BuildingBehavior, type, collected, this.BuildingLogicData);
    //} 
    void OnAccelerateFX()
    {
        if(this.BuildingLogicData.RemainResourceAccelerateTime >= 0)
        {
            if (this.m_AccelerateCollectFX == null)
            {
                this.m_AccelerateCollectFX = SceneManager.Instance.CreateAccelerateFX(this.BuildingBehavior);
            }
        }
        else
        {
            if(this.m_AccelerateCollectFX != null)
                Destroy(this.m_AccelerateCollectFX);
        }
    }
    void CreateCollectIcon()
    {
        GameObject go = Instantiate(Resources.Load(BUILDING_RESOURCE_ICON, typeof(GameObject))) as GameObject;
        this.m_BoxCollider = go.GetComponent<BoxCollider>();
        this.m_ResourceIcon = go.transform.FindChild("ResourceIcon").GetComponent<tk2dSprite>();
        go.transform.parent = this.transform;
        this.m_ResourceIcon.color = Color.clear;
        this.m_ResourceIconBackground = go.transform.FindChild("IconBackground").GetComponent<tk2dSprite>();
        this.m_ResourceIconBackground.color = Color.clear;
        Vector3 localPosition = Vector3.zero;
        localPosition.x = this.BuildingBehavior.ProgressBarOffset.x + this.BuildingBehavior.ProgressBarSize.x * 0.5f;
        localPosition.y = this.BuildingBehavior.ProgressBarOffset.y;
        localPosition.z = -10;
        go.transform.localPosition = localPosition;
        ButtonListener buttonListener = go.GetComponent<ButtonListener>();
        buttonListener.Controller = this;
        buttonListener.Message = "OnClick";

    }
    void OnCollectIconState()
    {
        if (!LogicController.Instance.PlayerData.IsNewbie)
            this.CollectIconSate(SetCollectState);
        else
            this.CollectIconSate(SetCollectSateForNewbieGuide);
    }
    void CollectIconSate(Action<int> action)
    {

        switch (this.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.GoldMine:
                m_ResourceIcon.spriteId = m_ResourceIcon.GetSpriteIdByName(ClientSystemConstants.SCENE_RESOURCE_ICON_DICTIONARY[ResourceType.Gold]);
                if (this.BuildingLogicData.CurrentBuilidngState != BuildingEditorState.Normal || this.BuildingBehavior.IsClick)
                    action.Invoke(0);
                else
                    action.Invoke(SystemFunction.CheckCollectValidity(this.BuildingLogicData, ResourceType.Gold));
                break;
            case BuildingType.Farm:
                m_ResourceIcon.spriteId = m_ResourceIcon.GetSpriteIdByName(ClientSystemConstants.SCENE_RESOURCE_ICON_DICTIONARY[ResourceType.Food]);
                if (this.BuildingLogicData.CurrentBuilidngState != BuildingEditorState.Normal || this.BuildingBehavior.IsClick)
                    action.Invoke(0);
                else
                    action.Invoke(SystemFunction.CheckCollectValidity(this.BuildingLogicData, ResourceType.Food));
                break;
        }
        //if (this.BuildingLogicData.BuildingType == BuildingType.Farm)
        //{
        //    print("name =" + this.BuildingLogicData.Name);
        //    print("prefabName = " + this.gameObject);
        //    print("BuildingLogicData.CurrentStoreFood =" + BuildingLogicData.CurrentStoreFood);
        //}
    }
    void SetCollectState(int state)
    {//-1 = player capacity >= max capacity; 0 = current capacity < 1% ; 1 = enable collect
        switch (state)
        {
            case -1:
                //m_ResourceIcon.color = COLLECT_FULL_COLOR;
                m_ResourceIcon.color = Color.white;
                m_ResourceIconBackground.color = COLLECT_FULL_COLOR;
                this.m_BoxCollider.enabled = false;
                break;
            case 0:
                m_ResourceIcon.color = Color.clear;
                m_ResourceIconBackground.color = Color.clear;
                this.m_BoxCollider.enabled = false;
                break;
            case 1:
                m_ResourceIconBackground.color = Color.white;
                m_ResourceIcon.color = Color.white;
                this.m_BoxCollider.enabled = true;
                break;
        }
    }
    void SetCollectSateForNewbieGuide(int state)
    {
        this.m_BoxCollider.enabled = false;
        switch (state)
        {
            case -1:
                //m_ResourceIcon.color = COLLECT_FULL_COLOR;
                if (this.gameObject.tag == NewbieGuide.TAG_HIGHTLIGHT)
                {
                    m_ResourceIcon.color = Color.white;
                    m_ResourceIconBackground.color = COLLECT_FULL_COLOR;
                }
                else
                {
                    m_ResourceIcon.color = Color.gray;
                    Color col = COLLECT_FULL_COLOR / 2;
                    col.a = COLLECT_FULL_COLOR.a;
                    m_ResourceIconBackground.color = col;
                }
                break;

            case 0:
                m_ResourceIcon.color = Color.clear;
                m_ResourceIconBackground.color = Color.clear;
                break;
            case 1:
                if (this.gameObject.tag == NewbieGuide.TAG_HIGHTLIGHT)
                {
                    m_ResourceIconBackground.color = Color.white;
                    m_ResourceIcon.color = Color.white;
                }
                else
                {
                    m_ResourceIconBackground.color = Color.gray;
                    m_ResourceIcon.color = Color.gray;
                }
                break;
        }
    }
}
public enum CollectMethod
{
    Building,
    Button
}
