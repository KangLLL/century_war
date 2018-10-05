using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class PrefabDictionary
{
    [SerializeField]
    private StringPrefab[] stringPrefabs;
    Dictionary<string, GameObject> dict;
    public GameObject this[string enumName]
    {
        get
        {
            if (dict == null)
            {
                dict = new Dictionary<string, GameObject>();
                foreach (StringPrefab stringPrefab in stringPrefabs)
                    dict.Add(stringPrefab.enumName, stringPrefab.gameObject);
            }
            return this.dict[enumName];
        }
    }
}

[Serializable]
public class StringPrefab
{
    public string enumName;
    public GameObject gameObject;
}