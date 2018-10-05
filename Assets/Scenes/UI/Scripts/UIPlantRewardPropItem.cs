using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIPlantRewardPropItem : MonoBehaviour
{
    [SerializeField] UISprite m_UISprite;
    [SerializeField] UILabel m_UIlabel;


    public void SetItemData(PropsType propsType)
    {
        PropsConfigData propsConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsType);
        m_UIlabel.text = ClientSystemConstants.PROPS_QUALITY_COLOR[(PropsQuality)propsConfigData.Quality] + propsConfigData.Name;
        m_UISprite.spriteName = propsConfigData.PrefabName;
        m_UISprite.MakePixelPerfect();
    }
}
