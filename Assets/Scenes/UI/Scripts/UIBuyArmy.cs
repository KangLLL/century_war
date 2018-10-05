using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
public class UIBuyArmy : MonoBehaviour {
    [SerializeField] ArmyType m_ArmyType;
    [SerializeField] UIWindowBuyArmy m_UIWindowBuyArmy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnClick()
    {
        if (!GetComponent<UIArmyItemInfomation>().IsLock )
            return;
        m_UIWindowBuyArmy.BuyArmy(m_ArmyType);

    }
    void ShowArmyInformationWindow()
    {
        m_UIWindowBuyArmy.HideWindow();
        UIManager.Instance.UIWindowArmyInformation.ArmyType = this.m_ArmyType;
        UIManager.Instance.UIWindowArmyInformation.ShowWindow();
    }
}
