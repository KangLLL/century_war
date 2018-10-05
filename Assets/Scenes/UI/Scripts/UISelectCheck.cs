using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
public class UISelectCheck : MonoBehaviour 
{
    Vector3 m_PressPosition;
    bool m_Drag = false;
    void OnPress(bool isPressed)
    {
        if (isPressed)
        { 
            m_PressPosition = CameraManager.Instance.MainCamera.transform.position;
            this.m_Drag = false;
        }
    } 
    void OnClick()
    {
        if (!this.m_Drag)
        {
            if (SceneManager.Instance.PickableObjectCurrentSelect != null && UIManager.Instance.UIWindowFocus == null)
            {
                SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(true);
                SceneManager.Instance.PickableObjectCurrentSelect = null;
            }
        }
    }
    void OnDrag(Vector2 delta)
    {
        if((m_PressPosition - CameraManager.Instance.MainCamera.transform.position).magnitude > 16)
        {
            this.m_Drag = true;
        }
    }
	void OnDrop(GameObject go)
	{
        /*
	    BuildingBehavior buildingBehavior = go.GetComponent<BuildingBehavior>();
        if (buildingBehavior != null)
        {
            if (buildingBehavior.BuildingType != BuildingType.Wall)
                buildingBehavior.OnClick();
            else
            {
                BuildingCommon buildingCommon = go.GetComponent(typeof(BuildingCommon)) as BuildingCommon;
                if (buildingCommon != null)
                    buildingCommon.OnClick();
            }
        }
        else
        {
            RemovableObjectBehavior removableObjectBehavior = go.GetComponent<RemovableObjectBehavior>() as RemovableObjectBehavior;
            if (removableObjectBehavior != null)
                removableObjectBehavior.OnClick();
        }
        */
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
            SceneManager.Instance.PickableObjectCurrentSelect.OnClick();
	}
}
