using UnityEngine;
using System.Collections;

public interface IPickableObject
{
    //void SetArrowPosition(bool initialPosition);
    void OnUnSelect(bool isCancel);
    void ShowBuildingTitle(bool isShow);
    bool IsClick { get; set; }
    bool IsBuild { get; }
    void OnClick();
}
