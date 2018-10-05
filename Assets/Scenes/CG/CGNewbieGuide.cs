using UnityEngine;
using System.Collections;

public class CGNewbieGuide : MonoBehaviour 
{
	[SerializeField]
	private UIWindowGuide m_TipsDialog;
	[SerializeField]
	private Vector3 m_DialogPosition;
	[SerializeField]
	private UIAnchor.Side m_DialogSide;
	[SerializeField]
	private float m_MaskColorPercentage;
	[SerializeField]
	private GameObject m_SkipButton;
	
	private bool m_IsClicked;
	
	void OnClick()
	{
		if(!this.m_IsClicked)
		{
			CGDirector.Instance.StartCG();
			NewbieCommonHelper.ChangeAllSpritesColor(1/this.m_MaskColorPercentage,null);
			GameObject.Destroy(this.m_TipsDialog.transform.parent.gameObject);
			GameObject.Destroy(this.gameObject);
			this.m_SkipButton.SetActive(true);
			this.m_IsClicked = true;
		}
	}
	
	public void StartGuide()
	{
		this.gameObject.SetActive(true);
		NewbieCommonHelper.ChangeAllSpritesColor(this.m_MaskColorPercentage, null);
		this.m_TipsDialog.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[0.1f]);
		this.m_TipsDialog.ShowWindow(this.m_DialogSide,true,this.m_DialogPosition);	
	}
}
