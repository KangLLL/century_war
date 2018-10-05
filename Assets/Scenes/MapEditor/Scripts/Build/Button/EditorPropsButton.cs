using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorPropsButton : MonoBehaviour 
{
	void OnClick()
	{
		EditorFactory.Instance.ConstructDefenseObject(PropsType.HoneycombA);
	}
}
