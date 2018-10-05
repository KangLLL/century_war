using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIGridExt : UIGrid {

	// Use this for initialization
	void Start () {
        this.Reposition();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public new void Reposition()
    {
        //if (!mStarted)
        //{
        //    repositionNow = true;
        //    return;
        //}
        Transform myTrans = transform;

        int x = 0;
        int y = 0;

        if (sorted)
        {
            List<Transform> list = new List<Transform>();

            for (int i = 0; i < myTrans.childCount; ++i)
            {
                Transform t = myTrans.GetChild(i);
                if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
            }
            list.Sort(SortByName);
            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                Transform t = list[i];

                if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;
                float depth = t.localPosition.z;
                t.localPosition = (arrangement == Arrangement.Horizontal) ?
                    new Vector3(cellWidth * x, -cellHeight * y, depth) :
                    new Vector3(cellWidth * y, -cellHeight * x, depth);

                if (++x >= maxPerLine && maxPerLine > 0)
                {
                    x = 0;
                    ++y;
                }
            }
        }
        else
        {
            for (int i = 0; i < myTrans.childCount; ++i)
            {
                Transform t = myTrans.GetChild(i);
                if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

                float depth = t.localPosition.z;
                t.localPosition = (arrangement == Arrangement.Horizontal) ?
                    new Vector3(cellWidth * -x, cellHeight * y, depth) :
                    new Vector3(cellWidth * -y, cellHeight * x, depth);

                if (++x >= maxPerLine && maxPerLine > 0)
                {
                    x = 0;
                    ++y;
                }
            }
        }

        UIDraggablePanel drag = NGUITools.FindInParents<UIDraggablePanel>(gameObject);
        if (drag != null) drag.UpdateScrollbars(true);
    }
}
