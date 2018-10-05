using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundHelper
{
	private static HashSet<int> s_HashSet = new HashSet<int>();
	
	public static List<TilePosition> FillCircle(int xc, int yc, int r)
    {
		List<TilePosition> result = new List<TilePosition>();
		
		s_HashSet.Clear();
        for(int i = 0; i <= r; i++)
        {
            result.AddRange(DrawCircle(xc, yc, i));
        }
		
		return result;
    }

    public static List<TilePosition> DrawCircle(int xc, int yc, int r)
    {
		List<TilePosition> result = new List<TilePosition>();
		
        int x = 0;
        int y = r;
        int p = 3 - (r << 1);
        while (x < y)
        {
            result.AddRange(DrawCirclePoint(xc,yc,x,y));
            if (p <= 0)
            {
                p = p + (x << 2) + 6;
            }
            else 
            {
                if (p - (x << 1) - 1 > 0)
                {
                    result.AddRange(DrawCirclePoint(xc, yc, x, y - 1));
                }
                else
                {
                    result.AddRange(DrawCirclePoint(xc, yc, x+1, y));
                }
                p = p + ((x - y)<<2) + 10;
                y--;
            }
            x++;
        }
        if (x == y)
        {
            result.AddRange(DrawCirclePoint(xc, yc, x, y));
        }
		return result;
    }
	
	private static List<TilePosition> DrawCirclePoint(int xc, int yc, int x, int y)
    {
		List<TilePosition> result = new List<TilePosition>();
		
		int code;
        int xt;
        int yt;
        //1
        xt = xc + x;
        yt = yc + y;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
           	result.Add(new TilePosition(xt,yt));
        }

        //2
        xt = xc - x;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //3
        xt = xc + x;
        yt = yc - y;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //4
        xt = xc - x;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //5
        xt = xc + y;
        yt = yc + x;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //6
        xt = xc - y;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //7
        xt = xc + y;
        yt = yc - x;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }

        //8
        xt = xc - y;
        code = xt + (yt << 16);
        if (!s_HashSet.Contains(code))
        {
            s_HashSet.Add(code);
            result.Add(new TilePosition(xt,yt));
        }  
		
		return result;
    }   
}
