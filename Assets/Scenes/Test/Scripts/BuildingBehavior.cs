using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using ConfigUtilities.Structs; 
using System;
using CommonUtilities;
//using ConfigUtilities;
public class BuildingBehavior : MonoBehaviour,IPickableObject {
    [SerializeField] protected Vector2[] m_ArrowOffset;
    [SerializeField] Vector2[] m_ButtonOffset;
    //[SerializeField] GameObject[] m_Button;
    tk2dSprite m_ButtonOkUISprite;
    public Vector2 ProgressBarOffset;
    public Vector2 ProgressBarSize;
    public int BUILDING_PICK_AXIS_Z = -4000;
    //TilePosition m_BuildCenter;
    //public TilePosition BuildCenter
    //{ get { return m_BuildCenter; } set { m_BuildCenter = value; } }
    //CameraManager CameraManager.Instance;
    protected bool m_IsBuild = false;
    public bool IsBuild { get { return m_IsBuild; } }
    TilePosition m_FirstZoneIndex;
    public TilePosition FirstZoneIndex { get { return m_FirstZoneIndex; } set { m_FirstZoneIndex = value; } }
    TilePosition m_FirstZoneIndexPrevious;
    public TilePosition FirstZoneIndexPrevious { get { return m_FirstZoneIndexPrevious; } set { m_FirstZoneIndexPrevious = value; } }
    TilePosition m_TouchDownZone;
    public TilePosition TouchDownZone { get { return m_TouchDownZone; } set { m_TouchDownZone = value; } }
    //tk2dSprite m_Tk2dSprite;
    protected List<TilePosition> m_RealBuildObstalceList;
    protected List<TilePosition> m_RealActorObstacleList;
    Dictionary<TilePosition, tk2dSlicedSprite> m_CellDict = new Dictionary<TilePosition, tk2dSlicedSprite>();
    //BuildingBehavior m_BuildingBehavior;
    //bool m_IsPress = false;
    bool m_IsClick = false;
    public bool IsClick { get { return m_IsClick; } set { m_IsClick = value;} }
    Vector3 m_PressPosition;
    public Vector3 PressPosition { get { return m_PressPosition; } set { m_PressPosition = value; } }
    AdjacencyList<TilePosition> m_AdjacencyList;
    bool m_EnableCreate = false;
    public bool EnableCreate { get { return m_EnableCreate; } set { m_EnableCreate = value; } }
    protected List<tk2dSprite> m_ArrowList = new List<tk2dSprite>();
    //public BuildingData BuildingData { get; set; }
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingConfigData BuildingConfigData { get; set; }
    public BuildingType BuildingType { get; set; }
    public bool Created { get; set; }
    public string buildingFacility;
    const string BUTTON_OK = "BuildingScene/Common/OkButton";
    const string BUTTON_CANCEL = "BuildingScene/Common/CancelButton";
    const string CELL = "BuildingScene/Common/Cell";
    const string ARROW = "BuildingScene/Common/Arrow";
    const string TEXT_BUILDING_TITLE = "BuildingScene/Common/TextBuildingTitle";
    const string TEXT_BUILDING_WARRING_TITLE = "TextWarring";
    const string TEXT_BUILDING_WARRING_BACKGROUND = "BuildingScene/Common/Warring";
    protected tk2dTextMesh m_TextMeshBuildingTitle;
    tk2dTextMesh m_TextMeshBuildingWarringTitle;
    tk2dSprite m_BuildingWarringBg;
    GameObject m_ButtonOk;
    public GameObject ButtonOk { get { return m_ButtonOk; } }
    GameObject m_ButtonCancel;
    public GameObject ButtonCancel { get { return m_ButtonCancel; } }
    Dictionary<int, UICamera.MouseOrTouch> m_MouseOrTouchDictionary = new Dictionary<int, UICamera.MouseOrTouch>();
    public Dictionary<int, UICamera.MouseOrTouch> MouseOrTouchDictionary { get { return m_MouseOrTouchDictionary; } set { m_MouseOrTouchDictionary = value; } }
    //TweenColortk2dSprite m_TweenColorBuildingBk;
    BuildingCommon m_BuildingCommon;
    public BuildingCommon BuildingCommon { get { return m_BuildingCommon; } }
    public Transform BuildingAnchor { get; set; }
    //Transform m_AttackRange;
    protected int m_DropDownFX = -1;
	protected bool m_AllowClick = true;
    BoxCollider m_ButtonOkBoxCollider;
    [SerializeField] protected Vector2 m_BuildingTitleOffset = new Vector2(50, 0);
    [SerializeField] Vector2 m_WarringOffset = new Vector2(50, 0);
    TweenColortk2dSprite m_TweenColorBuildingBk;
    protected AttackRangeFX m_AttackRangeFX;
	// Use this for initialization
    protected virtual void Awake()
    {
        this.BuildingAnchor = this.transform.FindChild("BuildingBackgroundAnchor");
        Transform trans = this.transform.FindChild("BuildingBackgroundAnchor/AttackRangeFX");
        m_AttackRangeFX = trans != null ? trans.GetComponent<AttackRangeFX>() : null;
        this.m_TweenColorBuildingBk = this.transform.FindChild(ClientSystemConstants.BACKGROUND_NAME).GetComponent<TweenColortk2dSprite>();
    }
    void Start()
    {
        this.CreateBuildingObstacle();
        this.CreateActorObstacle();
        this.CreateCell();
        this.CreateArrow();
        this.CreateComponent();
        this.InitBuildPosition();
        this.CreateButton();
        this.SetButtonOkState();
	}
    void Update()
    {
        this.OnShowBuildingWarringTitle();
        this.OnReadyForUpgrade();
    }
    // Update is called once per frame
    #region Ngui interactive
    public virtual void OnClick()
    {
        print("BuildingBehavior OnClick");
        if (!this.enabled || !UIManager.Instance.SceneFocus || !this.Created || !CheckBuildingCreateStack() || !this.m_EnableCreate || this.m_MouseOrTouchDictionary.Count > 1)
        {
            //if (this.BuildingType != BuildingType.Wall)
            this.m_FirstZoneIndexPrevious = this.m_FirstZoneIndex;
            return;
        }
        if (this.ReadyForUpgrade() || this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal && this.OnCheckCollectValidity())
            return;
       
        if (this.m_FirstZoneIndex.Equals(this.m_FirstZoneIndexPrevious) && (this.m_PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude < 16 && this.m_AllowClick)
        {
            //if (!CheckBuildingCreateStack())
            //    return;
            this.IsClick = !this.m_IsClick;
            this.SetAttackRange(this.m_IsClick); 
            SetArrowState(this.m_IsClick);
            this.OnActiveBuildingFX(this.m_IsClick);
        }
        else
        {
            if (!this.m_IsClick)
                CameraManager.Instance.OnClick();
            this.m_FirstZoneIndexPrevious = this.m_FirstZoneIndex;
        }

        if (!CheckBuildingCreateStack())
            return; 
        if(this.IsClick && m_IsBuild)
            AudioController.Play("BuildingPick");
        if (this.m_EnableCreate && !m_IsBuild) 
            this.Build(); 
        //this.SetCellVisible(!this.m_IsBuild); 
        if (this.m_IsClick)
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect != null && SceneManager.Instance.PickableObjectCurrentSelect != this)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = this;
            if (this.m_IsBuild)
                UIManager.Instance.ShowPopupBtnByCurrentSelect();
        }
        else
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect == this)
                UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        }
    }
    public virtual void OnUnSelect(bool isCancel)
    { 
        if (!this.Created)
            return;

        this.IsClick = false;
        this.SetAttackRange(this.m_IsClick); 
        this.SetArrowState(false);
        this.ResetBuild();
        //this.OnDestroyBorder();
        if(isCancel)
            UIManager.Instance.HidePopuBtnByCurrentSelect(false);
        else
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        this.OnActiveBuildingFX(false);
    }
    void OnDrag(Vector2 v)
    { 
        if (!this.enabled /*|| this.BuildingType == BuildingType.Wall*/)
            return;
        if (this.m_IsClick && SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
            OnMoveBuilding();
        else
		{
            //if (SceneManager.Instance.PickableObjectCurrentSelect == null || SceneManager.Instance.PickableObjectCurrentSelect.IsBuild)
            {
                if (SceneManager.Instance.MouseOrTouchDictionaryGloble.Count < 2)
                    CameraManager.Instance.OnDragExt(this.m_MouseOrTouchDictionary);
                else
                {
                    CameraManager.Instance.OnDragExt(SceneManager.Instance.MouseOrTouchDictionaryGloble);
                    this.m_AllowClick = false;
                }
            }
		}

    }
    void OnPress(bool isPressed)
    {
		SceneManager.Instance.OnPressGloble(isPressed);
        if (isPressed)
        {
			this.m_AllowClick = true;
            if (!m_MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionary.Add(UICamera.currentTouchID, UICamera.currentTouch);
        }
        else
            if (m_MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionary.Remove(UICamera.currentTouchID);

        this.SetTouchDownZone();
        if(!this.m_IsClick)
        {
            CameraManager.Instance.OnPress(isPressed);
        }
        if (isPressed)
        {
            m_FirstZoneIndexPrevious = m_FirstZoneIndex;
            m_PressPosition = CameraManager.Instance.MainCamera.transform.position;
        }
    }
    void OnDrop(GameObject go)
    {
        print("BuildingBehavior OnDrop");
        CameraManager.Instance.OnClick();
        if (SceneManager.Instance.PickableObjectCurrentSelect != null) 
            SceneManager.Instance.PickableObjectCurrentSelect.OnClick();
    }
    #endregion
    void SetAttackRange(bool active)
    {
        if (this.m_AttackRangeFX != null)
        {
            if (active)
                m_AttackRangeFX.ShowAttackRange(this.BuildingLogicData.ApMaxScope, this.BuildingLogicData.ApMinScope);
            else
                m_AttackRangeFX.HideAttackRange(this.BuildingLogicData.ApMaxScope, this.BuildingLogicData.ApMinScope);
        }
    }
    public void SetTouchDownZone()
    {
        if (this.m_MouseOrTouchDictionary.Count > 0)
        {
            List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(this.m_MouseOrTouchDictionary.Values);
            m_TouchDownZone = PositionConvertor.GetBuildingTileIndexFromScreenPosition(mouseOrTouchList[0].pos);
        } 
    } 

    void OnMoveBuilding()
    {
        if (this.m_FirstZoneIndex.Equals(this.m_FirstZoneIndexPrevious))
            if (m_IsBuild)
            {
                this.UnBuild();
                UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            }
        if (this.m_MouseOrTouchDictionary.Count > 0)
        {
            List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(this.m_MouseOrTouchDictionary.Values);
            TilePosition touchDownZone = PositionConvertor.GetBuildingTileIndexFromScreenPosition(mouseOrTouchList[0].pos); //PositionConvertor.GetTileIndexFromScreenPosition(PositionConvertor.GetInputPosition(TouchPhase.Moved));
            TilePosition offsetZone = touchDownZone - m_TouchDownZone;
            //if (offsetZone.Row == 0 && offsetZone.Column == 0)
            //return;
            if (!((m_FirstZoneIndexPrevious + offsetZone).Equals(m_FirstZoneIndex)))
                AudioController.Play("BuildingMoving"); 
            m_FirstZoneIndex = m_FirstZoneIndexPrevious + offsetZone;
            this.LimitBuildingPosition();
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(m_FirstZoneIndex);
            position.z = BUILDING_PICK_AXIS_Z;
            this.transform.position = position;
            CheckTile();
            SetButtonOkState();
        }
    }
    protected void LimitBuildingPosition()
    {
        m_FirstZoneIndex.Row = m_FirstZoneIndex.Row < 0 ? 0 : m_FirstZoneIndex.Row;
        m_FirstZoneIndex.Row = m_FirstZoneIndex.Row >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height ? ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height - 1 : m_FirstZoneIndex.Row;
        m_FirstZoneIndex.Column = m_FirstZoneIndex.Column < 0 ? 0 : m_FirstZoneIndex.Column;
        m_FirstZoneIndex.Column = m_FirstZoneIndex.Column >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width ? ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width - 1 : m_FirstZoneIndex.Column;
      
    }
    void InitBuildPosition()
    {
        if (!this.Created)
        {
            m_FirstZoneIndex = PositionConvertor.GetBuildingTileIndexFromWorldPosition(CameraManager.Instance.MainCamera.transform.position);
            if (this.BuildingType != BuildingType.Wall || this.BuildingType == BuildingType.Wall && !SceneManager.Instance.EnableCreateWallContinuation)
            {
                if (!CheckTile())
                    SearchBuildingPosition();
            }
            else
            {
                m_FirstZoneIndex = SceneManager.Instance.FindBuildNextPoint();
                if (m_FirstZoneIndex == null)
                {
                    
                    SceneManager.Instance.EnableCreateWallContinuation = false;
                    if (!CheckTile())
                        SearchBuildingPosition();
                }
            }
            this.CheckTile();
            Vector3 position = PositionConvertor.GetWorldPositionByBuildingTileIndex(m_FirstZoneIndex);
            position.z = BUILDING_PICK_AXIS_Z;
            this.transform.position = position;
            this.SetArrowState(true);
            this.m_IsClick = true;
        }
        else
        {
            m_FirstZoneIndex = this.BuildingLogicData.BuildingPosition;
            this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(m_FirstZoneIndex);
            this.Build();
            //this.SetCellVisible(false);
            this.SetArrowState(false);
        }
    }
    protected void SearchBuildingPosition()
    {
        TilePosition searchZone = m_FirstZoneIndex;
        WorldTileRange cameraValidVisiableZone = CameraManager.Instance.GetValidVisibleRange();
        OnBFS(cameraValidVisiableZone, searchZone);
    }

    bool SearchBuildingPosition(TilePosition tilePosition)
    {
        if (CheckTile(tilePosition))
        { 
            m_FirstZoneIndex = tilePosition;
            return true;
        }
        else
        { 
            return false; 
        } 
    }
    protected void CreateCell()
    { 
        foreach (TilePosition tp in m_RealBuildObstalceList)
        {
           tk2dSlicedSprite cellTk2dSlicedSprite = (GameObject.Instantiate(Resources.Load(CELL, typeof(GameObject))) as GameObject).GetComponent<tk2dSlicedSprite>();
           cellTk2dSlicedSprite.transform.parent = this.transform;
           Vector3 localPosition = PositionConvertor.GetWorldPositionFromBuildingTileIndex(tp);
           localPosition.z = -50;//1;测试用
           cellTk2dSlicedSprite.transform.localPosition = localPosition + new Vector3(ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width >> 1, ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height >> 1);
           m_CellDict.Add(tp, cellTk2dSlicedSprite);
        }
    }
    protected void CreateArrow()
    {
        GameObject arrowGo = Resources.Load(ARROW, typeof(GameObject)) as GameObject;
        for (int i = 0; i < 4; i++)
        {
            tk2dSprite arrowUISprite = (GameObject.Instantiate(arrowGo) as GameObject).GetComponent<tk2dSprite>();
            arrowUISprite.transform.parent = this.transform;
            arrowUISprite.spriteId = arrowUISprite.GetSpriteIdByName(ClientSystemConstants.ARROW_ICON_DICTIONARY[i]);
            Vector3 localPosition = Vector3.zero;
            localPosition.x = m_ArrowOffset[i].x;
            localPosition.y = m_ArrowOffset[i].y;
            localPosition.z = 0;
            arrowUISprite.transform.localPosition = localPosition;
            m_ArrowList.Add(arrowUISprite);
        }
    }

    public void SetArrowState(bool isActive)
    {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            isActive = false;
        foreach (tk2dSprite uiSprite in m_ArrowList)
        {
            if (uiSprite != null)
            {
                uiSprite.gameObject.SetActive(isActive);
                if (isActive)
                {
                    Vector3 localPosition = uiSprite.transform.localPosition;
                    localPosition.z = -99;
                    uiSprite.transform.localPosition = localPosition;
                }
            }
        }
    }
    protected void CreateButton()
    {
        if (!this.Created)
        {
            m_ButtonOk = Instantiate(Resources.Load(BUTTON_OK, typeof(GameObject))) as GameObject;
            m_ButtonCancel = Instantiate(Resources.Load(BUTTON_CANCEL, typeof(GameObject))) as GameObject;
            m_ButtonOk.GetComponent<ButtonListener>().Controller = this;
            m_ButtonOk.GetComponent<ButtonListener>().Message = "OnConstructBuilding";
            m_ButtonOkBoxCollider = m_ButtonOk.GetComponent<BoxCollider>();
            m_ButtonCancel.GetComponent<ButtonListener>().Controller = this;
            m_ButtonCancel.GetComponent<ButtonListener>().Message = "OnCancel";
            m_ButtonOkUISprite = m_ButtonOk.transform.FindChild("Background").GetComponent<tk2dSprite>();
            m_ButtonOk.transform.parent = this.transform;
            m_ButtonCancel.transform.parent = this.transform;
            m_ButtonCancel.transform.localPosition = new Vector3(m_ButtonOffset[0].x, m_ButtonOffset[0].y, -10);
            m_ButtonOk.transform.localPosition = new Vector3(m_ButtonOffset[1].x, m_ButtonOffset[1].y, -10);
        }
    }
    protected void DestroyButton()
    {
        DestroyImmediate(m_ButtonOk);
        DestroyImmediate(m_ButtonCancel);
    }
    public bool CheckTile()
    {
        //int rowCount = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
        //int columnCount = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
        bool valid = true; 
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            TilePosition position = m_FirstZoneIndex + m_RealBuildObstalceList[i];
          
            if (!position.IsValidBuildingTilePosition())
            {
                SetCellColor(m_RealBuildObstalceList[i], false);
                valid = false; 
            }
            else
            {
                if (SceneManager.Instance.BuildingMapData[position.Row, position.Column] == false || position.IsEdgeBuildingTilePosition())
                {
                    SetCellColor(m_RealBuildObstalceList[i], false);
                    valid = false;
                }
                else
                {
                    SetCellColor(m_RealBuildObstalceList[i], true);
                }
            }
        }
        this.m_EnableCreate = valid;
        return valid;
    }
    public void SetTileState(bool isActive)
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SetCellColor(m_RealBuildObstalceList[i], isActive);
        }
    }
    bool CheckTile(TilePosition centerTilePosition)
    {
        //int rowCount = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
        //int columnCount = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
        //int obstacleRow = 0;
        //int obstacleColumn = 0;
        bool valid = true;
            for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
            {
                 //obstacleRow = centerTilePosition.Row + m_RealBuildObstalceList[i].Row;
                 //obstacleColumn = centerTilePosition.Column + m_RealBuildObstalceList[i].Column;
                 TilePosition position = centerTilePosition + m_RealBuildObstalceList[i];
                // if (obstacleRow > rowCount - 1 || obstacleRow < 0 || obstacleColumn > columnCount - 1 || obstacleColumn < 0)
                 if (!position.IsValidBuildingTilePosition())
                {
                    valid = false;
                    return valid;
                }
                 else
                 {
                     if (SceneManager.Instance.BuildingMapData[position.Row, position.Column] == false || position.IsEdgeBuildingTilePosition())
                     {
                         valid = false;
                         return valid;
                     }
                 }
            }
        return valid;
    }
    void SetCellColor(TilePosition cell, bool isValid)
    {
        m_CellDict[cell].color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
    }
    public void SetCellVisible(bool isVisible)
    {
        foreach (KeyValuePair<TilePosition, tk2dSlicedSprite> kv in m_CellDict)
        {
            Color color = m_CellDict[kv.Key].color;
            color.a = isVisible ? 0.5f : 0.0f;
            m_CellDict[kv.Key].color = color;
        }
    } 
    public void Build()
    {
        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[m_FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, m_FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = false;
            SceneManager.Instance.BuildingGameObjectData[m_FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, m_FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = this.gameObject;
        }
        TilePosition actorFirstZoneIndex = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(m_FirstZoneIndex);
        for (int i = 0; i < m_RealActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = false;
            SceneManager.Instance.ActorGameObjectData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = this.gameObject;
        }
        if (!this.m_FirstZoneIndex.Equals(this.BuildingLogicData.BuildingPosition))
        {
            LogicController.Instance.MoveBuilding(this.BuildingLogicData.BuildingIdentity, this.m_FirstZoneIndex);
            this.OnCreateBorder(BorderType.BuildingOutlineBorder);
        }
        else
        {
            this.OnDestroyBorder();
        }
        this.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(m_FirstZoneIndex);
        this.m_IsBuild = true;
        this.m_EnableCreate = true;
        switch (this.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.Wall:
                ((WallBehavior)this).SetSelfWallState();
                break;
        }
        this.CreateDropDownFX();
        this.SetCellVisible(false);
        
    } 
    public void UnBuild()
    {


        for (int i = 0; i < m_RealBuildObstalceList.Count; i++)
        {
            SceneManager.Instance.BuildingMapData[m_FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, m_FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = true;
            SceneManager.Instance.BuildingGameObjectData[m_FirstZoneIndex.Row + m_RealBuildObstalceList[i].Row, m_FirstZoneIndex.Column + m_RealBuildObstalceList[i].Column] = null;
        }
        TilePosition actorFirstZoneIndex = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(m_FirstZoneIndex);
        for (int i = 0; i < m_RealActorObstacleList.Count; i++)
        {
            //SceneManager.Instance.ActorMapData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = true;
            SceneManager.Instance.ActorGameObjectData[actorFirstZoneIndex.Row + m_RealActorObstacleList[i].Row, actorFirstZoneIndex.Column + m_RealActorObstacleList[i].Column] = null;
        }
        this.m_IsBuild = false;
        this.OnCreateBorder();
        switch (this.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.Wall:
                //if (SceneManager.Instance.SelectedAllWallList.Count == 0)
                    ((WallBehavior)this).SetOtherWallState(false);
                break;
        } 
    }
 
    public bool CheckBuildingCreateStack()
    {
        if (SceneManager.Instance.BuildingBehaviorTemporary)
            return SceneManager.Instance.BuildingBehaviorTemporary.Equals(this);
        return true;
    }

    public void ResetBuild()
    {
        print("ResetBuild");
        if (!this.Created)
            return;
        if (!m_IsBuild)
        {
            if (CheckTile())
            {Build();}
            else
            {
                if (/*m_BuildCenter*/this.BuildingLogicData.BuildingPosition != null)
                {
                    m_FirstZoneIndex = this.BuildingLogicData.BuildingPosition;// m_BuildCenter; 
                    Build();
                    //this.SetCellVisible(false);
                }
            }
        }
        //this.CreateBorder(BorderType.BuildingOutlineBorder);
    }
    protected void OnCreateBorder(BorderType borderType = BorderType.BuildingBorder)
    {
        SceneManager.Instance.CreateBuildingBorder(borderType);
    }

    protected void OnDestroyBorder()
    {
        SceneManager.Instance.DestroyBuildingBorder();
    }
    void OnBFS(WorldTileRange worldTileRange ,TilePosition startTilePosition)
    {
        int rowCount = Mathf.Abs(worldTileRange.Up - worldTileRange.Bottom);
        int columnCount = Mathf.Abs(worldTileRange.Right - worldTileRange.Left);
        TilePosition[,] tpArray = new TilePosition[rowCount, columnCount];
        m_AdjacencyList = new AdjacencyList<TilePosition>();
        int index = 0;
        int times = 0;
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                TilePosition tp = new TilePosition(column + worldTileRange.Left, row + worldTileRange.Bottom);
                m_AdjacencyList.AddVertex(tp);
                tpArray[row, column] = tp;
                if (tp.Equals(startTilePosition))
                {
                    index = times;
                }
                times++;
            }
        }
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if (column + 1 < columnCount)
                {
                    m_AdjacencyList.AddEdge(tpArray[row, column], tpArray[row, column + 1]);
                }
                if (row + 1 < rowCount)
                {
                    m_AdjacencyList.AddEdge(tpArray[row, column], tpArray[row + 1, column]);
                }
            }
        }
        m_AdjacencyList.BFSTraverse(index, SearchBuildingPosition);
    }

    public void ActiveButton(bool active)
    {
        if (this.m_ButtonOk != null && this.m_ButtonCancel != null)
        {
            this.m_ButtonOk.SetActive(active);
            this.m_ButtonCancel.SetActive(active);
        }
    }

    public void OnCancel()
    {
        SceneManager.Instance.PickableObjectCurrentSelect = null;
        SceneManager.Instance.EnableCreateWallContinuation = false;
        DestroyImmediate(this.gameObject);
        this.OnDestroyBorder();
    }
    void OnCancelConstruct()
    {
        UIManager.Instance.UIWindowConfirmPrompt.ShowWindow(StringConstants.PROMPT_CANCEL_UPGRADE, string.Format(StringConstants.PROMPT_CANCEL_UPGRADE_CONTEXT, ConfigInterface.Instance.SystemConfig.ResourceReturnRate * 100));
        UIManager.Instance.UIWindowConfirmPrompt.Click += () =>
            {
                if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
                {
                    if (this.BuildingLogicData.Level == 0)
                    {
                        LogicController.Instance.CancelBuildingConstruct(this.BuildingLogicData.BuildingIdentity);
                        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
                        this.UnBuild();
                        this.OnCancel();
                        this.OnDestroyBorder();
                    }
                    else
                    {
                        LogicController.Instance.CancelBuildingUpgrade(this.BuildingLogicData.BuildingIdentity);
                        
                    }
                }
            };
    }
    public void SetButtonOkState()
    {
        if (!this.Created)
        {
            this.m_ButtonOkUISprite.spriteId = m_EnableCreate ? m_ButtonOkUISprite.Collection.GetSpriteIdByName("Background green") : m_ButtonOkUISprite.Collection.GetSpriteIdByName("Background Gray");
            this.m_ButtonOkBoxCollider.enabled = m_EnableCreate;
        }
    }
    public bool ReadyForUpgrade()
    {
        //if (!m_IsBuild)
           // return false;
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return false;
        
        if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate)
        {
            this.CreateUpgradeFX();
            if (this.BuildingLogicData.Level == 0)
            {
                LogicController.Instance.FinishBuildingConstruct(this.BuildingLogicData.BuildingIdentity);
            }
            else
                LogicController.Instance.FinishBuildingUpgrade(this.BuildingLogicData.BuildingIdentity);
            SceneManager.Instance.ConstructBuilding(this.BuildingLogicData, true);
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            if (SceneManager.Instance.PickableObjectCurrentSelect != null)
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(false);
            SceneManager.Instance.PickableObjectCurrentSelect = null;
            Destroy(this.gameObject);
            this.enabled = false;
            return true;
        }
        else
            return false;
    }
    void OnReadyForUpgrade()
    {
        if (this.BuildingLogicData == null || SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate)
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect as BuildingBehavior == this)
            {
                UIManager.Instance.HidePopuBtnByCurrentSelect(true);
                this.m_IsClick = false;
                this.SetArrowState(false);
            }
                
        }
    }

    void CreateComponent()
    {
        //BuildingCommon m_BuildingCommon = null;
        //if (this.Created)
        {
            switch (this.BuildingType)
            {
                case BuildingType.ArmyCamp:
                    m_BuildingCommon = this.gameObject.AddComponent<ArmyCampBehavior>();
                    this.CreateBuildingWarringTitle();
                    break;
                case BuildingType.Barracks:
                    m_BuildingCommon = this.gameObject.AddComponent<BarrackBehavior>();
                    this.CreateBuildingWarringTitle();
                    break;
                case BuildingType.BuilderHut:
                    m_BuildingCommon = this.gameObject.AddComponent<BuilderHutBehavior>();
                    break;
                case BuildingType.CityHall:
                    m_BuildingCommon = this.gameObject.AddComponent<CityHallBehavior>();
                    break;
				/*
                case BuildingType.ClanCastle:
                    break;
                */
                case BuildingType.DefenseTower:
                    m_BuildingCommon = this.gameObject.AddComponent<DefenseTowerBehavior>();
                    break;
                case BuildingType.Farm:
                    m_BuildingCommon = this.gameObject.AddComponent<FarmBehavior>();
                    break;
                case BuildingType.FoodStorage:
                    m_BuildingCommon = this.gameObject.AddComponent<FoodStorageBehavior>();
                    break;
                case BuildingType.Fortress:
                    m_BuildingCommon = this.gameObject.AddComponent<FortressBehavior>();
                    break;
                case BuildingType.GoldMine:
                    m_BuildingCommon = this.gameObject.AddComponent<GoldMineBehavior>();
                    break;
                case BuildingType.GoldStorage:
                    m_BuildingCommon = this.gameObject.AddComponent<GoldStorageBehavior>();
                    break;
                case BuildingType.Tavern:
                     m_BuildingCommon = this.gameObject.AddComponent<TavernBehavior>();
                    break;
                case BuildingType.PropsStorage:
                    m_BuildingCommon = this.gameObject.AddComponent<PropsStorageBehavior>();
                    break;

                case BuildingType.Wall:
                    m_BuildingCommon = this.gameObject.AddComponent<BuildingCommon>();
                    break;
                case BuildingType.Artillery:
                    m_BuildingCommon = this.gameObject.AddComponent<ArtilleryBehavior>();
                    break;
            }
            m_BuildingCommon.BuildingLogicData = this.BuildingLogicData;
            m_BuildingCommon.BuildingConfigData = this.BuildingConfigData;
            m_BuildingCommon.BuildingBehavior = this;
            m_BuildingCommon.ProgressBarOffset = this.ProgressBarOffset;
            m_BuildingCommon.ProgressBarSize = this.ProgressBarSize;
            m_BuildingCommon.FacilityName = this.buildingFacility;

            //SceneManager.Instance.AddBuildingBehavior(this);
            this.CreateBuildingTitle();
        }

    }
    protected void CreateBuildingTitle()
    {
        this.m_TextMeshBuildingTitle = (Instantiate(Resources.Load(TEXT_BUILDING_TITLE, typeof(GameObject))) as GameObject).GetComponent<tk2dTextMesh>();
        this.m_TextMeshBuildingTitle.transform.parent = this.transform;
        this.m_TextMeshBuildingTitle.color = Color.clear;
        this.m_TextMeshBuildingTitle.Commit();
    }
    void CreateBuildingWarringTitle()
    {
        //this.m_TextMeshBuildingWarringTitle = (Instantiate(Resources.Load(TEXT_BUILDING_WARRING_TITLE, typeof(GameObject))) as GameObject).GetComponent<tk2dTextMesh>();
        //this.m_TextMeshBuildingWarringTitle.transform.parent = this.transform;
        //this.m_TextMeshBuildingWarringTitle.color = Color.clear;
        //this.m_TextMeshBuildingWarringTitle.Commit();
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        this.m_BuildingWarringBg = (Instantiate(Resources.Load(TEXT_BUILDING_WARRING_BACKGROUND, typeof(GameObject))) as GameObject).GetComponent<tk2dSprite>();
        m_BuildingWarringBg.transform.parent = this.transform;
        this.m_BuildingWarringBg.color = Color.clear;
        this.m_TextMeshBuildingWarringTitle = this.m_BuildingWarringBg.transform.FindChild(TEXT_BUILDING_WARRING_TITLE).GetComponent<tk2dTextMesh>();
        this.m_TextMeshBuildingWarringTitle.color = Color.clear;
        this.m_TextMeshBuildingWarringTitle.Commit();

        Vector3 localPosition = Vector3.zero;
        localPosition.x = this.ProgressBarOffset.x + m_WarringOffset.x;
        localPosition.y = this.ProgressBarOffset.y + m_WarringOffset.y;
        //localPosition.z = -10;
        //this.m_TextMeshBuildingWarringTitle.transform.localPosition = localPosition;
        localPosition.z = -110;
        this.m_BuildingWarringBg.transform.localPosition = localPosition;
    }
    public virtual void ShowBuildingTitle(bool isShow)
    {
        if (isShow)
        {
            this.m_TextMeshBuildingTitle.text = "   " + this.BuildingLogicData.Name + "\n" + StringConstants.LEFT_PARENTHESES + this.BuildingLogicData.Level + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV;
            
            int order = 0;
            if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
                order = 1;
            else
            {
                if (this.BuildingLogicData.ArmyProducts != null)
                    if (this.BuildingLogicData.ArmyProducts.Count > 0 && !this.BuildingLogicData.IsArmyProduceBlock)
                        order++;
                if (this.BuildingLogicData.ArmyUpgrade.HasValue)
                    order++;
                //if (this.BuildingLogicData.ItemProducts != null)
                //    if (this.BuildingLogicData.ItemProducts.Count > 0)
                //        order++;
                //if (this.BuildingLogicData.ItemUpgrade.HasValue)
                //    order++;
            }
            Vector3 localPosition = Vector3.zero;
            localPosition.x = this.ProgressBarOffset.x + m_BuildingTitleOffset.x;
            localPosition.y = this.ProgressBarOffset.y + m_BuildingTitleOffset.y + order * ClientConfigConstants.Instance.ProgressBarInterval;
            localPosition.z = -100;
            this.m_TextMeshBuildingTitle.transform.localPosition = localPosition;
        }
        //this.m_TextMeshBuildingTitle.GetComponent<TweenColortk2dSprite>().Play(isShow);
        this.m_TextMeshBuildingTitle.color = isShow ? Color.white : Color.clear;
        this.m_TextMeshBuildingTitle.Commit();
    }
    void OnShowBuildingWarringTitle()
    {
        if (this.BuildingType != BuildingType.ArmyCamp && this.BuildingType != BuildingType.Barracks && !this.Created || SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.BuildingLogicData == null)
            return;
        bool buildingState = this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal;
        switch (this.BuildingType)
        {
            case BuildingType.ArmyCamp:
                bool campsIsFull = LogicController.Instance.CurrentAvailableArmyCapacity >= LogicController.Instance.CampsTotalCapacity;
                this.m_TextMeshBuildingWarringTitle.text = StringConstants.PROMPT_IS_FULL;
                this.m_TextMeshBuildingWarringTitle.color = !this.m_IsClick & campsIsFull && buildingState ? Color.white : Color.clear;
                this.m_BuildingWarringBg.color = !m_IsClick & campsIsFull && buildingState ? Color.white : Color.clear;
                this.m_TextMeshBuildingWarringTitle.Commit();
                break;
            case BuildingType.Barracks:
                bool barrackIsFull = this.BuildingLogicData.IsArmyProduceBlock; 
                this.m_TextMeshBuildingWarringTitle.text = StringConstants.PROMPT_EXCLAMATION_POINT;
                this.m_TextMeshBuildingWarringTitle.color = !m_IsClick & barrackIsFull && buildingState ? Color.white : Color.clear;
                this.m_BuildingWarringBg.color = !m_IsClick & barrackIsFull && buildingState ? Color.white : Color.clear;
                this.m_TextMeshBuildingWarringTitle.Commit();
                break;
        }
    }
    void CreateBuildingObstacle()
    {
        if (!this.Created)
            m_RealBuildObstalceList = PositionConvertor.TilePointListToTilePositionList(this.BuildingConfigData.BuildingObstacleList);
        else
            m_RealBuildObstalceList = this.BuildingLogicData.BuildingObstacleList;
    }
    void CreateActorObstacle()
    {
        if (!this.Created)
        { 
            m_RealActorObstacleList = PositionConvertor.TilePointListToTilePositionList(this.BuildingConfigData.ActorObstacleList); 
            this.OnCreateBorder();
        }
        else 
            m_RealActorObstacleList = this.BuildingLogicData.ActorObstacleList; 
    }
    void CreateDropDownFX()
    {
        if (this.m_DropDownFX < 0)
        {
            this.m_DropDownFX++;
            return;
        }
        SceneManager.Instance.CreateSmokeFX(this);
        //AudioController.Play("BuildingDrop");
    }
    void CreateUpgradeFX()
    {
        SceneManager.Instance.CreateUpgradeFX(this);
    }
    bool OnCheckCollectValidity()
    {
        if (SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
        {
            if (this.BuildingLogicData.CanCollectGold)
                if (SystemFunction.CheckCollectValidity(this.BuildingLogicData, ResourceType.Gold) > 0)
                    return true;
            if (this.BuildingLogicData.CanCollectFood)
                if (SystemFunction.CheckCollectValidity(this.BuildingLogicData, ResourceType.Food) > 0)
                    return true;
            if (this.BuildingLogicData.CanCollectOil)
                if (SystemFunction.CheckCollectValidity(this.BuildingLogicData, ResourceType.Oil) > 0)
                    return true;
        }
        return false;
    }

    protected void OnActiveBuildingFX(bool active)
    {
        if (active)
        {
            iTween.ScaleTo(m_TweenColorBuildingBk.gameObject, iTween.Hash(iT.ScaleTo.scale, new Vector3(1.15f, 1.15f, 1), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.ScaleTo.looptype, iTween.LoopType.pingPong, iT.ScaleTo.time, 0.1f, iT.MoveTo.islocal, true));
            iTween.MoveTo(m_TweenColorBuildingBk.gameObject, iTween.Hash(iT.MoveTo.position, new Vector3(0, 15, 0), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.looptype, iTween.LoopType.pingPong, iT.MoveTo.time, 0.1f, iT.MoveTo.islocal, true));
            iTween.RotateTo(this.gameObject, iTween.Hash(iT.RotateTo.rotation, Vector3.zero, iT.RotateTo.oncomplete, "OnCompleteMoveTo", iT.RotateTo.delay, 0.2f, iT.RotateTo.time, 0.0f));

            m_TweenColorBuildingBk.delay = 0;
            m_TweenColorBuildingBk.duration = 0.8f;
            m_TweenColorBuildingBk.m_From = Color.white;
            m_TweenColorBuildingBk.m_To = Color.gray;
            m_TweenColorBuildingBk.style = UITweener.Style.PingPong;
            m_TweenColorBuildingBk.Play(true);
        }
        else
        {
            m_TweenColorBuildingBk.Reset();
            m_TweenColorBuildingBk.Color = Color.white;
            m_TweenColorBuildingBk.enabled = false;
        }
    }
    void OnCompleteMoveTo()
    {
        iTween.Stop(m_TweenColorBuildingBk.gameObject);
        m_TweenColorBuildingBk.gameObject.transform.localScale = Vector3.one;
        m_TweenColorBuildingBk.gameObject.transform.localPosition = Vector3.zero;
    }
}
