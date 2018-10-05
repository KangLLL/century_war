using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraManager : MonoBehaviour {
    [SerializeField] Camera m_MainCamera;
    public Camera MainCamera { get { return m_MainCamera; } }
    const float ATTENUATION_FACTOR = 0.8f;
    const float WEIGHT_ATTENUATION_FACTOR = 0.5f;
    const float STOP_MOVE_THRESHOLE = 1.0f;
    const float CAMERA_SIZE_STANDARD = ClientSystemConstants.CAMERA_SIZE_STANDARD;
    float CAMERA_SIZE_MIN = ClientSystemConstants.CAMERA_SIZE_MIN;
    float CMAERA_SIZE_MAX = ClientSystemConstants.CAMERA_SIZE_MAX;
    int MAP_ROW = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
    int MAP_COLUMN = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
    WorldRange m_MapWorldRange = ClientSystemConstants.WORLDRANGE;

    public WorldRange MapWorldRange
    { get { return m_MapWorldRange; } }
    public WorldTileRange GetValidVisibleRange()
    { 
        float heightHaft = m_MainCamera.orthographicSize;
        float widthHaft = heightHaft * m_MainCamera.aspect;
        int rowCountHaft = (int)(heightHaft / ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height);
        int columnCountHaft = (int)(widthHaft / ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width);
        TilePosition cameraCenterZone = PositionConvertor.GetBuildingTileIndexFromWorldPosition(m_MainCamera.transform.position);
        WorldTileRange validVisibleRange = new WorldTileRange(Mathf.Clamp(cameraCenterZone.Row + rowCountHaft, 0, MAP_ROW),
                                                      Mathf.Clamp(cameraCenterZone.Row - rowCountHaft, 0, MAP_ROW),
                                                      Mathf.Clamp(cameraCenterZone.Column - columnCountHaft, 0, MAP_COLUMN),
                                                      Mathf.Clamp(cameraCenterZone.Column + columnCountHaft, 0, MAP_COLUMN));
        return validVisibleRange;
    }
    Vector3 m_PressPosition;
    Vector3 m_CameraPosition;
    bool m_EnableMoveAutomatic = false;
    Vector3 m_LastCameraPosition;
    Vector2 m_WeightedDelta;
	float m_CameraSize;

    static CameraManager S_Instance;
    public static CameraManager Instance { get { return S_Instance; } }
    Dictionary<int, UICamera.MouseOrTouch> m_MouseOrTouchDictionary = new Dictionary<int, UICamera.MouseOrTouch>();
    void Awake()
    {
        S_Instance = this;
    }
    void Update()
    {
        CameraMoveAutomatic();
        #if UNITY_EDITOR 
		OnMouseRoll();
        #endif
    }

    public void OnPress(bool isPress)
    { 
        if (isPress)
        {
            if (!m_MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionary.Add(UICamera.currentTouchID, UICamera.currentTouch);
            this.m_EnableMoveAutomatic = false;
        }
        else
            if (m_MouseOrTouchDictionary.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionary.Remove(UICamera.currentTouchID);
    }
    public void OnDrag(Vector2 delta)
    {
        if (m_MouseOrTouchDictionary.Count == 1) 
            this.SetCameraPosition(m_MouseOrTouchDictionary); 
        else
            if (m_MouseOrTouchDictionary.Count > 1)
            { 
                this.SetCameraPosition(m_MouseOrTouchDictionary);
                this.SetCameraScale(m_MouseOrTouchDictionary);
            } 
		LimitCameraPosition(); 
    }

    public void OnDragExt(Dictionary<int, UICamera.MouseOrTouch> mouseOrTouchDictionary)
    {
        if (mouseOrTouchDictionary.Count == 1)
            this.SetCameraPosition(mouseOrTouchDictionary);
        else
            if (mouseOrTouchDictionary.Count > 1)
            {
                this.SetCameraPosition(mouseOrTouchDictionary);
                this.SetCameraScale(mouseOrTouchDictionary);
            }
        LimitCameraPosition(); 
    }

    public void OnClick()
    {
        CheckMoveDelta();
    }
    //Test 
    void OnDrop(GameObject go)
    {
        print("camera OnDrop");
        CheckMoveDelta();
    }
    void SetCameraScale(Dictionary<int, UICamera.MouseOrTouch> mouseOrTouchDictionary)
    {
        List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(mouseOrTouchDictionary.Values);
        m_CameraSize = m_MainCamera.orthographicSize; 
        float fingerBeganDistance = Vector2.Distance(this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[0].pos - mouseOrTouchList[0].delta), this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[1].pos - mouseOrTouchList[1].delta));
        float fingerMoveDistance = Vector2.Distance(this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[0].pos) ,this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[1].pos)); 
        float cameraChangeScale = (fingerBeganDistance - fingerMoveDistance) * 0.75f;
        if (cameraChangeScale != 0) 
            m_MainCamera.orthographicSize = Mathf.Clamp(m_CameraSize + cameraChangeScale, CAMERA_SIZE_MIN, CMAERA_SIZE_MAX); 
    }
    void SetCameraPosition(Dictionary<int, UICamera.MouseOrTouch> mouseOrTouchDictionary)
    {
        m_CameraPosition = m_MainCamera.transform.position;
        List<UICamera.MouseOrTouch> mouseOrTouchList = new List<UICamera.MouseOrTouch>(mouseOrTouchDictionary.Values);
        Vector2 deltaSumWorld = Vector2.zero;
        for (int i = 0; i < mouseOrTouchList.Count; i++)
        {
            Vector2 deltaWorld = this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[i].pos) - this.m_MainCamera.ScreenToWorldPoint(mouseOrTouchList[i].pos - mouseOrTouchList[i].delta);
            deltaSumWorld += deltaWorld;
        }
        deltaSumWorld = deltaSumWorld / Mathf.Pow(mouseOrTouchList.Count, 2);
        m_CameraPosition.x -= deltaSumWorld.x;
        m_CameraPosition.y -= deltaSumWorld.y;
        m_WeightedDelta = deltaSumWorld * WEIGHT_ATTENUATION_FACTOR + m_WeightedDelta * (1 - WEIGHT_ATTENUATION_FACTOR);
        m_MainCamera.transform.position = m_CameraPosition;
    }
    void LimitCameraPosition()
    {
        Vector3 position = m_MainCamera.transform.position;
        float height = m_MainCamera.orthographicSize * 2;
        float width = height * m_MainCamera.aspect;
        float borderLeft = position.x - width * 0.5f;
        float borderRight = position.x + width * 0.5f;
        float borderUp = position.y + height * 0.5f;
        float borderBottom = position.y - height * 0.5f;

        if (borderLeft < m_MapWorldRange.Left) 
            position.x = m_MapWorldRange.Left + width * 0.5f; 

        if (borderRight > m_MapWorldRange.Right) 
            position.x = m_MapWorldRange.Right - width * 0.5f;

        if (borderUp > m_MapWorldRange.Up) 
            position.y = m_MapWorldRange.Up - height * 0.5f; 

        if (borderBottom < m_MapWorldRange.Bottom) 
            position.y = m_MapWorldRange.Bottom + height * 0.5f; 

        m_MainCamera.transform.position = position;
    }
    public Vector3 GetCameraMoveToValidPosition(Vector3 target)
    { 
        float height = m_MainCamera.orthographicSize * 2;
        float width = height * m_MainCamera.aspect;
        float borderLeft = target.x - width * 0.5f;
        float borderRight = target.x + width * 0.5f;
        float borderUp = target.y + height * 0.5f;
        float borderBottom = target.y - height * 0.5f;

        if (borderLeft < m_MapWorldRange.Left)
            target.x = m_MapWorldRange.Left + width * 0.5f;

        if (borderRight > m_MapWorldRange.Right)
            target.x = m_MapWorldRange.Right - width * 0.5f;

        if (borderUp > m_MapWorldRange.Up)
            target.y = m_MapWorldRange.Up - height * 0.5f;

        if (borderBottom < m_MapWorldRange.Bottom)
            target.y = m_MapWorldRange.Bottom + height * 0.5f;

        return target;
    }
    void CheckMoveDelta()
    {
		if(m_MouseOrTouchDictionary.Count != 0)
			return;
        if (m_WeightedDelta.magnitude > STOP_MOVE_THRESHOLE)
            m_EnableMoveAutomatic = true;
         
    }
 
    void CameraMoveAutomatic()
    { 
        m_WeightedDelta *= ATTENUATION_FACTOR;
        if (m_EnableMoveAutomatic)
        {
            Vector3 position = m_MainCamera.transform.position;
            position.x -= m_WeightedDelta.x;
            position.y -= m_WeightedDelta.y;
            m_MainCamera.transform.position = position;
            if (m_WeightedDelta.magnitude < STOP_MOVE_THRESHOLE)
            {
                m_EnableMoveAutomatic = false;
                this.m_WeightedDelta = Vector2.zero;
            }
            LimitCameraPosition();
        }
    }
 
    #if UNITY_EDITOR 
	void OnMouseRoll()
	{
        m_MainCamera.orthographicSize += Input.GetAxisRaw("Mouse ScrollWheel") * 80;
        if (m_MainCamera.orthographicSize < CAMERA_SIZE_MIN)
            m_MainCamera.orthographicSize = CAMERA_SIZE_MIN;
        if (m_MainCamera.orthographicSize > CMAERA_SIZE_MAX)
            m_MainCamera.orthographicSize = CMAERA_SIZE_MAX;
        Vector3 position = m_MainCamera.transform.position;
        if (Input.GetKey(KeyCode.A))
            position -= new Vector3(Time.deltaTime * 100, 0, 0);

        if (Input.GetKey(KeyCode.D))
            position += new Vector3(Time.deltaTime * 100, 0, 0);

        if (Input.GetKey(KeyCode.W))
            position += new Vector3(0, Time.deltaTime * 100, 0);

        if (Input.GetKey(KeyCode.S))
            position -= new Vector3(0, Time.deltaTime * 100, 0);
        m_MainCamera.transform.position = position;
        LimitCameraPosition();
    }
    #endif
}
public struct WorldRange
{
    public float Up;
    public float Bottom;
    public float Left;
    public float Right;
    public WorldRange(float _Up, float _Bottom, float _Left, float _Right)
    {
        Up = _Up;
        Bottom = _Bottom;
        Left = _Left;
        Right = _Right;
    }
}
public struct WorldTileRange
{
    public int Up;
    public int Bottom;
    public int Left;
    public int Right;
    public WorldTileRange(int _Up, int _Bottom, int _Left, int _Right)
    {
        Up = _Up;
        Bottom = _Bottom;
        Left = _Left;
        Right = _Right;
    }
}


