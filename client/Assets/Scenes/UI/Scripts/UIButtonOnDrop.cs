using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UIButtonOnDrop : MonoBehaviour 
{
    void OnDrop(GameObject go)
    {
        //BuildingBehavior buildingBehavior = go.GetComponent<BuildingBehavior>();
        //if (buildingBehavior != null)
        //{
        //    if (buildingBehavior.BuildingType != BuildingType.Wall)
        //    {
        //        print("buildingBehavior.gameobject.name =" + buildingBehavior.gameObject.name);
        //        print("buildingBehavior.gameobject.name =" + buildingBehavior.GetType().ToString());
        //        buildingBehavior.OnClick();
        //    }
        //    else
        //    {
        //        BuildingCommon buildingCommon = go.GetComponent(typeof(BuildingCommon)) as BuildingCommon;
        //        if (buildingCommon != null)
        //            buildingCommon.OnClick();
        //    }
        //}
        //else
        //{
        //    RemovableObjectBehavior removableObjectBehavior = go.GetComponent<RemovableObjectBehavior>() as RemovableObjectBehavior;
        //    if (removableObjectBehavior != null)
        //        removableObjectBehavior.OnClick();
        //}
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
            SceneManager.Instance.PickableObjectCurrentSelect.OnClick();
    }
    void OnClick()
    {
        SceneManager.Instance.UnSelectBuilding();
    }
 
}
