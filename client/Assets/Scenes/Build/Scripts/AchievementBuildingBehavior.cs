using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using ConfigUtilities.Structs;
using System.Linq;
public class AchievementBuildingBehavior : BuildingBehavior
{
    public AchievementBuildingLogicData AchievementBuildingLogicData { get; set; }
    public AchievementBuildingConfigData AchievementBuildingConfigData { get; set; }
    public AchievementBuildingType AchievementBuildingType { get; set; }
	// Use this for initialization
	void Start () {
        this.CreateAchievementBuildingObstacle();
        this.CreateAchievementBuildingActorObstacle();
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
        if (!this.enabled || !UIManager.Instance.SceneFocus || !this.Created || !base.EnableCreate || base.MouseOrTouchDictionary.Count > 1)
        {
            base.FirstZoneIndexPrevious = base.FirstZoneIndex;
            return;
        }

        if (base.FirstZoneIndex.Equals(base.FirstZoneIndexPrevious) && (base.PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude < 16 && base.m_AllowClick)
        {
            if (!CheckBuildingCreateStack())
                return;
            base.IsClick = !base.IsClick;
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
        if (!base.Created)
            return;

        base.IsClick = false;
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
        if (!this.enabled)
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
    #endregion
    new void Build()
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = false;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = this.gameObject;
        }
        TilePosition actorFirstZoneIndex = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(base.FirstZoneIndex);
        for (int i = 0; i < m_RealActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = false;
            SceneManager.Instance.ActorGameObjectData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = this.gameObject;
        }
        if (!base.FirstZoneIndex.Equals(this.AchievementBuildingLogicData.BuildingPosition))
        {
            LogicController.Instance.MoveAchievementBuiding(this.AchievementBuildingLogicData.BuildingNo, base.FirstZoneIndex);
            this.OnCreateBorder(BorderType.BuildingOutlineBorder);
        }
        else
        {
            this.OnDestroyBorder();
        }
        this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
        this.m_IsBuild = true;
        base.EnableCreate = true;

        this.CreateDropDownFX();
        this.SetCellVisible(false);
    }
    void UnBuild(bool showBorder = true)
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = true;
            SceneManager.Instance.BuildingGameObjectData[base.FirstZoneIndex.Row + base.m_RealBuildObstalceList[i].Row, base.FirstZoneIndex.Column + base.m_RealBuildObstalceList[i].Column] = null;
        }
        TilePosition actorFirstZoneIndex = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(base.FirstZoneIndex);
        for (int i = 0; i < base.m_RealActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = true;
            SceneManager.Instance.ActorGameObjectData[actorFirstZoneIndex.Row + base.m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + base.m_RealActorObstacleList[i].Column] = null;
        }
        this.m_IsBuild = false;
        if (showBorder)
            this.OnCreateBorder();

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
                if (this.AchievementBuildingLogicData.BuildingPosition != null)
                {
                    base.FirstZoneIndex = this.AchievementBuildingLogicData.BuildingPosition;
                    this.Build();
                }
            }
        }
    }
    void CreateAchievementBuildingObstacle()
    {
        if (!this.Created)
            base.m_RealBuildObstalceList = PositionConvertor.TilePointListToTilePositionList(this.AchievementBuildingConfigData.BuildingObstacleList);
        else
            base.m_RealBuildObstalceList = this.AchievementBuildingLogicData.BuildingObstacleList;
    }
    void CreateAchievementBuildingActorObstacle()
    {
        if (!this.Created)
        {
            base.m_RealActorObstacleList = PositionConvertor.TilePointListToTilePositionList(this.AchievementBuildingConfigData.ActorObstacleList);
            base.OnCreateBorder();
        }
        else
            base.m_RealActorObstacleList = this.AchievementBuildingLogicData.ActorObstacleList;
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
            base.FirstZoneIndex = this.AchievementBuildingLogicData.BuildingPosition;
            this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(base.FirstZoneIndex);
            this.Build();
            base.SetArrowState(false);
        }
    }
    public override void ShowBuildingTitle(bool isShow)
    {
        if (isShow)
        {
            base.m_TextMeshBuildingTitle.text = "   " + this.AchievementBuildingLogicData.Name;
            Vector3 localPosition = Vector3.zero;
            localPosition.x = this.ProgressBarOffset.x + m_BuildingTitleOffset.x;
            localPosition.y = this.ProgressBarOffset.y + m_BuildingTitleOffset.y;
            localPosition.z = -100;
            this.m_TextMeshBuildingTitle.transform.localPosition = localPosition;
        }
        this.m_TextMeshBuildingTitle.color = isShow ? Color.white : Color.clear;
        this.m_TextMeshBuildingTitle.Commit();
    }
    //Button message
    void OnConstructBuilding()
    {
        if (base.EnableCreate)
        {
            this.AchievementBuildingLogicData = LogicController.Instance.BuildAchievementBuilding(this.AchievementBuildingType, base.FirstZoneIndex);
            base.Created = true;
            this.Build();
            SceneManager.Instance.BuildingBehaviorTemporary = null;
            base.IsClick = false;
            base.SetArrowState(false);
            base.DestroyButton();
        }
    }
    //Popup Button message
    void ShowWindowBuildingInfomation()
    {
        UIManager.Instance.UIWindowBuildingInfomation.AchievementBuildingLogicData = this.AchievementBuildingLogicData;
        UIManager.Instance.UIWindowBuildingInfomation.ShowWindowAchievementBuilding();
    }
    //Popup Button message
    void OnRemoveBuilidng()
    {
        UIManager.Instance.UIWindowConfirmPrompt.UnRegistDelegate();
        UIManager.Instance.UIWindowConfirmPrompt.Click += () =>
        {
            LogicController.Instance.DestroyAchievementBuilding(this.AchievementBuildingLogicData.BuildingNo);
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            if (SceneManager.Instance.PickableObjectCurrentSelect != null)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = null;
            this.UnBuild(false);
            Destroy(this.gameObject);
            this.enabled = false;
        };
        UIManager.Instance.UIWindowConfirmPrompt.ShowWindow(StringConstants.PROMT_REMOVE_OBJCET_TITLE, string.Format(StringConstants.PROMT_REMOVE_OBJCET_TITLE_CONTEXT, this.AchievementBuildingLogicData.Name));
    }
    //Popup Button message
    void OnRepairBuilidng()
    {
        if (this.AchievementBuildingLogicData.Life >= this.AchievementBuildingLogicData.MaxLife)
            return;
        AchievementBuildingConfigData achievementBuildingConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(this.AchievementBuildingLogicData.AchievementBuildingType);
        PropsType propsType = achievementBuildingConfigData.NeedPropsType;
        int currentPropsCount = LogicController.Instance.AllProps.Count(a => a.PropsType == propsType && a.RemainingCD <= 0);
        if (currentPropsCount > 0)
            LogicController.Instance.RepairAchievementBuilding(this.AchievementBuildingLogicData.BuildingNo);
        else
            UIErrorMessage.Instance.ErrorMessage(39, this.AchievementBuildingLogicData.Name, achievementBuildingConfigData.Name);
    }
    void CreateDropDownFX()
    {
        if (base.m_DropDownFX < 0)
        {
            base.m_DropDownFX++;
            return;
        }
        SceneManager.Instance.CreateSmokeFX(this); 
    }
}
