using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class T1 : MonoBehaviour 
{
	[Serializable]
	public class T4
	{
		[SerializeField]
		private int m_A;
		[SerializeField]
		private int m_B;
	}
	
	[SerializeField]
	private T4 m_T4;
	
	[SerializeField]
	private UISprite m_Sprite;
	[SerializeField]
	private T3 m_T3;
	
	[SerializeField]
	private GameObject clone;

	//private List<T4> m_TT;
	//private T4 m_Test;

	private Queue<int> m_QQ;
	
	//private int m_Start = 0;
	//private int m_Step = 10;
	
	void Start()
	{
		//Vector3 a = new Vector3(2,4,6);
		//Vector3 b = new Vector3(1,5,5);
		
		//Debug.Log(Vector3.Max(a,b));
		//this.m_Test = new T4();
		//this.m_TT = new List<T4>() { this.m_Test };
		this.m_QQ = new Queue<int>();
		this.m_QQ.Enqueue(1);
		this.m_QQ.Enqueue(1);
		this.m_QQ.Enqueue(1);
		this.m_QQ.Enqueue(1);
		this.m_QQ.Enqueue(1);
		/*
		Hashtable args = new Hashtable();
		args.Add("from",22);
		args.Add("to",88);
		args.Add("time",2);
		args.Add("onupdate","Flash");
		//args.Add("onupdatetarget", this.gameObject);
		iTween.ValueTo(this.gameObject,args);
		*/
	}
	
	void Flash(float v)
	{
		//this.m_Label.text = ((int)v).ToString();
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			//this.m_Sprite.transform.localScale *= 1.1f;
			//this.m_T3.RePosition();
			//Debug.Log(NGUIMath.CalculateRelativeWidgetBounds(this.m_Parent, this.m_Sprite.transform));
			//Debug.Log(NGUIMath.CalculateAbsoluteWidgetBounds(this.m_Sprite.transform));
			//Debug.Log(this.m_Sprite.relativeSize);
			
			/*
			Hashtable args = new Hashtable();
			//int current = int.Parse(this.m_Label.text);
			int previousGold = LogicController.Instance.PlayerData.CurrentStoreGold;
			args.Add("from",previousGold);
			//LogicController.Instance.Collect();
			int currentGold = LogicController.Instance.PlayerData.CurrentStoreGold;
			//this.m_Start += this.m_Step;
			args.Add("to",currentGold);
			args.Add("time",2);
			args.Add("onupdate","Flash");
			iTween.ValueTo(this.gameObject,args);
			*/
			
			//GameObject.Instantiate(this.clone);
	
			Debug.Log(this.m_QQ.Count);

			this.m_QQ.Dequeue();
		}
	}
	
	
	void OnClick()
	{
		Debug.Log("cccccccccccccc!");
	}
}
