using UnityEngine;
using System.Collections;
  
public class UIWindowItemCommon : MonoBehaviour {
    public BuildingLogicData BuildingLogicData { get; set; }
    public AchievementBuildingLogicData AchievementBuildingLogicData { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
    public virtual void SetWindowItem()
    {
        print("000000000000000");
    }
    public virtual void SetWindowItemAchievementBuilding()
    {}
}
