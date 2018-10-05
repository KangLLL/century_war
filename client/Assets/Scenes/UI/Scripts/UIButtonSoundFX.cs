using UnityEngine;
using System.Collections;

public class UIButtonSoundFX : MonoBehaviour {
    [SerializeField] string m_SoundFxName = "ButtonClick";
    [SerializeField] Trigger trigger = Trigger.OnClick;
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
    } 

    void OnHover(bool isOver)
    {
        if (enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
        { 
            AudioController.Play(this.m_SoundFxName);
        }
    }

    void OnPress(bool isPressed)
    {
        if (enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
        { 
            AudioController.Play(this.m_SoundFxName);
        }
    }

    void OnClick()
    {
        if (enabled && trigger == Trigger.OnClick)
        {
            AudioController.Play(this.m_SoundFxName);
        }
    }
}
