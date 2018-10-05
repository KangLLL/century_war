using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UINotification : MonoBehaviour {
    [SerializeField] int m_MessageInterval;
    [SerializeField] float m_MoveSpeed;
    [SerializeField] UISprite m_BackgroundBar;
    [SerializeField] UILabel m_UIlabel;
	// Use this for initialization
	void Start () {
        this.Notification();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            this.Notification();
	}
    void Notification()
    {
        this.gameObject.SetActive(false);
        return;
		/*
        List<string> mess = new List<string>() { "[ff0000]死亡骑士[-]  [4f5bff]凛风冲击：[-]  伤害提高了15%。",
                                                 "[ff0000]圣骑士[-]  [4f5bff]神圣：[-]  圣光普照的机制现在类似智能治疗，治疗范围内6个受伤最严重的目标，并且小守护者不再作为目标(野生小鬼、血虫、毒蛇陷阱的毒蛇等)。这个技能提供的总治疗量保持不变。",
                                                 "[ff0000]术士[-]  [4f5bff]痛苦：[-]  混乱之箭的伤害提高了15%。",
                                                 "[ff0000]团队副本、地下城及场景战役[-]  [4f5bff]团队副本[-]  修正了死亡或处于救赎之魂形态下的玩家无法获得成就“你说过交叉光是不好的”的问题。"   };
        string result = string.Empty;
        for (int i = 0; i < mess.Count; i++)
        {
            mess[i] = mess[i].PadRight(mess[i].Length + m_MessageInterval);
            result += mess[i];
        }

        m_UIlabel.text = result;
        float x = m_UIlabel.relativeSize.x * m_UIlabel.cachedTransform.localScale.x;
        m_UIlabel.transform.localPosition = new Vector3(m_BackgroundBar.transform.localPosition.x, 0, -1);
        iTween.Stop(m_UIlabel.gameObject);
        Vector3 to = new Vector3(m_BackgroundBar.transform.localPosition.x - x, 0, -1);
        iTween.MoveTo(m_UIlabel.gameObject, iTween.Hash(iT.MoveTo.position, to, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.speed, m_MoveSpeed, iT.MoveTo.islocal, true));
        */
    }
     
}
