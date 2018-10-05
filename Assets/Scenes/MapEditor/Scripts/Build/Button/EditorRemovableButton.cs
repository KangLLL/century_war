using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorRemovableButton : MonoBehaviour 
{
	void OnClick()
	{
		EditorFactory.Instance.ConstructRemovableObject(RemovableObjectType.SmallStone);
	}
}
