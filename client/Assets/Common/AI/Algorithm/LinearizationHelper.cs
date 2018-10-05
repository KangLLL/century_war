using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearizationHelper 
{
    public static List<TilePosition> BresenhamLine(TilePosition startIndex, TilePosition endIndex)
    {
		int x1 = startIndex.Column;
		int y1 = startIndex.Row;
		int x2 = endIndex.Column;
		int y2 = endIndex.Row;
		
        int dx, dy, p, const1, const2, x, y, inc;
        int steep = (Mathf.Abs(y2 - y1) > Mathf.Abs(x2 - x1)) ? 1 : 0;

        if (steep == 1)
        {
            int temp = x1;
            x1 = y1;
            y1 = temp;

            temp = x2;
            x2 = y2;
            y2 = temp;
        }

        if (x1 > x2)
        {
            int temp = x1;
            x1 = x2;
            x2 = temp;

            temp = y1;
            y1 = y2;
            y2 = temp;
        }

        dx = Mathf.Abs(x2 - x1);
        dy = Mathf.Abs(y2 - y1);
        p = 2 * dy - dx;

        const1 = 2 * dy;

        const2 = 2 * (dy - dx);

        x = x1;
        y = y1;
        inc = (y1 < y2) ? 1 : -1;
		
		List<TilePosition> result = new List<TilePosition>();
        while (x <= x2)
        {
            if (steep == 1)
                result.Add(new TilePosition(y, x));
            else
                result.Add(new TilePosition(x, y));
            x++;

            if (p < 0)
                p += const1;
            else
            {
                p += const2;
                if (x <= x2)
                {
					/*
                    if (steep == 1)
                    {
                        result.Add(new TileIndex(y, x));
                    }
                    else
                    {
                        result.Add(new TileIndex(x, y));
                    }
                    */
				}
                y += inc;
            }
        }
		return result;
    }	
}
