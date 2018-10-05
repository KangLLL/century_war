using UnityEngine;
using System.Collections;

public class AttackRangeFX : MonoBehaviour {
    [SerializeField] tk2dSprite m_tk2dSprite;
    [SerializeField] TweenColortk2dSprite m_TweenColortk2dSprite;
    [SerializeField] tk2dSprite m_tk2dSpriteBlindArea;
    [SerializeField] TweenColortk2dSprite m_TweenColortk2dSpriteBlindArea;
    //picture pixel
    const int SOURCE_DIAMETER = 512;
    const int SOURCE_BLINDAREA_DIAMETER = 256;
    Vector3 m_From = new Vector3(0.01f, 0.01f, 1f);


    public void ShowAttackRange(float radius,float blindAreaRadius)
    {
        iTween.Stop(m_tk2dSprite.gameObject);
        iTween.Stop(m_tk2dSpriteBlindArea.gameObject);

        this.m_tk2dSprite.color = Color.white;
        this.m_tk2dSpriteBlindArea.color = Color.white;
        m_tk2dSprite.transform.localScale = this.m_From;
        m_tk2dSpriteBlindArea.transform.localScale = this.m_From;
        iTween.ScaleTo(m_tk2dSprite.gameObject, iTween.Hash(iT.ScaleTo.scale, this.ConvertToScale(radius, SOURCE_DIAMETER), iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f));
        iTween.ScaleTo(m_tk2dSpriteBlindArea.gameObject, iTween.Hash(iT.ScaleTo.scale, this.ConvertToScale(blindAreaRadius, SOURCE_BLINDAREA_DIAMETER), iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f));
        this.m_TweenColortk2dSprite.Play(true);
        this.m_TweenColortk2dSpriteBlindArea.Play(true);
    }
    public void HideAttackRange(float radius, float blindAreaRadius)
    {
        iTween.Stop(m_tk2dSprite.gameObject);
        iTween.Stop(m_tk2dSpriteBlindArea.gameObject);

        iTween.ScaleTo(m_tk2dSprite.gameObject, iTween.Hash(iT.ScaleTo.scale, m_From, iT.ScaleTo.easetype, iTween.EaseType.linear, iT.ScaleTo.time, 0.2f, iT.ScaleTo.oncomplete, "OnCompleteScale"));
        iTween.ScaleTo(m_tk2dSpriteBlindArea.gameObject, iTween.Hash(iT.ScaleTo.scale, m_From, iT.ScaleTo.easetype, iTween.EaseType.linear, iT.ScaleTo.time, 0.2f));
        this.m_TweenColortk2dSprite.Play(false);
        this.m_TweenColortk2dSpriteBlindArea.Play(false);
    }
    Vector3 ConvertToScale(float radius,int pixel)
    {
        float scale = radius * 2 / pixel;
        return new Vector3(scale, scale, 1);
    }
    void OnCompleteScale()
    {
        this.m_tk2dSprite.color = Color.clear;
        this.m_tk2dSpriteBlindArea.color = Color.clear;
    }

}
