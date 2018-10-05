using UnityEngine;
using System.Collections;

public class LoadSoundSet : MonoBehaviour {
    [SerializeField] UIOption[] m_UIOptions;//0 = music on; 1 = sfx                      //0 = music on;1 = music off; 2 = sfx on; 3 = sfx off;
    [SerializeField] UICheckbox[] m_UICheckboxs;//0 = music on; 1 = sfx                   //0 = music on;1 = music off; 2 = sfx on; 3 = sfx off;
	// Use this for initialization
	void Start()
    {
        this.OnLoadSoundSet();
	}
    void OnLoadSoundSet()
    {
        foreach (UIOption uiOption in m_UIOptions)
            uiOption.Start();

        bool musicState = PlayerPrefs.GetInt("Music", 1) == 1;
        m_UICheckboxs[0].startsChecked = musicState;
        m_UICheckboxs[0].Awake();
        m_UICheckboxs[0].Start();
        //m_UICheckboxs[0].isChecked = musicState;

        bool SfxState = PlayerPrefs.GetInt("SFX", 1) == 1;
        m_UICheckboxs[1].startsChecked = SfxState;
        m_UICheckboxs[1].Awake();
        m_UICheckboxs[1].Start();
        //m_UICheckboxs[1].isChecked = SfxState;
 
    }
}
