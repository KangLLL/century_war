using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities.Enums;
 /*
public class BuildGenerate : MonoBehaviour {
    [SerializeField] GameObject[] m_BuildingSource;
    SceneManager m_SceneManager;
	// Use this for initialization
	void Start () {
        m_SceneManager = GetComponent<SceneManager>();
	}
	// Update is called once per frame
	void Update () {
	}
    void CreateBuilding(BuildingType type)
    {
        //if (m_SceneManager.BuildCreateStack.Count == 0)
        while (m_SceneManager.BuildCreateStack.Count != 0)
        {
            Destroy(m_SceneManager.BuildCreateStack.Pop().gameObject);
        }
            BuildingBehavior buildingBehavior = (GameObject.Instantiate(m_BuildingSource[(int)type]) as GameObject).GetComponent<BuildingBehavior>();
            //buildingBehavior.BuildTypes = type;
            buildingBehavior.transform.parent = this.transform;
            //m_SceneManager.BuildingList.Add(buildingBehavior);
            m_SceneManager.BuildCreateStack.Push(buildingBehavior);
        
    }
    void OnCreateBuilding(object param)
    {
        CreateBuilding((BuildingType)param);
    } 

}
*/