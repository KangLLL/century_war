using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CommandConsts;
 
 
public class UIWindowLogin : UIWindowNewbieGuideBase
{
    [SerializeField] UILabel m_UILabel;//user input text;
    [SerializeField] Vector3 m_From;
    [SerializeField] Vector3 m_To;
    [SerializeField] float m_MoveTime = 0.2f;
	[SerializeField] GameObject m_ActivatorView;
    public NewbieGuide NewBieGuide { get; set; }
    void Awake()
    {
        this.GetTweenComponent();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void ShowWindow(Vector3? toPosition = null, bool enableScale = true)
    {
        base.ShowWindow(toPosition, enableScale);
    }

    public override void HideWindow(bool enableScale = true)
    {
        base.HideWindow(enableScale);
    }
    //Button message
    void OnCompleteInput()
    {
        string userInput = m_UILabel.text;

        //if (userInput.Length > 0 && userInput.Length <= ClientConfigConstants.Instance.UserNameLennth)
        if (SystemFunction.RegexUserName(userInput))
        {
            if (this.SensitiveWordFilter(userInput))
			{
                LogicController.Instance.ChangeName(userInput, this, "OnChangeName");
				LockScreen.Instance.DisableInput();
				this.m_ActivatorView.SetActive(true);
			}
            else
                UIErrorMessage.Instance.ErrorMessage(25);
        }
        else
            UIErrorMessage.Instance.ErrorMessage(18, ClientConfigConstants.Instance.UserNameLennth.ToString());
        
    }
    void OnChangeName(Hashtable hash)
    {
		LockScreen.Instance.EnableInput();
		this.m_ActivatorView.SetActive(false);
        ChangeNameResponseParameter response = new ChangeNameResponseParameter();
        response.InitialParameterObjectFromHashtable(hash);
        if (response.Result)
        {
            this.HideWindow(false);
            this.NewBieGuide.ResetAll();
            NewbieGuideManager.Instance.InvokeNextGuide();
        }
        else 
            UIErrorMessage.Instance.ErrorMessage(19);
    }
    void OnSubmit(string text)
    {
        m_UILabel.text = SystemFunction.ReplaceEmoji(text); 
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, this.m_From, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.time, this.m_MoveTime, iT.MoveTo.islocal, true));
    }
    //Button message
    void OnSelectOnTab()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, this.m_To, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.time, this.m_MoveTime, iT.MoveTo.islocal, true));
    }
    bool SensitiveWordFilter(string sensitiveWord)
    {
        foreach (KeyValuePair<int, string> k in StringConstants.SENSITIVE_WORD)
            if (sensitiveWord.Contains(k.Value))
                return false;
        return true;
    }
 
}
