using UnityEngine;
using System.Collections;

public class UISelectMenu : MonoBehaviour {
    [SerializeField]
    UIWindowCommon m_Parent;
    [SerializeField]
    UIMenuType m_UIMenuType; 
    int m_SpriteDepth = 100;
 
 
    public void OnClick()
    {
        if (!this.enabled)
            return;
        if (m_Parent.ControlerFocus != null)
            return;
        else
            m_Parent.ControlerFocus = this.gameObject;
        
        m_Parent.HideWindow();
		UIManager.Instance.UIWindowBuyBuilding.ShowWindows(m_UIMenuType);
    }
    void OnPress(bool isPress)
    { 
        if (isPress)
        { 
            UISprite[] uiSprites = this.gameObject.GetComponentsInChildren<UISprite>();
            foreach (UISprite uiSprite in uiSprites)
                uiSprite.depth += this.m_SpriteDepth;
            UILabel[] uiLabels = this.gameObject.GetComponentsInChildren<UILabel>();
            foreach (UILabel uiLabel in uiLabels)
                uiLabel.depth += this.m_SpriteDepth;
        }
        else
        {
            UISprite[] uiSprites = this.gameObject.GetComponentsInChildren<UISprite>();
            foreach (UISprite uiSprite in uiSprites)
                uiSprite.depth += this.m_SpriteDepth;
            UILabel[] uiLabels = this.gameObject.GetComponentsInChildren<UILabel>();
            foreach (UILabel uiLabel in uiLabels)
                uiLabel.depth += this.m_SpriteDepth;
        }
    }
}
