using UnityEngine;
using System.Collections;

public class UIOption : MonoBehaviour {
    [SerializeField] OptionType m_OptionType;
    [SerializeField] UICheckbox m_UICheckbox;
    //[SerializeField] bool m_Inverse = false;
    //bool mUsingDelegates = false;
    
    
	// Use this for initialization

    public void Start()
    {
        if (m_UICheckbox != null)
        {
            //mUsingDelegates = true;
            if (m_UICheckbox.onStateChange == null)
                m_UICheckbox.onStateChange += OnActivateDelegate;
        }
        
    }

    void OnActivateDelegate(bool isActive)
    {
        //if (enabled && target != null) target.enabled = inverse ? !isActive : isActive;
 
        switch (this.m_OptionType)
        {
            case OptionType.Music:
                //bool musicState = isActive;this.m_Inverse ? !isActive : isActive;
             
                AudioController.SetCategoryVolume("Music", isActive ? 1 : 0);
                //int ms = musicState ? 1 : 0;
                PlayerPrefs.SetInt("Music", isActive ? 1 : 0);
                break;
            case OptionType.SoundFX:
                //bool soundFXstate = isActive; this.m_Inverse ? !isActive : isActive;
          
                AudioController.SetCategoryVolume("SFX", isActive ? 1 : 0);
                //int sf = soundFXstate ? 1 : 0;
                PlayerPrefs.SetInt("SFX", isActive ? 1 : 0); 
                break;
        }
    }

    //void OnActivate(bool isActive) {if (!mUsingDelegates) OnActivateDelegate(isActive); }

}
public enum OptionType
{
    Music,
    SoundFX
}
