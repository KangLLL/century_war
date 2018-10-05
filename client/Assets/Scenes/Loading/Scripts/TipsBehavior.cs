using UnityEngine;
using System.Collections;

public class TipsBehavior : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_TipsValue;

	void Start () 
	{
		string tipsValue = ClientConfigConstants.Instance.TipsInfos[Random.Range(0,ClientConfigConstants.Instance.TipsInfos.Length)];
		this.m_TipsValue.text =  ClientStringConstants.TIPS_TITLE + tipsValue;
	}
}
