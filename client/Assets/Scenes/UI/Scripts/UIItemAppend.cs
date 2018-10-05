using UnityEngine;
using System.Collections;

public class UIItemAppend : MonoBehaviour {
    [SerializeField] UILabel[] m_UILabelText;
    [SerializeField] UISprite m_UISpriteIcon;//base
    [SerializeField] UISprite[] m_UISprite;//extra
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetItemData(string spriteName,bool activeIcon , params string[] value)
    {
        for (int i = 0, count = m_UILabelText.Length; i < count; i++)
        {
            m_UILabelText[i].text = value[i];
        }
        m_UISpriteIcon.spriteName = spriteName;
        
        ActiveIcon(activeIcon);
    }
    public void SetItemData(bool activeIcon, params string[] value)
    {
        for (int i = 0, count = m_UILabelText.Length; i < count; i++)
        {
            m_UILabelText[i].text = value[i];
        }
        ActiveIcon(activeIcon);
    }
    public void MakePixelPerfect()
    {
        m_UISpriteIcon.MakePixelPerfect();
    }

    void ActiveIcon(bool active)
    {
        for (int i = 0, count = m_UISprite.Length; i < count; i++)
        {
            m_UISprite[i].alpha = active ? 1 : 0;
        }
    }
}
