using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using ConfigUtilities.Structs;
public class DefenseObjectBehavior : BuildingBehavior
{
    public DefenseObjectConfigWrapper DefenseObjectConfigData { get; set; }
    public DefenseObjectLogicData DefenseObjectLogicData { get; set; }
    public PropsLogicData PropsLogicData { get; set; }
    // Use this for initialization
    void Start()
    {
        this.CreateDefenseObjectObstacle();
        base.CreateCell();
        base.CreateArrow();
        this.CreateComponent();
        this.InitBuildPosition();
        base.CreateButton();
        base.SetButtonOkState();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    #region Ngui interactive
    public override void OnClick()
    {
        if (!this.enabled || !UIManager.Instance.SceneFocus || !CheckBuildingCreateStack() || !this.Created || !base.EnableCreate || base.MouseOrTouchDictionary.Count > 1)
        {
            base.FirstZoneIndexPrevious = base.FirstZoneIndex;
            return;
        }

        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious) && (base.PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude < 16 && base.m_AllowClick)
        {
            //if (!CheckBuildingCreateStack())
            //    return;
            base.IsClick = !base.IsClick;
            this.SetAttackRange(base.IsClick); 
            base.SetArrowState(base.IsClick);
           base.OnActiveBuildingFX(base.IsClick);
        }
        else
        {
            if (!base.IsClick)
                CameraManager.Instance.OnClick();
            base.FirstZoneIndexPrevious = base.FirstZoneIndex;
        }

        if (!base.CheckBuildingCreateStack())
            return;
        if (base.IsClick && base.IsBuild)
            AudioController.Play("BuildingPick");
        if (base.EnableCreate && !base.IsBuild) 
            this.Build();
        if (base.IsClick)
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect != null && SceneManager.Instance.PickableObjectCurrentSelect != this)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = this;
            if (base.IsBuild)
                UIManager.Instance.ShowPopupBtnByCurrentSelect();
        }
        else
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect == this)
                UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        }
    }
    public override void OnUnSelect(bool isCancel)
    {
        print("DefenseObjectBehavior OnUnSelect");
        if (!this.Created)
            return;

        base.IsClick = false;
        this.SetAttackRange(base.IsClick); 
        base.SetArrowState(false);
        this.ResetBuild();
        if (isCancel)
            UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        else
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        base.OnActiveBuildingFX(false);
    }
    void OnDrag(Vector2 v)
    {
        if (!this.enabled )
            return;
        if (base.IsClick && SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
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
    //    print("DefenseObjectBehavior OnPress");
    //    SceneManager.Instance.OnPressGloble(isPressed);
    //    if (isPressed)
    //    {
    //        base.m_AllowClick = true;
    //        if (!base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
    //            base.MouseOrTouchDictionary.Add(UICamera.currentTouchID, UICamera.currentTouch);
    //    }
    //    else
    //        if (base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
    //            base.MouseOrTouchDictionary.Remove(UICamera.currentTouchID);

    //    base.SetTouchDownZone();
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
    //void OnDrop(GameObject go)
    //{
    //    if (SceneManager.Instance.PickableObjectCurrentSelect != null)
    //        SceneManager.Instance.PickableObjectCurrentSelect.OnClick();
    //}

    #endregion
    void CreateDefenseObjectObstacle()
    {
        if (!this.Created)
            m_RealBuildObstalceList = this.DefenseObjectConfigData.BuildingObstacle;
        else
            m_RealBuildObstalceList = this.DefenseObjectLogicData.BuildingObstacleList;
    }
    void CreateComponent()
    {
        base.CreateBuildingTitle();
    }
    void InitBuildPosition()
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
            base.FirstZoneIndex = this.DefenseObjectLogicData.Position;
            this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
            this.Build();
            base.SetArrowState(false);
        }
    }
    new void Build()
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = false;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = this.gameObject;
        }

        if (!base.FirstZoneIndex.Equals(this.DefenseObjectLogicData.Position))
        {
            LogicController.Instance.MoveDefenseObject(this.DefenseObjectLogicData.DefenseObjectID, base.FirstZoneIndex);
            base.OnCreateBorder(BorderType.BuildingOutlineBorder);
        }
        else
        {
            base.OnDestroyBorder();
        }
        this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
        base.m_IsBuild = true;
        base.EnableCreate = true;
        //this.CreateDropDownFX();
        base.SetCellVisible(false);

    }
    void UnBuild(bool showBorder = true)
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = true;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = null;
        }
        base.m_IsBuild = false;
        if (showBorder)
            base.OnCreateBorder();
    }
    //Button message
    void OnConstructBuilding()
    {
        if (base.EnableCreate)
        {
            this.DefenseObjectLogicData = LogicController.Instance.AddDefenseObject(this.PropsLogicData.PropsNo, base.FirstZoneIndex);
            base.Created = true;
            this.Build();
            SceneManager.Instance.BuildingBehaviorTemporary = null;
            base.IsClick = false;
            base.SetArrowState(false);
            base.DestroyButton();
        }
    }
    void OnMoveBuilding()
    {
        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious))
            if (base.IsBuild)
            {
                this.UnBuild();
                UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            }
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
    new void ResetBuild()
    { 
        if (!this.Created)
            return;
        if (!base.IsBuild)
        {
            if (base.CheckTile())
                this.Build();
            else
            {
                if (this.DefenseObjectLogicData.Position != null)
                {
                    base.FirstZoneIndex = this.DefenseObjectLogicData.Position;
                    this.Build(); 
                }
            }
        }
    }
    public override void ShowBuildingTitle(bool isShow)
    {
        if (isShow)
        {
            base.m_TextMeshBuildingTitle.text = "   " + this.DefenseObjectLogicData.Name;
            Vector3 localPosition = Vector3.zero;
            localPosition.x = this.ProgressBarOffset.x + m_BuildingTitleOffset.x;
            localPosition.y = this.ProgressBarOffset.y + m_BuildingTitleOffset.y;
            localPosition.z = -100;
            this.m_TextMeshBuildingTitle.transform.localPosition = localPosition;
        }
        this.m_TextMeshBuildingTitle.color = isShow ? Color.white : Color.clear;
        this.m_TextMeshBuildingTitle.Commit();
    }
    void SetAttackRange(bool active)
    {
        if (this.m_AttackRangeFX != null)
        {
            if (active)
                m_AttackRangeFX.ShowAttackRange(this.DefenseObjectLogicData.TriggerScope, 0);
            else
                m_AttackRangeFX.HideAttackRange(this.DefenseObjectLogicData.TriggerScope, 0);
        }
    }
    //Button message 
    void OnPropsDestroy()
    {
        LogicController.Instance.DestroyDefenseObject(this.DefenseObjectLogicData.DefenseObjectID);
        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
            SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
        SceneManager.Instance.PickableObjectCurrentSelect = null;
        this.UnBuild(false);
        Destroy(this.gameObject);
        this.enabled = false;
    }
}
