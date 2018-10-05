using UnityEngine;
using System.Collections;
using CommandConsts;

public class T2 : MonoBehaviour 
{
	void OnClick()
	{
		CollectRequestParameter request = new CollectRequestParameter();
		request.BuildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
		request.BuildingNO = 0;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		request.Quantity = 10000000;
		CommunicationUtility.Instance.CollectGold(request);


		Debug.Log("123");
		/*object[] aa = GameObject.FindObjectsOfType(typeof(tk2dBaseSprite));
		
		foreach (object a in aa) {
			tk2dBaseSprite p = (tk2dBaseSprite)a;
			p.color = Color.red;
		}
		
		Debug.Log(UICamera.currentTouchID);
		
		
		tk2dBaseSprite[] aa = RegisteredComponentController.GetAllOfType<tk2dBaseSprite>();
		
		Debug.Log(aa.Length);
		foreach (tk2dBaseSprite a in aa) 
		{
			a.color = Color.red;
		}
		
		BattleData.IsNewbie = true;
		*/
		//Debug.Log("logout!");
		//Bonjour.LoginOut();
	}
	
	void Awake()
	{
		Debug.Log("Awake");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			CollectRequestParameter request = new CollectRequestParameter();
			request.BuildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
			request.BuildingNO = 0;
			request.OperateTick = LogicTimer.Instance.GetServerTick();
			request.Quantity = 10000000;
			CommunicationUtility.Instance.CollectGold(request);
			CommunicationUtility.Instance.CollectGold(request);
		}
	}
}
