using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Linq;
using System.Collections.Generic;
public class NewbieGuide
{
    // 0 = mask window;1 = guide window;2 = login window;3 = guide window left bottom;4 = guide window left top
    protected Vector3[] PositionLayer = { new Vector3(0, 0, -600), new Vector3(0, 0, -2700), new Vector3(0, 0, -2800), new Vector3(-200, -200, -2700), new Vector3(-200, 200, -700) };
    protected string m_BuildingInfo = "Front/UIBtnInfo";
    float m_cellWidth = 305;
    public const string TAG_HIGHTLIGHT = "HightLight";
    public const string TAG_UNHIGHTLIGHT = "UnHightLight";
    protected void HightlightController(GameObject go , bool activeCollider = true)
    {
        if (go == null)
            return;
        this.SetSpriteColor(go, Color.white);
        this.SetLabelColor(go, Color.white);
        this.ActiveCollider(go, activeCollider);
    }
    protected void UnHightlightController(GameObject go, bool activeCollider = false)
    {
        if (go == null)
            return;
        this.SetSpriteColor(go, Color.gray);
        this.SetLabelColor(go, Color.gray);
        this.ActiveCollider(go, activeCollider);

    }
    protected void ActiveCollider(GameObject go, bool acitve)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        BoxCollider boxCollider = t.GetComponent<BoxCollider>();
        if (boxCollider != null)
            boxCollider.enabled = acitve;
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            ActiveCollider(child.gameObject, acitve);
        }
    }
    protected void ActiveColliderSelf(GameObject go, bool acitve)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        BoxCollider boxCollider = t.GetComponent<BoxCollider>();
        if (boxCollider != null)
            boxCollider.enabled = acitve;
    }
    void SetSpriteColorGray(GameObject go)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        UISprite uiSprite = t.GetComponent<UISprite>();
        tk2dSprite tk2d = t.GetComponent<tk2dSprite>();
        if (uiSprite != null)
        {
            float alpha = uiSprite.color.a;
            Color color = uiSprite.color / 2; 
            color.a = alpha;
            uiSprite.color = color;
        }
        if (tk2d != null && go.activeInHierarchy)
        {
            float alpha = tk2d.color.a;
            Color color = tk2d.color / 2;
            color.a = alpha;
            tk2d.color = color;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetSpriteColorGray(child.gameObject);
        }
    }

    void SetLabelColorGray(GameObject go)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        UILabel uiLabel = t.GetComponent<UILabel>();
        tk2dTextMesh tk2dText = t.GetComponent<tk2dTextMesh>();
        if (uiLabel != null)
        {
            float alpha = uiLabel.color.a;
            Color color = uiLabel.color / 2;
            color.a = alpha;
            uiLabel.color = color;
        }
        if (tk2dText != null  && go.activeInHierarchy)
        {
            float alpha = tk2dText.color.a;
            Color color = tk2dText.color / 2;
            color.a = alpha;
            tk2dText.color = color;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetLabelColorGray(child.gameObject);
        }
    }
    void SetSpriteColorFull(GameObject go)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        UISprite uiSprite = t.GetComponent<UISprite>();
        tk2dSprite tk2d = t.GetComponent<tk2dSprite>();
        if (uiSprite != null)
        {
            float alpha = uiSprite.color.a;
            Color color = uiSprite.color * 2;
            color.a = alpha;
            uiSprite.color = color;
        }
        if (tk2d != null && go.activeInHierarchy)
        {
            float alpha = tk2d.color.a;
            Color color = tk2d.color * 2;
            color.a = alpha;
            tk2d.color = color;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetSpriteColorFull(child.gameObject);
        }
    }
    void SetLabelColorFull(GameObject go)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        UILabel uiLabel = t.GetComponent<UILabel>();
        tk2dTextMesh tk2dText = t.GetComponent<tk2dTextMesh>();
        if (uiLabel != null)
        {
            float alpha = uiLabel.color.a;
            Color color = uiLabel.color * 2;
            color.a = alpha;
            uiLabel.color = color;
        }
        if (tk2dText != null && go.activeInHierarchy)
        {
            float alpha = tk2dText.color.a;
            Color color = tk2dText.color * 2;
            color.a = alpha;
            tk2dText.color = color;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetLabelColorFull(child.gameObject);
        }
    }

    void SetSpriteColor(GameObject go ,Color color)
    {
        if (go == null)
            return;
        go.tag = (color == Color.white) ? TAG_HIGHTLIGHT : TAG_UNHIGHTLIGHT;
        Transform t = go.transform;
        UISprite uiSprite = t.GetComponent<UISprite>();
        tk2dSprite tk2d = t.GetComponent<tk2dSprite>();

        if (uiSprite != null)
        {
            float alpha = uiSprite.alpha;
            uiSprite.color = color;
            uiSprite.alpha = alpha;
        }
        if (tk2d != null && go.activeInHierarchy)
        {
            float alpha = tk2d.color.a;
            Color col = color;
            col.a = alpha;
            tk2d.color = col;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetSpriteColor(child.gameObject, color);
        }
    }

    void SetLabelColor(GameObject go, Color color)
    {
        if (go == null)
            return;
        Transform t = go.transform;
        UILabel uiLabel = t.GetComponent<UILabel>();
        tk2dTextMesh tk2dText = t.GetComponent<tk2dTextMesh>();
        if (uiLabel != null)
        {
            float alpha = uiLabel.alpha;
            uiLabel.color = color;
            uiLabel.alpha = alpha;
        }
        if (tk2dText != null && go.activeInHierarchy)
        {
            float alpha = tk2dText.color.a;
            Color col = color;
            col.a = alpha;
            tk2dText.color = col;
        }
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetLabelColor(child.gameObject, color);
        }
    }
    //void SetSpriteDepth(GameObject go, int depth)
    //{
    //    Transform t = go.transform;
    //    UISprite uiSprite = t.GetComponent<UISprite>();
    //    if (uiSprite != null)
    //        uiSprite.depth += depth;
    //    for (int i = 0, imax = t.childCount; i < imax; ++i)
    //    {
    //        Transform child = t.GetChild(i);
    //        SetSpriteDepth(child.gameObject, depth);
    //    }
    //}
    //void SetLabelDepth(GameObject go, int depth)
    //{
    //    Transform t = go.transform;
    //    UILabel uiLabel = t.GetComponent<UILabel>();
    //    if (uiLabel != null)
    //        uiLabel.depth += depth;
    //    for (int i = 0, imax = t.childCount; i < imax; ++i)
    //    {
    //        Transform child = t.GetChild(i);
    //        SetLabelDepth(child.gameObject, depth);
    //    }
    //}
    //public GameObject ChangeParentNode(GameObject child,GameObject parent)
    //{
    //    GameObject sourceParent = child.transform.parent.gameObject;
    //    child.transform.parent = parent.transform;
    //    child.SetActive(false);
    //    child.SetActive(true);
    //    return sourceParent;
    //}
    protected DynamicInvokeGuide AddDynamicGuide(GameObject go)
    {
       return go.AddComponent<DynamicInvokeGuide>();
    }
    public void SetRollPanelLocalPosition(GameObject go, int cellIndext)
    {
        UIDraggablePanel uiDraggablePanel = go.GetComponent<UIDraggablePanel>();
        if (uiDraggablePanel != null)
            uiDraggablePanel.MoveRelative(new Vector3(-1 * this.m_cellWidth * cellIndext, 0, 0));
    }
    public void ResetAll()
    {
        NewbieGuideManager.Instance.UIWindowGuide.HideWindow(false);
        //NewbieGuideManager.Instance.UIWindowGuide.ActiveChild(true);
        NewbieGuideManager.Instance.UIWindowGuide.SetClickState(true);
        for (int i = 0; i < NewbieGuideManager.Instance.MainUIControllerParent.Length; i++)
            NewbieGuideManager.Instance.MainUIControllerParent[i].SetActive(true);

        for (int i = 0; i < NewbieGuideManager.Instance.MainUIController.Length; i++)
        {
            NewbieGuideManager.Instance.MainUIController[i].SetActive(true);
            this.HightlightController(NewbieGuideManager.Instance.MainUIController[i]);
        }

        this.SetExpProgressBarHightlight();
        this.SetGoldProgressBarHightlight();
        this.SetFoodProgressBarHightlight();

        this.SetAllBuildingColorHightlight();
        HightlightController(SceneManager.Instance.AgeMap);
        HightlightController(NewbieGuideManager.Instance.PopupController[0]);
        ActiveCollider(CameraManager.Instance.gameObject, true);
        this.DestroyGuideArrow();
        this.ActiveUILayerUICameraInput(true);
        this.ActiveSceneLayerUICameraInput(true);

    }
    protected void ActiveUILayerUICameraInput(bool active)
    {
        NewbieGuideManager.Instance.UICameraInput[0].eventReceiverMask.value = active ? NewbieGuideManager.Instance.UICameraInput[0].cachedCamera.cullingMask : 0;
    }
    protected void ActiveSceneLayerUICameraInput(bool active)
    {
        NewbieGuideManager.Instance.UICameraInput[1].eventReceiverMask.value = active ? NewbieGuideManager.Instance.UICameraInput[1].cachedCamera.cullingMask : 0;
    }
    // progress bar use it only
    protected void SetExpProgressBarHightlight()
    {
        NewbieGuideManager.Instance.MainUIBarSprite[0].color = NewbieGuideManager.Instance.MainUIBarSpriteColor[0];
    }
    // progress bar use it only
    protected void SetGoldProgressBarHightlight()
    {
        NewbieGuideManager.Instance.MainUIBarSprite[1].color = NewbieGuideManager.Instance.MainUIBarSpriteColor[1];
    }
    // progress bar use it only
    protected void SetFoodProgressBarHightlight()
    {
        NewbieGuideManager.Instance.MainUIBarSprite[2].color = NewbieGuideManager.Instance.MainUIBarSpriteColor[2];
    }

    protected void SetAllMainUIColorGray()
    {
        for (int i = 0; i < NewbieGuideManager.Instance.MainUIControllerParent.Length; i++)
        {
            SetSpriteColorGray(NewbieGuideManager.Instance.MainUIControllerParent[i]);
            SetLabelColorGray(NewbieGuideManager.Instance.MainUIControllerParent[i]);
            ActiveCollider(NewbieGuideManager.Instance.MainUIControllerParent[i],false);
        }
    }
    protected void SetAllMainUIColorFull(bool activeCollider = false)
    {
        for (int i = 0; i < NewbieGuideManager.Instance.MainUIControllerParent.Length; i++)
        {
            SetSpriteColorFull(NewbieGuideManager.Instance.MainUIControllerParent[i]);
            SetLabelColorFull(NewbieGuideManager.Instance.MainUIControllerParent[i]);
            ActiveCollider(NewbieGuideManager.Instance.MainUIControllerParent[i], activeCollider);
        }
    }
    protected void SetAllMainUIActive(bool activeCollider)
    {
        for (int i = 0; i < NewbieGuideManager.Instance.MainUIControllerParent.Length; i++) 
            ActiveCollider(NewbieGuideManager.Instance.MainUIControllerParent[i], activeCollider);
    }
    protected void SetAllBuildingActive(bool activeCollider)
    {
        for (int i = 0; i < LogicController.Instance.AllBuildings.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllBuildings[i].BuildingPosition.Row, LogicController.Instance.AllBuildings[i].BuildingPosition.Column);
            ActiveCollider(go, activeCollider);
        }
        for (int i = 0; i < LogicController.Instance.AllRemovableObjects.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Row, LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Column);
            ActiveCollider(go, activeCollider);
        }
    }
    protected void SetAllBuildingColorUnHightlight(bool activeCollider = false)
    {
        for (int i = 0; i < LogicController.Instance.AllBuildings.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllBuildings[i].BuildingPosition.Row, LogicController.Instance.AllBuildings[i].BuildingPosition.Column);
            this.UnHightlightController(go, activeCollider);
        }
        for (int i = 0; i < LogicController.Instance.AllRemovableObjects.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Row, LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Column);
            this.UnHightlightController(go, activeCollider);
        }
    }
    protected void SetAllBuildingColorHightlight(bool activeCollider = true)
    {
        for (int i = 0; i < LogicController.Instance.AllBuildings.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllBuildings[i].BuildingPosition.Row, LogicController.Instance.AllBuildings[i].BuildingPosition.Column);
            this.HightlightController(go, activeCollider);
        }
        for (int i = 0; i < LogicController.Instance.AllRemovableObjects.Count; i++)
        {
            GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Row, LogicController.Instance.AllRemovableObjects[i].BuildingPosition.Column);
            this.HightlightController(go, activeCollider);
        }
    }
    protected void ColorGrayCotroller(GameObject go, bool activeCollider = false)
    {
        this.SetSpriteColorGray(go);
        this.SetLabelColorGray(go);
        this.ActiveCollider(go, activeCollider);
    }
    protected void ColorFullCotroller(GameObject go, bool activeCollider = true)
    {
         this.SetSpriteColorFull(go);
         this.SetLabelColorFull(go);
         this.ActiveCollider(go, activeCollider);
    }
    protected void RemoveDynamicGuide(GameObject go)
    {
        DynamicInvokeGuide[] dynamicInvokeGuides = go.GetComponents<DynamicInvokeGuide>();
        foreach (DynamicInvokeGuide dig in dynamicInvokeGuides)
            Object.Destroy(dig);
    }
 
    protected void RemoveDynamicGuide(DynamicInvokeGuide dynamicInvokeGuide)
    {
        Object.Destroy(dynamicInvokeGuide);
    }
    protected void CreateUIGuideArrow(GameObject refObjectm, ArrowOffsetType arrowOffsetType, ArrowDirection arrowDirection)
    {
        if (refObjectm != null)
            this.CreateGuideArrow(refObjectm, LayerEnum.UILayer, arrowOffsetType, arrowDirection);
    }
    protected void CreateSceneGuideArrow(GameObject refObjectm, ArrowOffsetType arrowOffsetType, ArrowDirection arrowDirection)
    {
        if (refObjectm != null)
            this.CreateGuideArrow(refObjectm, LayerEnum.SceneLayer, arrowOffsetType, arrowDirection);
    }
    void CreateGuideArrow(GameObject refObjectm, LayerEnum layerEnum, ArrowOffsetType arrowOffsetType, ArrowDirection arrowDirection)
    {
        this.DestroyGuideArrow();
        switch (layerEnum)
        {
            case LayerEnum.UILayer:
                NewbieGuideManager.Instance.CurrentGuideArrow = Object.Instantiate(NewbieGuideManager.Instance.UILayerGuideArrowPrefab[(int)arrowDirection]) as GameObject;
                break;
            case LayerEnum.SceneLayer:
                NewbieGuideManager.Instance.CurrentGuideArrow = Object.Instantiate(NewbieGuideManager.Instance.SceneLayerGuideArrowPrefab[(int)arrowDirection]) as GameObject;
                break;
        }
        
        NewbieGuideManager.Instance.CurrentGuideArrow.transform.parent = refObjectm.transform;
        NewbieGuideManager.Instance.CurrentGuideArrow.transform.localPosition = NewbieGuideManager.Instance.GuideArrowOffset[(int)arrowOffsetType];
        //NewbieGuideManager.Instance.CurrentGuideArrow.transform.localEulerAngles = new Vector3(0, 0, ((int)arrowDirection) * 90);
    }
    protected void DestroyGuideArrow()
    {
        Object.DestroyImmediate(NewbieGuideManager.Instance.CurrentGuideArrow);
    }
    protected void CameraFollowTarget(Transform target)
    {
        Vector2 targetPosition = CameraManager.Instance.GetCameraMoveToValidPosition(target.position);
        iTween.MoveTo(CameraManager.Instance.MainCamera.gameObject, iTween.Hash(iT.MoveTo.position, new Vector3(targetPosition.x,targetPosition.y,0), iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.speed, 500f));
        this.ActiveSceneLayerUICameraInput(false);
        NewbieGuideManager.Instance.TriggerCondition(() => { return Vector2.Distance(CameraManager.Instance.MainCamera.transform.position, targetPosition) < 10; }, () => { this.ActiveSceneLayerUICameraInput(true); });
    }
    protected void ActiveBuildingInfoCollider(GameObject parent, bool active)
    {
        Transform t = parent.transform.FindChild(this.m_BuildingInfo);
        if (t != null)
            this.ActiveCollider(t.gameObject, active);
    }
    #region condition
    protected bool ConditionExistBuilding(BuildingType buildingType)
    {
        return LogicController.Instance.GetBuildingCount(buildingType) > 0;
    }

    protected bool ConditionExistBuildingLevel(BuildingType buildingType, int level)
    {
        if (LogicController.Instance.GetBuildings(buildingType).Count > 0)
        {
            return LogicController.Instance.GetBuildings(buildingType)[0].Level == level;
        }
        return false;
    }
    protected bool ConditionExistBuildingState(BuildingType buildingType, BuildingEditorState buildingEditorState)
    {
        if (LogicController.Instance.GetBuildings(buildingType).Count > 0)
        {
            return LogicController.Instance.GetBuildings(buildingType)[0].CurrentBuilidngState.Equals(buildingEditorState);
        }
        return false;
    }
    protected bool ConditionRemovableObectState(RemovableObjectType removableObjectType, RemovableObjectEditorState removableObjectEditorState)
    {
        return LogicController.Instance.AllRemovableObjects.Count(a => a.ObjectType.Equals(removableObjectType) && a.EditorState != removableObjectEditorState) > 0;
    }
    protected bool ConditionPlant(RemovableObjectType removableObjectType)
    {
        return LogicController.Instance.AllRemovableObjects.Count(a => a.ObjectType.Equals(removableObjectType)) > 0;
    }
    #endregion
    public virtual void OnIntroduction()
    { }
    public virtual void OnIntroLogin()
    { }
    public virtual void OnIntroConstructGoldMine()
    { }
    public virtual void OnIntroConstructGoldStorage()
    { }
    public virtual void OnIntroConstructFarm()
    { }
    public virtual void OnIntroConstructFoodStorage()
    { }
    public virtual void OnIntroConstructArmyCamp()
    { }
    public virtual void OnIntroConstructBarracks()
    { }
    public virtual void OnIntroAttack()
    { }
    public virtual void OnIntroConstructFortress()
    { }
    public virtual void OnIntroUpgradeCityHall()
    { }
    public virtual void OnIntroConstructBuilderHut()
    { }
    public virtual void OnIntroUpgradeBarracks()
    { }
    public virtual void OnIntroTask()
    { }
    public virtual void OnConstructGoldMine()
    { }
    public virtual void OnConstructGoldStorage()
    { }
    public virtual void OnConstructFarm()
    { }
    public virtual void OnConstructFoodStorage()
    { }
    public virtual void OnConstructArmyCamp()
    { }
    public virtual void OnConstructBarracks()
    { }
    public virtual void OnProduceArmy()
    { }
    public virtual void OnConstructFortress()
    { }
    public virtual void OnUpgradeCityHall()
    { }
    public virtual void OnConstructBuilderHut()
    { }
    public virtual void OnUpgradeBarracks()
    { }
    public virtual void OnIntroPropsStorage()
    { }
    public virtual void OnConstructPropsStorage()
    { }
    public virtual void OnIntroPlant()
    { }
    public virtual void OnIntroRemoveObstacle()
    { }
    public virtual void OnRemoveObstacle()
    { }

    
}

public enum LayerEnum
{
    UILayer,
    SceneLayer,
    NewbieSceneLayer,
    NewbieUILayer
}
public enum ArrowDirection
{
    Down,
    Right,
    Up,
    Left
}
public enum ArrowOffsetType
{
    UIBuildingButton,
    UIResourceMenu,
    UIMilitaryMenu,
    UIDefenseMenu,
    UIBuyBuildingButton,
    SceneBuildingOkButton,
    UISelectBuilderButton1,
    UISelectBuilderButton2,
    SceneConstructBuilding,
    UIExpBar,
    UIGoldBar,
    UIFoodBar,
    UIExpBarText,
    UIGoldBarText,
    UIFoodBarText, 
    UIBuilderBar,
    UIPopupButtonImmediately,
    UICostGemImmediatelyButton,
    UITPopupButtonTrainTroops,
    UIBuyArmy,
    UIProduceArmyImmediately,
    UIAttackButton,
    SceneSelectBuilding,
    UIPopupButtonUpgrade,
    UITaskButton,
    UICompositeMenu,
    UIPopupButtonRemoveObstacel,
    UIPopupButtonPropsStorage,
    UIPropsCell,
    UICloseWindowPropStorage,
    UIPlantMenu,
    UIButtonConformPlant
}