using UnityEngine;
using System.Collections;

[System.Serializable]
public class StringAndGameObjectKeyValuePair
{
    public string key;
    public GameObject value;
}

[System.Serializable]
public class StringAndGameObjectDictionary
{
    [SerializeField]
    private System.Collections.Generic.List<StringAndGameObjectKeyValuePair> m_KeyValueList;


    public System.Collections.Generic.Dictionary<string, GameObject> KeyValueDictionary
    {
        get
        {
            System.Collections.Generic.Dictionary<string, GameObject> dictionary = new System.Collections.Generic.Dictionary<string, GameObject>();
            foreach (StringAndGameObjectKeyValuePair keyValuePair in m_KeyValueList)
            {
                dictionary.Add(keyValuePair.key, keyValuePair.value);
            }
            return dictionary;
        }
    }

    public System.Collections.Generic.List<StringAndGameObjectKeyValuePair> KeyValuePairList
    {
        get
        {
            return m_KeyValueList;
        }
    }
}
