using UnityEngine;
using System.Collections;

public class NewbieFingerBehavior : MonoBehaviour 
{
	[SerializeField]
	private Vector3[] m_ClickPosition;
	[SerializeField]
	private GameObject m_ClickEffect;
	[SerializeField]
	private float m_MoveTime;
	
	private GameObject m_PreviousEffect;
	
	private int m_NextIndex;
	
	// Use this for initialization
	void Start () 
	{
		this.transform.position = this.m_ClickPosition[0];
		this.GenerateClickEffect();
		this.MoveToNext();
	}
	
	public void oncomplete()
	{
		if(this.m_PreviousEffect != null)
		{
			GameObject.Destroy(this.m_PreviousEffect);
		}
		this.GenerateClickEffect();
		this.MoveToNext();
	}
	
	private void MoveToNext()
	{
		this.m_NextIndex = (this.m_NextIndex + 1) % this.m_ClickPosition.Length;
		
		iTween.MoveTo(this.gameObject, iTween.Hash(
			iT.MoveTo.x, this.m_ClickPosition[this.m_NextIndex].x,
			iT.MoveTo.y, this.m_ClickPosition[this.m_NextIndex].y,
			iT.MoveTo.z, this.m_ClickPosition[this.m_NextIndex].z,
			iT.MoveTo.time, this.m_MoveTime,
			iT.MoveTo.oncompletetarget, this.gameObject, 
			iT.MoveTo.oncomplete, "oncomplete"));
	}
	
	private void GenerateClickEffect()
	{
		GameObject click = GameObject.Instantiate(this.m_ClickEffect) as GameObject;
		Vector3 localPosition = click.transform.position;
		click.transform.parent = this.transform;
		click.transform.localPosition = localPosition;
		click.transform.parent = null;
		this.m_PreviousEffect = click;
	}
	
	void FixedUpdate()
	{
		if(BattleDirector.Instance.IsBattleStart)
		{
			TimeTickRecorder.Instance.ResumeTimeTick();
			GameObject.Destroy(this.gameObject);
			if(this.m_PreviousEffect != null)
			{
				GameObject.Destroy(this.m_PreviousEffect);
			}
		}
	}
}
