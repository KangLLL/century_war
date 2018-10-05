using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using System;

public class WallBehavior : BuildingBehavior
{
    // Use this for initialization
    tk2dSprite[] m_Tk2dSpriteWall;//0 = Top wall; 1 = Right wall;
    BoxCollider m_BoxCollider;
    Vector3 m_BoxColliderSize;
	//bool m_AllowClick = true;
    //bool m_EnableBuildAll = false;
    protected override void Awake()
    {
        base.Awake();
        this.GetComponent();
        this.InitialWall();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
     
    #region Ngui interactive 
    public override void OnClick()
    {
        print("WallBehavior OnClick");
        if (!UIManager.Instance.SceneFocus || !base.Created || !base.EnableCreate || base.MouseOrTouchDictionary.Count > 1)
        { 
            if (SceneManager.Instance.SelectedAllWallList.Count == 0) 
                base.FirstZoneIndexPrevious = base.FirstZoneIndex; 

            //for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
            //{
            //    print("for================");
            //    BuildingBehavior buildingBehavior = SceneManager.Instance.SelectedAllWallList[i];
            //    buildingBehavior.FirstZoneIndexPrevious = buildingBehavior.FirstZoneIndex;
            //}
            return;
        }
        bool clickAllWallState = true;
        if (SceneManager.Instance.SelectedAllWallList.Contains(this))
        {
            print("SceneManager.Instance.SelectedAllWallList.Contains");
            for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
            {
                BuildingBehavior buildingBehavior = SceneManager.Instance.SelectedAllWallList[i];
                buildingBehavior.FirstZoneIndexPrevious = buildingBehavior.FirstZoneIndex;
            }
           //base.FirstZoneIndexPrevious = base.FirstZoneIndex;
            clickAllWallState = this.OnClickAllWall();
            if (!clickAllWallState) 
                return;
            else
                UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        }
        else
            clickAllWallState = false;
        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious) && (base.PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude < 16 && this.m_AllowClick || clickAllWallState)
        {
            if (!base.CheckBuildingCreateStack())
                return;
            base.IsClick = !base.IsClick;
            base.SetArrowState(base.IsClick);
            base.OnActiveBuildingFX(base.IsClick);
        }
        else
        {
            if (!base.IsClick)
            {
                print("WallBehavior  CameraManager.Instance.OnClick();");
                CameraManager.Instance.OnClick();
            }
            if (SceneManager.Instance.SelectedAllWallList.Count == 0)
                base.FirstZoneIndexPrevious = base.FirstZoneIndex;
        }
        if (!base.CheckBuildingCreateStack())
            return;
        if (base.IsClick && base.IsBuild)
            AudioController.Play("BuildingPick");
        if (base.EnableCreate && !base.IsBuild)
            base.Build();
        //this.BuildingBehavior.SetCellVisible(!this.BuildingBehavior.IsBuild);

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
        this.AjustBoxColliderSize(base.IsClick);
    }  
    void OnDrag(Vector2 v)
    {
        if (SceneManager.Instance.SelectedAllWallList.Count == 0)
        {
            if (base.IsClick && SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
                this.OnMoveWall();
            else
            {
                if (SceneManager.Instance.MouseOrTouchDictionaryGloble.Count < 2)
                    CameraManager.Instance.OnDragExt(base.MouseOrTouchDictionary);
                else
                {
                    CameraManager.Instance.OnDragExt(SceneManager.Instance.MouseOrTouchDictionaryGloble);
                    this.m_AllowClick = false;
                }
            }
        }
        else
            if (SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
                this.OnMoveAllWall();
    }
    void OnPress(bool isPressed)
    {
        SceneManager.Instance.OnPressGloble(isPressed);
        if (isPressed)
        {
            this.m_AllowClick = true;
            if (!base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                base.MouseOrTouchDictionary.Add(UICamera.currentTouchID, UICamera.currentTouch);
        }
        else
            if (base.MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                base.MouseOrTouchDictionary.Remove(UICamera.currentTouchID);

        base.SetTouchDownZone();
        if (!base.IsClick)
        {
            CameraManager.Instance.OnPress(isPressed);
        }
        if (isPressed)
        {
            base.FirstZoneIndexPrevious = base.FirstZoneIndex;
            base.PressPosition = CameraManager.Instance.MainCamera.transform.position;
        }
        this.OnPressAllWall(isPressed);
    }
    void OnDrop(GameObject go)
    {
        print("WallBehavior OnDrop");
        CameraManager.Instance.OnClick();
        if (SceneManager.Instance.PickableObjectCurrentSelect != null) 
            SceneManager.Instance.PickableObjectCurrentSelect.OnClick(); 
    }
    #endregion
    void OnMoveWall()
    {
        print("OnMoveWall");
        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious))//this.BuildingBehavior.BuildingLogicData.BuildingPosition))
            if (base.IsBuild)
            {
                base.UnBuild();
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
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
            position.z = base.BUILDING_PICK_AXIS_Z;
            this.transform.position = position;
            //this.BuildingBehavior.EnableCreate = 
            base.CheckTile();
            base.SetButtonOkState();
        }
    }
    public void OnUnSelectAllWall(bool isCancel)
    {
        bool enableBuild = SceneManager.Instance.GetAllBuildingEnableState();
        for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
        {
            WallBehavior wallBehavior = SceneManager.Instance.SelectedAllWallList[i];
            wallBehavior.SetArrowState(false);
            wallBehavior.IsClick = false;
            this.RebuildWall(wallBehavior, enableBuild); 
            wallBehavior.OnActiveBuildingFX(false);
            wallBehavior.AjustBoxColliderSize(false);
        }
        //this.m_EnableBuildAll = false; 
        SceneManager.Instance.DestroyBuildingBorder();
        UIManager.Instance.HidePopuBtnByCurrentSelect(isCancel); 
        SceneManager.Instance.ClearAllWallList();
    }
    public override void OnUnSelect(bool isCancel)
    {
        if (!this.Created)
            return;

        if (SceneManager.Instance.SelectedAllWallList.Count > 0)
        {
            this.SetArrowPosition(true);
            this.OnUnSelectAllWall(isCancel);
            return;
        }
        else
            this.AjustBoxColliderSize(false);
        base.IsClick = false;
        base.SetArrowState(false);
        this.ResetBuild();

        if (isCancel)
            UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        else
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        base.OnActiveBuildingFX(false);
    }
    public void ResetBuildingArrow()
    {
        print("ResetBuildingArrow=================");
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
            ((WallBehavior)SceneManager.Instance.PickableObjectCurrentSelect).SetArrowPosition(true);
            //SceneManager.Instance.PickableObjectCurrentSelect.SetArrowPosition(true);
       
    }
    void RebuildWall(BuildingBehavior buildingBehavior, bool enableBuild)
    {  
        if (enableBuild) 
            buildingBehavior.Build();
        else
            if (buildingBehavior.BuildingLogicData.BuildingPosition != null)
            {
                buildingBehavior.FirstZoneIndex = buildingBehavior.BuildingLogicData.BuildingPosition;
                buildingBehavior.Build();
            }
    }

    public void OnRotateWallRow()
    {
        
        switch (SceneManager.Instance.WallSelectedMode)
        {
            case WallSelectedMode.Row:
                this.RotateWallRow(SceneManager.Instance.WallListLeft);
                this.RotateWallRow(SceneManager.Instance.WallListRight);
                break;
            case WallSelectedMode.Column:
                this.RotateWallRow(SceneManager.Instance.WallListTop);
                 this.RotateWallRow(SceneManager.Instance.WallListBottom);
                break;
        }
        if (base.FirstZoneIndex.Equals(base.BuildingLogicData.BuildingPosition))
            if (base.IsBuild)
                base.UnBuild();
 
        this.SetSelectWallState();
        this.SetArrowPosition(false);
    }
    void RotateWallRow(List<WallBehavior> wallBehaviorList)
    {
        if (wallBehaviorList.Count > 0)
        {
           
            for (int i = 0; i < wallBehaviorList.Count; i++)
            {
                if (wallBehaviorList[i].FirstZoneIndex.Equals(wallBehaviorList[i].BuildingLogicData.BuildingPosition))
                    if (wallBehaviorList[i].IsBuild)
                        wallBehaviorList[i].UnBuild();

            }
            if ((base.FirstZoneIndex.Row - wallBehaviorList[0].FirstZoneIndex.Row) == 0)
            {
                //degree = 0
                if (wallBehaviorList[0].FirstZoneIndex.Column > base.FirstZoneIndex.Column)
                {
                    wallBehaviorList.Sort((a, b) => a.FirstZoneIndex.Column - b.FirstZoneIndex.Column);
                    for (int i = 0; i < wallBehaviorList.Count; i++)
                    {
                        wallBehaviorList[i].FirstZoneIndex = new TilePosition(base.FirstZoneIndex.Column, base.FirstZoneIndex.Row - 1 - i);
                        wallBehaviorList[i].transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(wallBehaviorList[i].FirstZoneIndex);
                        wallBehaviorList[i].CheckTile(); 
                    }
                }
                else
                { 
                    //degree = 180
                    wallBehaviorList.Sort((a, b) => b.FirstZoneIndex.Column - a.FirstZoneIndex.Column);
                    for (int i = 0; i < wallBehaviorList.Count; i++)
                    {
                        wallBehaviorList[i].FirstZoneIndex = new TilePosition(base.FirstZoneIndex.Column, base.FirstZoneIndex.Row + 1 + i);
                        wallBehaviorList[i].transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(wallBehaviorList[i].FirstZoneIndex);
                        wallBehaviorList[i].CheckTile();
                    }
                }
            }
            else
            {
                //degree = 90 
                if (wallBehaviorList[0].FirstZoneIndex.Row > base.FirstZoneIndex.Row)
                {
                    wallBehaviorList.Sort((a, b) => a.FirstZoneIndex.Row - b.FirstZoneIndex.Row);
                    for (int i = 0; i < wallBehaviorList.Count; i++)
                    {
                        wallBehaviorList[i].FirstZoneIndex = new TilePosition(base.FirstZoneIndex.Column + 1 + i, base.FirstZoneIndex.Row);
                        wallBehaviorList[i].transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(wallBehaviorList[i].FirstZoneIndex);
                        wallBehaviorList[i].CheckTile();
                    }
                }
                else
                {
                    //degree =  270
                    wallBehaviorList.Sort((a, b) => b.FirstZoneIndex.Row - a.FirstZoneIndex.Row);
                    for (int i = 0; i < wallBehaviorList.Count; i++)
                    {
                        wallBehaviorList[i].FirstZoneIndex = new TilePosition(base.FirstZoneIndex.Column - 1 - i, base.FirstZoneIndex.Row);
                        wallBehaviorList[i].transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(wallBehaviorList[i].FirstZoneIndex);
                        wallBehaviorList[i].CheckTile();
                    }
                }
            }
        }

    }

    void SetSelectWallState()
    {
        if (SceneManager.Instance.SelectedAllWallList.Count > 1)
        { 
            List<WallBehavior> wallBehaviorList = SceneManager.Instance.SelectedAllWallList;
            if ((wallBehaviorList[0].FirstZoneIndex.Row - wallBehaviorList[1].FirstZoneIndex.Row) == 0)
            {//degree = 0
                wallBehaviorList.Sort((a, b) => a.FirstZoneIndex.Column - b.FirstZoneIndex.Column);
                for (int i = 0; i < wallBehaviorList.Count; i++)
                {
                    wallBehaviorList[i].SetWallState(BuildingObstacleDirection.Top, false);
                    wallBehaviorList[i].SetWallState(BuildingObstacleDirection.Right, true);
                }
                wallBehaviorList[wallBehaviorList.Count - 1].SetWallState(BuildingObstacleDirection.Right, false);
            }
            else
            {//degree = 90
                wallBehaviorList.Sort((a, b) => a.FirstZoneIndex.Row - b.FirstZoneIndex.Row);
                for (int i = 0; i < wallBehaviorList.Count; i++)
                {
                    wallBehaviorList[i].SetWallState(BuildingObstacleDirection.Top, true);
                    wallBehaviorList[i].SetWallState(BuildingObstacleDirection.Right, false);
                }
                wallBehaviorList[wallBehaviorList.Count - 1].SetWallState(BuildingObstacleDirection.Top, false);
            }
        }
    }
    public void SetWallState(BuildingObstacleDirection wallDirection, bool active)
    {
        //print("SetWallState");
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                //m_Tk2dSpriteWall[0].color = active ? Color.white : Color.clear;
                m_Tk2dSpriteWall[0].gameObject.SetActive(active);
                break;
            case BuildingObstacleDirection.Right:
                //m_Tk2dSpriteWall[1].color = active ? Color.white : Color.clear;
                m_Tk2dSpriteWall[1].gameObject.SetActive(active);
                break;
        }
    }

    void OnMoveAllWall()
    {
        //print("OnMoveAllWall");
        //m_EnableBuildAll = true;
        for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
        { 
            WallBehavior wallBehavior = SceneManager.Instance.SelectedAllWallList[i];
            if (wallBehavior.FirstZoneIndex.Equals(wallBehavior.BuildingLogicData.BuildingPosition))
                if (wallBehavior.IsBuild)
                {
                    wallBehavior.UnBuild();
                    this.SetSelectWallState();
                } 
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(base.MouseOrTouchDictionary.Values);
            TilePosition touchDownZone = PositionConvertor.GetBuildingTileIndexFromScreenPosition(mouseOrTouchList[0].pos);
            TilePosition offsetZone = touchDownZone - base.TouchDownZone;

            if (!((wallBehavior.FirstZoneIndexPrevious + offsetZone).Equals(wallBehavior.FirstZoneIndex)))
                AudioController.Play("BuildingMoving"); 
            wallBehavior.FirstZoneIndex = wallBehavior.FirstZoneIndexPrevious + offsetZone;
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(wallBehavior.FirstZoneIndex);
            position.z = wallBehavior.BUILDING_PICK_AXIS_Z;
            wallBehavior.transform.position = position;
            wallBehavior.CheckTile();
        }
    }
    bool OnClickAllWall()
    {
        if (SceneManager.Instance.SelectedAllWallList.Count > 0)
        {
            if (SceneManager.Instance.GetAllBuildingEnableState())
            {
                for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
                { 
                    //SceneManager.Instance.SelectedAllWallList[i].BuildingBehavior.SetCellVisible(false);
                    SceneManager.Instance.SelectedAllWallList[i].SetArrowState(false);
                    SceneManager.Instance.SelectedAllWallList[i].IsClick = false;
                    SceneManager.Instance.SelectedAllWallList[i].OnActiveBuildingFX(false);
                    SceneManager.Instance.SelectedAllWallList[i].Build();
                    SceneManager.Instance.SelectedAllWallList[i].AjustBoxColliderSize(false);
                }
                this.ResetBuildingArrow();
                SceneManager.Instance.ClearAllWallList();
                return true;
            }
            return false;
        }
        return true;
    }
    void OnPressAllWall(bool isPressed)
    {
        if (isPressed)
        {
            for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
            {
                SceneManager.Instance.SelectedAllWallList[i].FirstZoneIndexPrevious = SceneManager.Instance.SelectedAllWallList[i].FirstZoneIndex;
            }
        }
    }
    void OnUpradeAllWall()
    {
        if (SceneManager.Instance.SelectedAllWallList.Count > 1)
        {
            for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++)
            {
                if (!SceneManager.Instance.SelectedAllWallList[i].IsBuild || !SceneManager.Instance.SelectedAllWallList[i].EnableCreate)
                    return;
            }
            this.ResetBuildingArrow();
            UIManager.Instance.HidePopuBtnByCurrentSelect(false);
            List<WallBehavior> wallBehaviorList = SceneManager.Instance.SelectedAllWallList;
            //wallBehaviorList.Sort(delegate(WallBehavior a, WallBehavior b) { return a.BuildingLogicData.Level - b.BuildingLogicData.Level; });
            wallBehaviorList.Sort((WallBehavior a, WallBehavior b) => a.BuildingLogicData.Level - b.BuildingLogicData.Level);
            int upgradeCount = 0;
            int upgradeStep = 0;
            int wallLowLevel = wallBehaviorList[0].BuildingLogicData.Level;
            for (int i = 0; i < wallBehaviorList.Count; i++)
            {
                if (wallBehaviorList[i].BuildingLogicData.Level == wallLowLevel &&
                    wallBehaviorList[i].BuildingLogicData.Level <= LogicController.Instance.CurrentCityHallLevel - wallBehaviorList[i].BuildingLogicData.UpgradeStep &&
                    LogicController.Instance.IdleBuilderNumber > 0 &&
                    SystemFunction.UpgradeCostBalance(wallBehaviorList[i].BuildingLogicData).Count == 0)
                {
                    SceneManager.Instance.CreateUpgradeFX(wallBehaviorList[i]);
                    LogicController.Instance.BuyUpgradeWall(wallBehaviorList[i].BuildingLogicData.BuildingIdentity.buildingNO);
                    //SceneManager.Instance.CreateFX(wallBehaviorList[i].BuildingBehavior.BuildingAnchor, FxType.Upgrade1x1, false);
                    wallBehaviorList[i].BuildingCommon.ConstructBuilding();
                    upgradeStep = wallBehaviorList[i].BuildingLogicData.UpgradeStep;
                    upgradeCount++;
                }
                else
                {
                    wallBehaviorList[i].SetCellVisible(false);
                    wallBehaviorList[i].SetArrowState(false);
                    wallBehaviorList[i].IsClick = false;
                    wallBehaviorList[i].OnActiveBuildingFX(false);
                }
            }
            if (upgradeCount > 0)
                UIErrorMessage.Instance.ErrorMessage(string.Format(StringConstants.ERROR_MESSAGE[24], upgradeCount.ToString(), wallLowLevel.ToString(), (wallLowLevel + upgradeStep).ToString()), Color.white);
            //UIErrorMessage.Instance.ErrorMessage(24, upgradeCount.ToString(), wallLowLevel.ToString(), (wallLowLevel + upgradeStep).ToString());
            else
                UIErrorMessage.Instance.ErrorMessage(23);
            
            SceneManager.Instance.ClearAllWallList();
        }
    }
    // static public int SortByLevel(WallBehavior a, WallBehavior b) { return  a.BuildingLogicData.Level - b.BuildingLogicData.Level; }
    void GetComponent()
    {
        m_Tk2dSpriteWall = new tk2dSprite[2];
        m_Tk2dSpriteWall[0] = this.transform.FindChild(ClientSystemConstants.WALL_TOP).GetComponent<tk2dSprite>();
        m_Tk2dSpriteWall[1] = this.transform.FindChild(ClientSystemConstants.WALL_RIGHT).GetComponent<tk2dSprite>();
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxColliderSize = m_BoxCollider.size;
        //base.GetComponent();
    }
    public void AjustBoxColliderSize(bool scale = true)
    {
        m_BoxCollider.size = scale ? m_BoxColliderSize * 1.5f : m_BoxColliderSize;
    }

    public void SetSelfWallState()
    {
        if (!this.FindNeighourWall(BuildingObstacleDirection.Top, 1).HasValue)
        {
           // m_Tk2dSpriteWall[0].color = Color.clear;
            m_Tk2dSpriteWall[0].gameObject.SetActive(false);
            //this.SetWallActorObstacle(BuildingObstacleDirection.Top, 1, false);
        }
        else
        {
            bool value = this.FindNeighourWall(BuildingObstacleDirection.Top, 1).Value;
            //m_Tk2dSpriteWall[0].color = value ? Color.white : Color.clear;
            m_Tk2dSpriteWall[0].gameObject.SetActive(value);
            if (value)
                this.SetWallActorObstacle(BuildingObstacleDirection.Top, 1, true);
        }

        if (!this.FindNeighourWall(BuildingObstacleDirection.Right, 1).HasValue)
        {
            //m_Tk2dSpriteWall[1].color = Color.clear;
            m_Tk2dSpriteWall[1].gameObject.SetActive(false);
            //this.SetWallActorObstacle(BuildingObstacleDirection.Right, 1, false);
        }
        else
        {
            bool value = this.FindNeighourWall(BuildingObstacleDirection.Right, 1).Value;
            //m_Tk2dSpriteWall[1].color = value ? Color.white : Color.clear;
            m_Tk2dSpriteWall[1].gameObject.SetActive(value);
            if(value)
                this.SetWallActorObstacle(BuildingObstacleDirection.Right, 1, true);
        }
        SetOtherWallState(true);
    }
    public void SetOtherWallState(bool isBuild)
    {
        //if (isBuild)
        {
            if (this.FindNeighourWall(BuildingObstacleDirection.Left, 1).HasValue)
            {
                if (this.FindNeighourWall(BuildingObstacleDirection.Left, 1).Value)
                {
                    this.GetWallBehavior(BuildingObstacleDirection.Left, 1).SetWallSprite(BuildingObstacleDirection.Right, isBuild);
                    this.GetWallBehavior(BuildingObstacleDirection.Left, 1).SetWallActorObstacle(BuildingObstacleDirection.Right, 1, isBuild); 
                }
            }

            if (this.FindNeighourWall(BuildingObstacleDirection.Bottom, 1).HasValue)
            {
                if (this.FindNeighourWall(BuildingObstacleDirection.Bottom, 1).Value)
                {
                    this.GetWallBehavior(BuildingObstacleDirection.Bottom, 1).SetWallSprite(BuildingObstacleDirection.Top, isBuild);
                    this.GetWallBehavior(BuildingObstacleDirection.Bottom, 1).SetWallActorObstacle(BuildingObstacleDirection.Top, 1, isBuild);
      
                }
            }
        }
        if (!isBuild)
        {
            //right and top actor obstacle
            if (this.FindNeighourWall(BuildingObstacleDirection.Right, 1).HasValue)
            {
                if (this.FindNeighourWall(BuildingObstacleDirection.Right, 1).Value)
                {
                    this.GetWallBehavior(BuildingObstacleDirection.Right, 1).SetWallActorObstacle(BuildingObstacleDirection.Left, 1, isBuild);
                
                }
            }
            else
                this.SetWallActorObstacle(BuildingObstacleDirection.Right, 1, isBuild);

            if (this.FindNeighourWall(BuildingObstacleDirection.Top, 1).HasValue)
            {
                if (this.FindNeighourWall(BuildingObstacleDirection.Top, 1).Value)
                {
                    this.GetWallBehavior(BuildingObstacleDirection.Top, 1).SetWallActorObstacle(BuildingObstacleDirection.Bottom, 1, isBuild);

                }
            }
            else
                this.SetWallActorObstacle(BuildingObstacleDirection.Top, 1, isBuild);

            if (!this.FindNeighourWall(BuildingObstacleDirection.Left, 1).HasValue)
                this.SetWallActorObstacle(BuildingObstacleDirection.Left, 1, isBuild);

            if (!this.FindNeighourWall(BuildingObstacleDirection.Bottom, 1).HasValue)
                this.SetWallActorObstacle(BuildingObstacleDirection.Bottom, 1, isBuild); 
        }
    }
    public void SetWallSprite(BuildingObstacleDirection direction,bool isActive)
    {
        //print("SetWallSprite");
        switch (direction)
        {
            case BuildingObstacleDirection.Top:
                //m_Tk2dSpriteWall[0].color = isActive ? Color.white : Color.clear;
                m_Tk2dSpriteWall[0].gameObject.SetActive(isActive);
                break;
            case BuildingObstacleDirection.Right:
                //m_Tk2dSpriteWall[1].color = isActive ? Color.white : Color.clear;
                m_Tk2dSpriteWall[1].gameObject.SetActive(isActive);
                break;
        }
    }

    WallBehavior  GetWallBehavior(BuildingObstacleDirection wallDirection,int index /*,TilePosition tilePosition*/)
    {
       
        //return SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(tilePosition.Row, tilePosition.Column).GetComponent<WallBehavior>();
        TilePosition tilePosition = null;
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row + index);
                break;
            case BuildingObstacleDirection.Bottom:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row - index); 
                break;
            case BuildingObstacleDirection.Left:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column - index, this.BuildingLogicData.BuildingPosition.Row); 
                break;
            case BuildingObstacleDirection.Right:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column + index, this.BuildingLogicData.BuildingPosition.Row); 
                break;
        }
        return SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(tilePosition.Row, tilePosition.Column).GetComponent<WallBehavior>();
    }

    public Nullable<bool> FindNeighourWall(BuildingObstacleDirection wallDirection,int index)
    {
        TilePosition tilePosition = null;
        BuildingLogicData buildingLogicData = null;
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row + index);
                
                break;
            case BuildingObstacleDirection.Bottom:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row - index);
                
                break;
            case BuildingObstacleDirection.Left:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column - index, this.BuildingLogicData.BuildingPosition.Row);
                
                break;
            case BuildingObstacleDirection.Right:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column + index, this.BuildingLogicData.BuildingPosition.Row); 
                break;
        }
        IObstacleInfo obstacleInfo = SceneManager.Instance.GetBuildingInfoFormBuildingObstacleMap(tilePosition.Row, tilePosition.Column);
        if (obstacleInfo == null)
            return null;
        else
        {
            buildingLogicData = obstacleInfo as BuildingLogicData;
            if (buildingLogicData != null)
            {
                if (buildingLogicData.BuildingIdentity.buildingType == BuildingType.Wall)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
    public Nullable<bool> FindNeighourWallNotNull(BuildingObstacleDirection wallDirection, int index)
    {
        TilePosition tilePosition = null;
        BuildingLogicData buildingLogicData = null;
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row + index);

                break;
            case BuildingObstacleDirection.Bottom:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column, this.BuildingLogicData.BuildingPosition.Row - index);

                break;
            case BuildingObstacleDirection.Left:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column - index, this.BuildingLogicData.BuildingPosition.Row);

                break;
            case BuildingObstacleDirection.Right:
                tilePosition = new TilePosition(this.BuildingLogicData.BuildingPosition.Column + index, this.BuildingLogicData.BuildingPosition.Row);
                break;
        }
 
        IObstacleInfo obstacleInfo = SceneManager.Instance.GetBuildingInfoFormBuildingObstacleMap(tilePosition.Row, tilePosition.Column);
        if (obstacleInfo == null)
            return null;
        else
        {
            buildingLogicData = obstacleInfo as BuildingLogicData;
            if (buildingLogicData != null)
            {
                if (buildingLogicData.BuildingIdentity.buildingType == BuildingType.Wall)
                    return true;
                else
                    return null;
            }
            return null;
        }
    }

    void SetWallActorObstacle(BuildingObstacleDirection wallDirection, int index,bool isActive)
    {
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                SceneManager.Instance.ActorGameObjectData[this.BuildingLogicData.ActorPosition.Row + this.BuildingLogicData.ActorObstacleList[0].Row + index, this.BuildingLogicData.ActorPosition.Column + this.BuildingLogicData.ActorObstacleList[0].Column] = isActive ? this.gameObject : null;
                break;
            case BuildingObstacleDirection.Bottom:
                SceneManager.Instance.ActorGameObjectData[this.BuildingLogicData.ActorPosition.Row + this.BuildingLogicData.ActorObstacleList[0].Row - index, this.BuildingLogicData.ActorPosition.Column + this.BuildingLogicData.ActorObstacleList[0].Column] = isActive ? this.gameObject : null;
                break;
            case BuildingObstacleDirection.Left:
                SceneManager.Instance.ActorGameObjectData[this.BuildingLogicData.ActorPosition.Row + this.BuildingLogicData.ActorObstacleList[0].Row, this.BuildingLogicData.ActorPosition.Column + this.BuildingLogicData.ActorObstacleList[0].Column - index] = isActive ? this.gameObject : null;
                break;
            case BuildingObstacleDirection.Right:
                SceneManager.Instance.ActorGameObjectData[this.BuildingLogicData.ActorPosition.Row + this.BuildingLogicData.ActorObstacleList[0].Row, this.BuildingLogicData.ActorPosition.Column + this.BuildingLogicData.ActorObstacleList[0].Column + index] = isActive ? this.gameObject : null;
                break;
        }
    }

    public void OnSelectWallRow()
    {
        print("OnSelectWallRow");
        SceneManager.Instance.ClearAllWallList();
        int rightIndex = 1;
        while (FindNeighourWallNotNull(BuildingObstacleDirection.Right, rightIndex).HasValue)
        {
            //if (FindNeighourWall(BuildingObstacleDirection.Right, rightIndex).Value)
            {
                WallBehavior wallBehavior = this.GetWallBehavior(BuildingObstacleDirection.Right, rightIndex);
                wallBehavior.SetTileState(true);
                SceneManager.Instance.WallListRight.Add(wallBehavior);
                wallBehavior.AjustBoxColliderSize();
            }
            rightIndex++;
        }
        int leftIndex = 1;
        while (FindNeighourWallNotNull(BuildingObstacleDirection.Left, leftIndex).HasValue)
        {
            //if (FindNeighourWall(BuildingObstacleDirection.Left, leftIndex).Value)
            {
                WallBehavior wallBehavior = this.GetWallBehavior(BuildingObstacleDirection.Left, leftIndex);
                wallBehavior.SetTileState(true);
                SceneManager.Instance.WallListLeft.Add(wallBehavior);
                wallBehavior.AjustBoxColliderSize();
            }
            leftIndex++;
        }
        if (SceneManager.Instance.WallListRight.Count > 0 || SceneManager.Instance.WallListLeft.Count > 0)
        {
            base.SetTileState(true);
            SceneManager.Instance.SelectedAllWallList.AddRange(SceneManager.Instance.WallListLeft);
            SceneManager.Instance.SelectedAllWallList.Add(this);
            SceneManager.Instance.SelectedAllWallList.AddRange(SceneManager.Instance.WallListRight);
            SceneManager.Instance.WallSelectedMode = WallSelectedMode.Row;
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            UIManager.Instance.ShowPopupBtnByCurrentSelect();
            this.SetArrowPosition(false);
            //this.m_EnableBuildAll = true;
            return;
        }

        int topIndex = 1;
        while (FindNeighourWallNotNull(BuildingObstacleDirection.Top, topIndex).HasValue)
        {
            //if (FindNeighourWall(BuildingObstacleDirection.Top, topIndex).Value)
            {
                WallBehavior wallBehavior = this.GetWallBehavior(BuildingObstacleDirection.Top, topIndex);
                wallBehavior.SetTileState(true);
                SceneManager.Instance.WallListTop.Add(wallBehavior);
                wallBehavior.AjustBoxColliderSize();
            }
            topIndex++;
        }
        int bottomIndex = 1;
        while (FindNeighourWallNotNull(BuildingObstacleDirection.Bottom, bottomIndex).HasValue)
        {
            //if (FindNeighourWall(BuildingObstacleDirection.Bottom, bottomIndex).Value)
            {
                WallBehavior wallBehavior = this.GetWallBehavior(BuildingObstacleDirection.Bottom, bottomIndex);
                wallBehavior.SetTileState(true);
                SceneManager.Instance.WallListBottom.Add(wallBehavior);
                wallBehavior.AjustBoxColliderSize();
            }
            bottomIndex++;
        }
        if (SceneManager.Instance.WallListTop.Count > 0 || SceneManager.Instance.WallListBottom.Count > 0)
        {
            base.SetTileState(true);
            SceneManager.Instance.SelectedAllWallList.AddRange(SceneManager.Instance.WallListTop);
            SceneManager.Instance.SelectedAllWallList.Add(this);
            SceneManager.Instance.SelectedAllWallList.AddRange(SceneManager.Instance.WallListBottom);
            SceneManager.Instance.WallSelectedMode = WallSelectedMode.Column;
            
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            UIManager.Instance.ShowPopupBtnByCurrentSelect();
            this.SetArrowPosition(false);
           //this.m_EnableBuildAll = true;
        }
    }



    WallBehavior FindNeighourWallBehavior(BuildingObstacleDirection wallDirection, TilePosition refTilePosition, int index)
    {
        TilePosition tilePositionNext = null;
        GameObject go = null;
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                tilePositionNext = new TilePosition(refTilePosition.Column, refTilePosition.Row + index);
                break;
            case BuildingObstacleDirection.Bottom:
                tilePositionNext = new TilePosition(refTilePosition.Column, refTilePosition.Row - index);
                break;
            case BuildingObstacleDirection.Left:
                tilePositionNext = new TilePosition(refTilePosition.Column - index, refTilePosition.Row);
                break;
            case BuildingObstacleDirection.Right:
                tilePositionNext = new TilePosition(refTilePosition.Column + index, refTilePosition.Row);
                break;
        }
        if (tilePositionNext.Row < 0 || tilePositionNext.Column < 0 || tilePositionNext.Row >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height || tilePositionNext.Column >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
        {
            return null;
        }
        go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(tilePositionNext.Row, tilePositionNext.Column);
        if (go == null)
            return null;
        else
        {
            WallBehavior bhv = go.GetComponent<WallBehavior>();
            if (bhv.BuildingType == BuildingType.Wall)
            {
                return bhv;//(WallBehavior)bhv.BuildingCommon;
            }
        }
        return null;
    }

 

    void InitialWall()
    {
        //m_Tk2dSpriteWall[0].color = Color.clear;
        //m_Tk2dSpriteWall[1].color = Color.clear;
        m_Tk2dSpriteWall[0].gameObject.SetActive(false);
        m_Tk2dSpriteWall[1].gameObject.SetActive(false);
    }
    public void SetArrowPosition(bool initialPosition)
    {
        if (initialPosition)
        {
            for (int i = 0; i < base.m_ArrowList.Count; i++)
            {
                Vector3 localPosition = Vector3.zero;
                localPosition.x = base.m_ArrowOffset[i].x;
                localPosition.y = base.m_ArrowOffset[i].y;
                localPosition.z = 0;
                base.m_ArrowList[i].transform.localPosition = localPosition;
            }
        }
        else
        {
            int count = SceneManager.Instance.SelectedAllWallList.Count;
            if (count > 1)
            {
                int width = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width * SceneManager.Instance.SelectedAllWallList.Count;
                int height = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
                float offsetwidthTop = 20;
                float offsetwidthRight = 40;
                float offsetHeight = 18;
                int degree = 0;
                List<WallBehavior> selectedAllWallList = SceneManager.Instance.SelectedAllWallList;
                if ((selectedAllWallList[0].FirstZoneIndex.Row - selectedAllWallList[1].FirstZoneIndex.Row) == 0)
                {
                    selectedAllWallList.Sort((a, b) => a.FirstZoneIndex.Column - b.FirstZoneIndex.Column);
                    degree = 0;
                }
                else
                {
                    selectedAllWallList.Sort((a, b) => a.FirstZoneIndex.Row - b.FirstZoneIndex.Row);
                    degree = 90;
                }
                Vector2 minWorldPosition = PositionConvertor.GetWorldPositionByBuildingTileIndex(selectedAllWallList[0].FirstZoneIndex);
                Vector2 maxWorldPosition = PositionConvertor.GetWorldPositionByBuildingTileIndex(selectedAllWallList[count - 1].FirstZoneIndex);
                Vector2 halfWorldPosition = (maxWorldPosition - minWorldPosition) * 0.5f + minWorldPosition;
                Vector2 worldPosition = Vector2.zero;
                switch (degree)
                {
                    case 0:
                        worldPosition.x = halfWorldPosition.x + (width * 0.5f + offsetwidthRight);
                        worldPosition.y = halfWorldPosition.y + (0 + offsetHeight);
                        base.m_ArrowList[0].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (0 + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (height + offsetHeight);
                        base.m_ArrowList[1].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (-width * 0.5f);
                        worldPosition.y = halfWorldPosition.y + (0 + offsetHeight);
                        base.m_ArrowList[2].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (0 + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (-height + offsetHeight);
                        base.m_ArrowList[3].transform.position = worldPosition;
                        break;
                    case 90:
                        worldPosition.x = halfWorldPosition.x + (height + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (0 + offsetHeight);
                        base.m_ArrowList[0].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (0 + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (width * 0.5f + offsetwidthRight);
                        base.m_ArrowList[1].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (-height + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (0 + offsetHeight);
                        base.m_ArrowList[2].transform.position = worldPosition;

                        worldPosition.x = halfWorldPosition.x + (0 + offsetwidthTop);
                        worldPosition.y = halfWorldPosition.y + (-width * 0.5f);
                        base.m_ArrowList[3].transform.position = worldPosition;
                        break;
                }
            }
        }
    }
}
public class WallParamNextNode
{
    public BuildingObstacleDirection WallDirection;
    public TilePosition TilePosition;
    public int Weight;
}

public enum WallSelectedMode
{
    Row,
    Column
}
