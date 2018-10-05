
using UnityEngine;
using System.Collections;

 
public class AsNode
{
    public static int NULL = 0;
    public static int ASNL_ADDOPEN = 0;
    public static int ASNL_STARTOPEN = 1;
    public static int ASNL_DELETEOPEN = 2;
    public static int ASNL_ADDCLOSED = 3;
    public static int ASNC_INITIALADD = 0;
    public static int ASNC_OPENADD_UP = 1;
    public static int ASNC_OPENADD = 2;
    public static int ASNC_CLOSEDADD_UP = 3;
    public static int ASNC_CLOSEDADD = 4;
    public static int ASNC_NEWADD = 5;
    public AsNode(int a = -1, int b = -1)// : x(a), y(b), number(0), numchildren(0)
    {
        x = a;
        y = b;
        number = 0;
        numchildren = 0;
        parent = next = null;
        //dataptr = null;
    }
    public int f, g, h;
    public int x, y;
    public int numchildren;
    public int number;
    public AsNode parent;
    public AsNode next;
    public AsNode []children = new AsNode[8];
}
public class AsStack
{
    public AsNode data;
    public AsStack next;
}
public delegate int AsFunc(AsNode  previous, AsNode addnode, int data/*, void *pThis*/);
