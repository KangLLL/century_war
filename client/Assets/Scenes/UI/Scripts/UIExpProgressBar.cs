using UnityEngine;
using System.Collections;

public class UIExpProgressBar : UIProgressCommon
{
    [SerializeField] UISprite m_UISpriteIcon;
    [SerializeField] Vector3 m_ScaleFrom;
    const float DURATION_PROGRESS = 1.5f;
    const float DURATION_SCALE = 0.2f;
   
    //float m_ValueUpdate;
    float m_ProgressUpdate;
    float m_LastValue;
    float m_LastProgressValue;
    public new void SetProgressBar(float progress, float value)
    {

        if ((int)m_LastValue != ((int)value) || (int)(m_LastProgressValue * 1000) != (int)(progress * 1000))
        {
            iTween.Stop(this.gameObject);
            iTween.ValueTo(this.gameObject, iTween.Hash(iT.ValueTo.from, this.m_LastValue, iT.ValueTo.to, value, iT.ValueTo.time, DURATION_PROGRESS, iT.ValueTo.onupdate, "OnUpdateValue", iT.ValueTo.oncomplete, "OnCompleteValue"));
            iTween.ValueTo(this.gameObject, iTween.Hash(iT.ValueTo.from, this.m_LastProgressValue, iT.ValueTo.to, progress, iT.ValueTo.time, DURATION_PROGRESS, iT.ValueTo.onupdate, "OnUpdateProgress"));

            if (m_UISpriteIcon != null)
            {
                Vector3 scaleTo = m_ScaleFrom;
                scaleTo.z = 1;
                iTween.ValueTo(this.gameObject, iTween.Hash(iT.ValueTo.from, m_ScaleFrom, iT.ValueTo.to, scaleTo * 1.2f, iT.ValueTo.time, DURATION_SCALE, iT.ValueTo.looptype, iTween.LoopType.pingPong, iT.ValueTo.onupdate, "OnUpdateScale"));
            }
            this.m_LastValue = value;
            this.m_LastProgressValue = progress;
        }
        if ((int)value == 0)
        {
            base.SetProgressBar(0, 0);
        }
    }
    void OnUpdateValue(float value)
    {
        base.SetProgressBar(this.m_ProgressUpdate, value); 
    }
    void OnUpdateProgress(float progress)
    { 
        this.m_ProgressUpdate = progress;
    }
    void OnUpdateScale(Vector3 scale)
    {
        m_UISpriteIcon.transform.localScale = scale; 
    }
    void OnCompleteValue()
    {
        iTween.Stop(this.gameObject);
        if (m_UISpriteIcon != null)
        {
            m_UISpriteIcon.transform.localScale = m_ScaleFrom;
        }
        base.SetProgressBar(this.m_LastProgressValue, this.m_LastValue);
    }
    //public void SetProgressBar(float progress, string value)
    //{
    //    base.SetProgressBar(progress, value);
    //}
    //public  void SetText(params string[] text)
    //{
    //    base.SetText(text);
    //}
}
