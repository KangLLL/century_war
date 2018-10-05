using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommandConsts;
using ConfigUtilities.Structs;
public class RemovableObjectBehavior : BuildingBehavior
{
    public RemovableObjectLogicData RemovableObjectLogicData { get; set; }
    public RemovableObjectConfigData RemovableObjectConfigData { get; set; }
    public ProductRemovableObjectConfigData ProductRemovableObjectConfigData { get; set; }
    public RemovableObjectType RemovableObjectType { get; set; }
    RemovableObjectCommon m_RemovableObjectCommon;
    public RemovableObjectCommon RemovableObjectCommon { get { return m_RemovableObjectCommon; } }
	void Start () 
    {
        this.CreateRemovableObjectObstacle();
        base.CreateCell();
        base.CreateArrow();
        this.CreateComponent();
        this.InitObstaclePosition();
        base.CreateButton();
        base.SetButtonOkState();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    //void InitObstaclePosition()
    //{
    //    if (this.RemovableObjectLogicData.BuildingPosition != null)
    //    {
    //        base.FirstZoneIndex = this.RemovableObjectLogicData.BuildingPosition; 
    //        this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
    //        this.BuildObstacle();
    //    }
    //}
    void InitObstaclePosition()
    {
        if (!this.Created)
        {
            base.FirstZoneIndex = PositionConvertor.GetBuildingTileIndexFromWorldPosition(CameraManager.Instance.MainCamera.transform.position);
            if (!base.CheckTile())
                base.SearchBuildingPosition();
            this.CheckTile();
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
            position.z = BUILDING_PICK_AXIS_Z;
            this.transform.position = position;
            base.SetArrowState(true);
            base.IsClick = true; 
        }
        else
        { 
            if (this.RemovableObjectLogicData.BuildingPosition != null)
            {
                base.FirstZoneIndex = this.RemovableObjectLogicData.BuildingPosition;
                this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
                this.BuildObstacle();
                base.SetArrowState(false);
            }
        }
    }
    void LateUpdate()
    {
        if (!base.Created)
            return;
        if (this.RemovableObjectLogicData.BuildingPosition == null)
        {
            if (LogicController.Instance.GeneratePosition(this.RemovableObjectLogicData.RemovableObjectNo, SceneManager.Instance))
            {
                base.FirstZoneIndex = this.RemovableObjectLogicData.BuildingPosition;
                this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
                this.BuildObstacle();
                base.SetArrowState(false);
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
      
    }
 
    public void BuildObstacle()
    {
        for (int i = 0; i < this.RemovableObjectLogicData.BuildingObstacleList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + this.RemovableObjectLogicData.BuildingObstacleList[i].Row, base.FirstZoneIndex.Column + this.RemovableObjectLogicData.BuildingObstacleList[i].Column] = false;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + this.RemovableObjectLogicData.BuildingObstacleList[i].Row, base.FirstZoneIndex.Column + this.RemovableObjectLogicData.BuildingObstacleList[i].Column] = this.gameObject;
        }

        for (int i = 0; i < this.RemovableObjectLogicData.ActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[this.RemovableObjectLogicData.ActorPosition.Row + this.RemovableObjectLogicData.ActorObstacleList[i].Row, this.RemovableObjectLogicData.ActorPosition.Column + this.RemovableObjectLogicData.ActorObstacleList[i].Column] = false;
            SceneManager.Instance.ActorGameObjectData[this.RemovableObjectLogicData.ActorPosition.Row + this.RemovableObjectLogicData.ActorObstacleList[i].Row, this.RemovableObjectLogicData.ActorPosition.Column + this.RemovableObjectLogicData.ActorObstacleList[i].Column] = this.gameObject;
        }
        base.SetCellVisible(false);
        base.m_IsBuild = true;
        base.EnableCreate = true;
        this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
    }
    void UnBuildObstacle()
    {
        for (int i = 0; i < this.RemovableObjectLogicData.BuildingObstacleList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + this.RemovableObjectLogicData.BuildingObstacleList[i].Row, base.FirstZoneIndex.Column + this.RemovableObjectLogicData.BuildingObstacleList[i].Column] = true;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + this.RemovableObjectLogicData.BuildingObstacleList[i].Row, base.FirstZoneIndex.Column + this.RemovableObjectLogicData.BuildingObstacleList[i].Column] = null;
        }

        for (int i = 0; i < this.RemovableObjectLogicData.ActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[this.RemovableObjectLogicData.ActorPosition.Row + this.RemovableObjectLogicData.ActorObstacleList[i].Row, this.RemovableObjectLogicData.ActorPosition.Column + this.RemovableObjectLogicData.ActorObstacleList[i].Column] = true;
            SceneManager.Instance.ActorGameObjectData[this.RemovableObjectLogicData.ActorPosition.Row + this.RemovableObjectLogicData.ActorObstacleList[i].Row, this.RemovableObjectLogicData.ActorPosition.Column + this.RemovableObjectLogicData.ActorObstacleList[i].Column] = null;
        }
    }
    void CreateRemovableObjectObstacle()
    {
        if (!this.Created)
            base.m_RealBuildObstalceList = PositionConvertor.TilePointListToTilePositionList(this.RemovableObjectConfigData.BuildingObstacleList);
        else
            base.m_RealBuildObstalceList = this.RemovableObjectLogicData.BuildingObstacleList;
    }
    void CreateComponent()
    {
        this.m_RemovableObjectCommon = this.gameObject.AddComponent<RemovableObjectCommon>();
        m_RemovableObjectCommon.RemovableObjectLogicData = this.RemovableObjectLogicData;
        m_RemovableObjectCommon.RemovableObjectBehavior = this;
        m_RemovableObjectCommon.BuildingBehavior = this;
        m_RemovableObjectCommon.ProgressBarOffset = this.ProgressBarOffset;
        m_RemovableObjectCommon.ProgressBarSize = this.ProgressBarSize;
        base.CreateBuildingTitle();
        //this.CreateBuildingTitle();
    }
    //void CreateBuildingTitle()
    //{
    //    this.m_TextMeshBuildingTitle = (Instantiate(Resources.Load(TEXT_BUILDING_TITLE, typeof(GameObject))) as GameObject).GetComponent<tk2dTextMesh>();
    //    this.m_TextMeshBuildingTitle.transform.parent = this.transform;
    //    this.m_TextMeshBuildingTitle.color = Color.clear;
    //    this.m_TextMeshBuildingTitle.Commit();
    //}
    public override void OnClick()
    {
        if (!this.enabled || !base.Created || SceneManager.Instance.SceneMode == SceneMode.SceneVisit || !UIManager.Instance.SceneFocus || !base.EnableCreate || !base.CheckBuildingCreateStack() || this.ReadyForRemove())
        {
            base.FirstZoneIndexPrevious = base.FirstZoneIndex;
            return;
        }
        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious) && (base.PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude < 16 && base.m_AllowClick)
        {
            base.IsClick = !base.IsClick;
            //this.m_RemovableObjectCommon.OnActiveBuildingFX(base.IsClick);
            base.OnActiveBuildingFX(base.IsClick);
        }
        else
        {
            //if (!base.IsClick)
                CameraManager.Instance.OnClick();
        }

        //if (!base.CheckBuildingCreateStack())
        //    return;
        if (base.IsClick && base.IsBuild)
            AudioController.Play("BuildingPick");
        if (base.IsClick)
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect != null && SceneManager.Instance.PickableObjectCurrentSelect != this)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = this;
            //if (this.m_IsBuild)
                UIManager.Instance.ShowPopupBtnByCurrentSelect();
        }
        else
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect == this)
                UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        }
    }
    void OnDrag(Vector2 v)
    {
        if (!this.enabled)
            return;
        if (!base.Created && SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
            this.OnMoveBuilding();
        else
        {
            if (SceneManager.Instance.MouseOrTouchDictionaryGloble.Count < 2)
                CameraManager.Instance.OnDragExt(base.MouseOrTouchDictionary);
            else
            {
                CameraManager.Instance.OnDragExt(SceneManager.Instance.MouseOrTouchDictionaryGloble);
                base.m_AllowClick = false;
            }
        }
    }
    void OnDrop(GameObject go)
    {
        CameraManager.Instance.OnClick();
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
            SceneManager.Instance.PickableObjectCurrentSelect.OnClick();
    }
    //void OnPress(bool isPressed)
    //{
    //    SceneManager.Instance.OnPressGloble(isPressed);
    //    if (isPressed)
    //    {
    //        this.m_AllowClick = true;
    //        if (!base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
    //            base.MouseOrTouchDictionary.Add(UICamera.currentTouchID, UICamera.currentTouch);
    //    }
    //    else
    //        if (base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
    //            base.MouseOrTouchDictionary.Remove(UICamera.currentTouchID);

    //    if (!base.IsClick)
    //    {
    //        CameraManager.Instance.OnPress(isPressed);
    //    }
    //    if (isPressed)
    //    {
    //        base.FirstZoneIndexPrevious = base.FirstZoneIndex;
    //        base.PressPosition = CameraManager.Instance.MainCamera.transform.position;
    //    }
    //}
    //public bool CheckBuildingCreateStack()
    //{
    //    if (SceneManager.Instance.BuildingBehaviorTemporary)
    //        return SceneManager.Instance.BuildingBehaviorTemporary.Equals(this);
    //    return true;
    //}

    public override void OnUnSelect(bool isCancel)
    {
        base.IsClick = false;
        if (isCancel)
            UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        else
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        base.OnActiveBuildingFX(false);
    }

    public override void ShowBuildingTitle(bool isShow)
    {
        if (isShow)
        {
            this.m_TextMeshBuildingTitle.text = this.RemovableObjectLogicData.Name;

            int order = 0;
            if (this.RemovableObjectLogicData.EditorState == RemovableObjectEditorState.Removing)
                order++;

            Vector3 localPosition = Vector3.zero;
            localPosition.x = this.ProgressBarOffset.x + m_BuildingTitleOffset.x;//this.ProgressBarSize.x * 0.5f;
            localPosition.y = this.ProgressBarOffset.y + m_BuildingTitleOffset.y + order * ClientConfigConstants.Instance.ProgressBarInterval;
            localPosition.z = -10;
            this.m_TextMeshBuildingTitle.transform.localPosition = localPosition;
        }
        //this.m_TextMeshBuildingTitle.GetComponent<TweenColortk2dSprite>().Play(isShow);
        this.m_TextMeshBuildingTitle.color = isShow ? Color.white : Color.clear;
        this.m_TextMeshBuildingTitle.Commit();
    }

    bool ReadyForRemove()
    {
        if (this.RemovableObjectLogicData.EditorState == RemovableObjectEditorState.ReadyForComplete)
        {
            if (LogicController.Instance.AllProps.Count + (this.RemovableObjectLogicData.RewardProps.HasValue ? 1 : 0) > LogicController.Instance.PlayerData.PropsMaxCapacity)
            {
                UIErrorMessage.Instance.ErrorMessage(26);
                return true;
            }

            RewardConfigData rewardConfigData = LogicController.Instance.FinishRemove(this.RemovableObjectLogicData.RemovableObjectNo);
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            this.UnBuildObstacle();
            if (SceneManager.Instance.PickableObjectCurrentSelect != null)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = null;
            Destroy(this.gameObject);
            this.CreateRemovedFX(rewardConfigData.RewardGold, rewardConfigData.RewardFood, rewardConfigData.RewardOil, rewardConfigData.RewardGem, rewardConfigData.RewardExp);
            if (this.RemovableObjectLogicData.RewardPropsType.HasValue)
                this.CreatePropFX(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(this.RemovableObjectLogicData.RewardPropsType.Value));
            SceneManager.Instance.CreateObstacleUpgradeFX(this);
            return true;
        }
        return false;
       
    }
    //get gem animation
    void CreateRemovedFX(params int[] reward)
    {
        SceneManager.Instance.CreateAwardFX(this.BuildingAnchor, reward);
    }
    void CreatePropFX(PropsConfigData propsConfigData)
    {
        SceneManager.Instance.CreatePropFX(this.BuildingAnchor, propsConfigData);
    }
    public new void DestroyButton()
    {
        base.DestroyButton();
    }
    void OnMoveBuilding()
    {
        if (base.MouseOrTouchDictionary.Count > 0)
        {
            List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(base.MouseOrTouchDictionary.Values);
            TilePosition touchDownZone = PositionConvertor.GetBuildingTileIndexFromScreenPosition(mouseOrTouchList[0].pos);
            TilePosition offsetZone = touchDownZone - base.TouchDownZone;
            if (!((base.FirstZoneIndexPrevious + offsetZone).Equals(base.FirstZoneIndex)))
                AudioController.Play("BuildingMoving");
            base.FirstZoneIndex = base.FirstZoneIndexPrevious + offsetZone;
            this.LimitBuildingPosition();
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
            position.z = BUILDING_PICK_AXIS_Z;
            this.transform.position = position;
            base.CheckTile();
            base.SetButtonOkState();
        }
    }
}
