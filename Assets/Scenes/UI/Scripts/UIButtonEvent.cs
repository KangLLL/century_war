using UnityEngine;
using System.Collections;
using System;

public class UIButtonEvent : MonoBehaviour {
    void OnCompleteMoveTo(System.Object param)
    {
        UIManager.Instance.OnCompleteMoveTo(param);
    }
}
