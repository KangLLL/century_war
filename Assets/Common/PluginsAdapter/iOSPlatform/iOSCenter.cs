using UnityEngine;
using System.Collections;

public class iOSCenter : MonoBehaviour 
{
	private static iOSCenter s_Sigleton;
	
	public static iOSCenter Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	public long AccountID { get;set; }
	
	// Use this for initialization
	void Start () 
	{
		CommonHelper.PlatformType = ConfigUtilities.Enums.PlatformType.iOS;
		this.AccountID = -1;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
