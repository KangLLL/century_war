using UnityEngine;
using System.Collections;

public class UIDragPanelContentsEvent : MonoBehaviour {
    [SerializeField] UIWindowCommon m_UIWindowCommon;
    void OnDrag()
    {
        m_UIWindowCommon.HideWindow();
    }
}
