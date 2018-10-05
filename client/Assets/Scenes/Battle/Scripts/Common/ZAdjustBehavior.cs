using UnityEngine;
using System.Collections;

public class ZAdjustBehavior : MonoBehaviour 
{
	[SerializeField]
	private int height;
	
	void Update () 
	{
		if(this.transform.position.z != this.transform.position.y - height)
		{
			this.transform.position = new Vector3(this.transform.position.x, 
				this.transform.position.y, this.transform.position.y - height);
		}
	}
}
