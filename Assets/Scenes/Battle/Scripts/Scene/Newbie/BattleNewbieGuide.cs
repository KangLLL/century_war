using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleNewbieGuide : MonoBehaviour 
{
	[SerializeField]
	private float m_MaskColorPercentage;
	[SerializeField]
	private GameObject m_TipsDialogPrefab;
	
	[SerializeField]
	private GameObject m_NewbieFinger;
	[SerializeField]
	private GameObject m_InstructArrowPrefab;
	[SerializeField]
	private GameObject m_TrophyArrowPrefab;
	[SerializeField]
	private GameObject m_SummaryArrowPrefab;
	
	[SerializeField]
	private Vector3 m_StepStartPosition;
	[SerializeField]
	private UIAnchor.Side m_StepStartSide;
	[SerializeField]
	private Vector3 m_StepTrophyPosition;
	[SerializeField]
	private UIAnchor.Side m_StepTrophySide;
	[SerializeField]
	private Vector3 m_StepFortressPosition;
	[SerializeField]
	private UIAnchor.Side m_StepFortressSide;
	
	
	[SerializeField]
	private List<string> m_TrohpyUIName;
	
	private List<UILabel> m_TrophyLabel;
	private List<UISprite> m_TrophyUI;
	
	[SerializeField]
	private Vector3 m_TrophyArrowOffset;
	[SerializeField]
	private Vector3 m_StepFortressArrowOffset;
	[SerializeField]
	private Vector3 m_SummaryArrowOffset;
	
	private bool m_IsGuide;
	private bool m_IsMasked;
	//private float m_GuideStartSecond;
	private UIWindowGuide m_CurrentTipsDialog;
	
	private int m_CurrentGuideStep;
	
	private Transform m_CachedUIRoot;
	private GameObject m_InstructArrow;
	private GameObject m_SummaryArrow;
	private GameObject m_TrophyArrow;
	
	private List<tk2dBaseSprite> m_HighlightSprites;
	private List<UISprite> m_HighlightUI;
	private List<UILabel> m_HighlightLabel;
	
	void Start () 
	{
		TimeTickRecorder.Instance.PauseTimeTick();
		this.m_HighlightSprites = new List<tk2dBaseSprite>();
		this.m_HighlightUI = new List<UISprite>();
		this.m_HighlightLabel = new List<UILabel>();
		
		this.m_TrophyUI = new List<UISprite>();
		this.m_TrophyLabel = new List<UILabel>();
		
		GameObject next = GameObject.Find(ClientStringConstants.UI_NEXT_RIVAL_OBJECT_NAME);
		next.SetActive(false);
		
		GameObject cloud = GameObject.Find(ClientStringConstants.CLOUD_OBJECT_NAME);
		this.m_HighlightUI.AddRange(cloud.GetComponentsInChildren<UISprite>());
		
		NewbieCommonHelper.ChangeAllSpritesColor(this.m_MaskColorPercentage, null);
		NewbieCommonHelper.ChangeAllUIColor(this.m_MaskColorPercentage, this.m_HighlightUI);
		NewbieCommonHelper.ChangeAllLabelColor(this.m_MaskColorPercentage, null);
		
		//GameObject rootObject = GameObject.Find(ClientStringConstants.UI_ROOT_OBJECT_NAME);
		//this.m_CachedUIRoot = rootObject.transform.GetChild(0).FindChild(ClientStringConstants.UI_ANCHOR_OBJECT_NAME);
		
		foreach(string name in this.m_TrohpyUIName)
		{
			GameObject go = GameObject.Find(name) as GameObject;
			UILabel label = go.GetComponent<UILabel>();
			if(label != null)
			{
				this.m_TrophyLabel.Add(label);
			}
			this.m_TrophyLabel.AddRange(go.GetComponentsInChildren<UILabel>());
			this.m_TrophyUI.AddRange(go.GetComponentsInChildren<UISprite>());
		}
	}
	
	void Destory()
	{
		this.m_HighlightSprites = null;
		this.m_HighlightUI = null;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(!this.m_IsGuide)
		{
			if(BattleSceneHelper.Instance.GetAllBuildings().Count > 0 && !this.m_IsMasked)
			{
				NewbieCommonHelper.ChangeAllSpritesColor(this.m_MaskColorPercentage, null);
				this.m_IsMasked = true;
			}
			if(LockScreen.Instance.Inputable)
			{
				//this.m_GuideStartSecond = Time.timeSinceLevelLoad;
				this.StartCoroutine("Guide");
				this.m_IsGuide = true;
			}
		}
		
		if(BattleDirector.Instance.IsReceivedReplayID && this.m_SummaryArrow == null)
		{
			ButtonReturn button = GameObject.FindObjectOfType(typeof(ButtonReturn)) as ButtonReturn;
			if(button != null)
			{
				this.m_SummaryArrow = GameObject.Instantiate(this.m_SummaryArrowPrefab) as GameObject;
				this.m_SummaryArrow.transform.position = button.transform.position + this.m_SummaryArrowOffset;
			}		
		}
	}
	
	public void OnClick()
	{
		if(this.m_CurrentGuideStep < 3)
		{
			this.m_CurrentGuideStep ++;
		}
	}
	
	IEnumerator Guide()
	{
		GameObject dialog = GameObject.Instantiate(this.m_TipsDialogPrefab) as GameObject;

		dialog.transform.GetChild(0).gameObject.SetActive(true);
		Collider collider = dialog.GetComponentInChildren<Collider>();
		GameObject.Destroy(collider);
		dialog.transform.position = GameObject.Find(ClientStringConstants.UI_ROOT_OBJECT_NAME).transform.position;
		this.m_CurrentTipsDialog = dialog.GetComponentInChildren<UIWindowGuide>();	
		this.m_CurrentTipsDialog.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.4f]);
		this.m_CurrentTipsDialog.ShowWindow(this.m_StepStartSide,true,this.m_StepStartPosition);
		
		while(this.m_CurrentGuideStep < 1)
		{
			yield return null;
		}
		
		this.m_CurrentTipsDialog.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.41f]);
		this.m_CurrentTipsDialog.ShowWindow(this.m_StepTrophySide, true, this.m_StepTrophyPosition);
		
		this.m_TrophyArrow = GameObject.Instantiate(this.m_TrophyArrowPrefab) as GameObject;
		this.m_TrophyArrow.transform.position = this.m_TrophyUI[0].transform.position + this.m_TrophyArrowOffset;
		foreach (UISprite sprite in this.m_TrophyUI) 
		{	
			this.m_HighlightUI.Add(sprite);
			NewbieCommonHelper.ChangeUIColor(sprite, 1 / this.m_MaskColorPercentage);
		}
		foreach (UILabel label in this.m_TrophyLabel) 
		{
			this.m_HighlightLabel.Add(label);
			NewbieCommonHelper.ChangeLabelColor(label, 1 / this.m_MaskColorPercentage);
		}
		
		while(this.m_CurrentGuideStep < 2)
		{
			yield return null;
		}
		
		GameObject.Destroy(this.m_TrophyArrow);
		
		this.m_CurrentTipsDialog.SetWindowItem(StringConstants.NEWBIEGUIDE_CONTEXT[9.5f]);
		this.m_CurrentTipsDialog.ShowWindow(this.m_StepFortressSide, true, this.m_StepFortressPosition);
		BuildingAI building = (BuildingAI)GameObject.FindObjectOfType(typeof(BuildingAI)); 
		tk2dBaseSprite[] sprites = building.GetComponentsInChildren<tk2dBaseSprite>();
		this.m_InstructArrow = GameObject.Instantiate(this.m_InstructArrowPrefab) as GameObject;
		this.m_InstructArrow.transform.position = building.transform.position + this.m_StepFortressArrowOffset;
		foreach (tk2dBaseSprite sprite in sprites) 
		{
			this.m_HighlightSprites.Add(sprite);
			NewbieCommonHelper.ChangeSpriteColor(sprite, 1 / this.m_MaskColorPercentage);
		}
		
		while(this.m_CurrentGuideStep < 3)
		{
			yield return null;
		}
		
		GameObject.Destroy(this.m_InstructArrow);
		this.m_CurrentTipsDialog.HideWindow(true);
		GameObject.Destroy(this.m_CurrentTipsDialog.gameObject);
		NewbieCommonHelper.ChangeAllUIColor(1 / this.m_MaskColorPercentage, this.m_HighlightUI);
		NewbieCommonHelper.ChangeAllSpritesColor(1 / this.m_MaskColorPercentage, this.m_HighlightSprites);
		NewbieCommonHelper.ChangeAllLabelColor(1 / this.m_MaskColorPercentage, this.m_HighlightLabel);
		
		this.m_HighlightSprites.Clear();
		this.m_HighlightUI.Clear();
		
		this.GetComponent<Collider>().enabled = false;
		GameObject.Instantiate(this.m_NewbieFinger);
	}
	
	#region Utility Methods
	
	private void ResetHighlight()
	{
		foreach(UISprite sprite in this.m_HighlightUI)
		{
			NewbieCommonHelper.ChangeUIColor(sprite, 1 / this.m_MaskColorPercentage);
		}
		this.m_HighlightUI.Clear();
		foreach(tk2dBaseSprite sprite in this.m_HighlightSprites)
		{
			NewbieCommonHelper.ChangeSpriteColor(sprite, 1 / this.m_MaskColorPercentage);
		}
		this.m_HighlightSprites.Clear();
		foreach(UILabel label in this.m_HighlightLabel)
		{
			NewbieCommonHelper.ChangeLabelColor(label, 1 / this.m_MaskColorPercentage);
		}
		this.m_HighlightLabel.Clear();
	}
	
	#endregion
}
