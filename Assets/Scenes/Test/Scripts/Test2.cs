using UnityEngine;
using System.Collections;

public class Test2 :MonoBehaviour
{
    [SerializeField]
    UILabel m_UILabel;
    public void SetItemData(string text)
    {
        m_UILabel.text = text;
    }
}
