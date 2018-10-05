using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System.Linq;
public class RemovableObjectCommon : BuildingCommon {
    public RemovableObjectBehavior RemovableObjectBehavior { get; set; }
    public RemovableObjectLogicData RemovableObjectLogicData { get; set; }
    GameObject m_ReadyForRemoveFX;
	// Use this for initialization
	new void Start () {
        this.CreateComponent();
        base.Initial();
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.ShowProgress();
        this.OnReadyForCompleteFx();
	}
    //Button message
    void OnRemoveObject()
    {
        UIManager.Instance.UIWindowSelectBuilder.RemovableObjectLogicData = this.RemovableObjectLogicData;
        UIManager.Instance.UIWindowSelectBuilder.BuilderMenuType =  BuilderMenuType.RemoveObject;
        UIManager.Instance.UIWindowSelectBuilder.ShowWindow();
    }
    //Button message
    void OnCancelRemoveObject()
    {
        LogicController.Instance.CancelRemove(this.RemovableObjectLogicData.RemovableObjectNo);
        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        UIManager.Instance.ShowPopupBtnByCurrentSelect();
    }
    public void OnReadyForCompleteFx()
    {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (!base.BuildingBehavior.Created)
            return;
        if (this.RemovableObjectLogicData.EditorState == RemovableObjectEditorState.ReadyForComplete)
        {
            if (this.m_ReadyForRemoveFX == null)
            {
                this.CreateReadyForRemoveFX();
                if (SceneManager.Instance.PickableObjectCurrentSelect == this.RemovableObjectBehavior)
                {
                    this.RemovableObjectBehavior.OnUnSelect(true);
                    SceneManager.Instance.PickableObjectCurrentSelect = null;
                }

                //UIManager.Instance.HidePopuBtnByCurrentSelect(true);
                //if (SceneManager.Instance.PickableObjectCurrentSelect as RemovableObjectBehavior == this.GetComponent<RemovableObjectBehavior>())
                //    SceneManager.Instance.PickableObjectCurrentSelect = null;
            }
        }
        else
        {
            if (this.m_ReadyForRemoveFX != null)
                Destroy(this.m_ReadyForRemoveFX);
        } 
    }
    //public void OnReadyForComplete()
    //{ 
    //    RemovableObjectEditorState.o
    //    if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate)
    //    {
    //        if (SceneManager.Instance.PickableObjectCurrentSelect as BuildingBehavior == this)
    //        {
    //            UIManager.Instance.HidePopuBtnByCurrentSelect(true); 
    //        }

    //    }
    //}
    void CreateReadyForRemoveFX()
    {
       this.m_ReadyForRemoveFX = SceneManager.Instance.CreateReadyForRemoveFX(this.RemovableObjectBehavior);
    }
    protected override void ShowProgress()
    {
        if (!base.BuildingBehavior.Created)
            return;
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.RemovableObjectLogicData.EditorState == RemovableObjectEditorState.Removing)
        {
            m_ProgressBarBehaviorDictionary[4].gameObject.SetActive(true);
            m_ProgressBarBehaviorDictionary[4].SetProgressPosition(0);
            m_ProgressBarBehaviorDictionary[4].SetProgressBar((this.RemovableObjectLogicData.RemoveWorkload - this.RemovableObjectLogicData.RemoveRemainingWorkload) / this.RemovableObjectLogicData.RemoveWorkload, this.RemovableObjectLogicData.RemoveRemainingWorkload / this.RemovableObjectLogicData.AttachedBuilderEfficiency, false, string.Empty);
        }
        else
            m_ProgressBarBehaviorDictionary[4].gameObject.SetActive(false);
    }
    protected override void CreateComponent()
    {
        GameObject progressBarBehaviorGo = Resources.Load(PROGRESS_PREFAB_NAME, typeof(GameObject)) as GameObject;
        m_ProgressBarBehaviorDictionary.Add(4, (Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>()); 
    }
    //Button message 
    protected override void OnConstructBuilding()
    {
        if (base.BuildingBehavior.EnableCreate)
        {

            if (this.RemovableObjectBehavior.ProductRemovableObjectConfigData.GemPrice > LogicController.Instance.PlayerData.CurrentStoreGem)
            {
                int costBalanceGem = this.RemovableObjectBehavior.ProductRemovableObjectConfigData.GemPrice - LogicController.Instance.PlayerData.CurrentStoreGem;
                string title = StringConstants.PROMT_REQUEST_RESOURCE + StringConstants.COIN_GEM;
                string resourceContext = string.Format(StringConstants.PROMPT_RESOURCE_COST, costBalanceGem, StringConstants.COIN_GEM);

                UIManager.Instance.UIWindowCostPrompt.ShowWindow(costBalanceGem, resourceContext, title);
                UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
                UIManager.Instance.UIWindowCostPrompt.Click += () =>
                {
                    UIManager.Instance.UIWindowFocus = null;
                    UIManager.Instance.UISelectShopMenu.GoShopping();
                };
                return;
            }
            if (LogicController.Instance.AllRemovableObjects.Count(a => a.IsCountable) < ConfigInterface.Instance.SystemConfig.MaxRemovableObjectNumber)
            {
                this.RemovableObjectLogicData = LogicController.Instance.BuyRemovableObject(this.RemovableObjectBehavior.RemovableObjectType, this.RemovableObjectBehavior.FirstZoneIndex);
                this.RemovableObjectBehavior.RemovableObjectLogicData = this.RemovableObjectLogicData;
                base.BuildingBehavior.Created = true;
                this.RemovableObjectBehavior.BuildObstacle();
                SceneManager.Instance.BuildingBehaviorTemporary = null;
                base.BuildingBehavior.IsClick = false;
                base.BuildingBehavior.SetArrowState(false);
                this.RemovableObjectBehavior.DestroyButton();
               
                SceneManager.Instance.CreateSmokeFX(this.RemovableObjectBehavior);
            }
            else 
                UIErrorMessage.Instance.ErrorMessage(35); 
           
        }
    }
    public void OnConstructBuildingByNewbieGuide()
    {
        this.OnConstructBuilding();
    }

}
