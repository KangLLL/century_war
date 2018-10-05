using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool CheckBuildPlace(TilePosition startTilePosition);
public class AdjacencyList<T>
{
    CheckBuildPlace m_CheckBuildPlace;
    List<Vertex<T>> items; 
    public AdjacencyList() : this(10) { } 
    public AdjacencyList(int capacity) 
    {
        items = new List<Vertex<T>>(capacity);
    }
    public void AddVertex(T item)
    {
        if (Contains(item))
        {
            throw new ArgumentException("插入了重复顶点！");
        }
        items.Add(new Vertex<T>(item));
    }
    public void AddEdge(T from, T to) 
    {
        Vertex<T> fromVer = Find(from);
        if (fromVer == null)
        {
            throw new ArgumentException("头顶点并不存在！");
        }
        Vertex<T> toVer = Find(to);
        if (toVer == null)
        {
            throw new ArgumentException("尾顶点并不存在！");
        }
        AddDirectedEdge(fromVer, toVer);
        AddDirectedEdge(toVer, fromVer);
    }
    public bool Contains(T item)
    {
        foreach (Vertex<T> v in items)
        {
            if (v.data.Equals(item))
            {
                return true;
            }
        }
        return false;
    }
    private Vertex<T> Find(T item)
    {
        foreach (Vertex<T> v in items)
        {
            if (v.data.Equals(item))
            {
                return v;
            }
        }
        return null;
    }

    private void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
    {
        if (fromVer.firstEdge == null)
        {
            fromVer.firstEdge = new Node(toVer);
        }
        else
        {
            Node tmp, node = fromVer.firstEdge;
            do
            {
                if (node.adjvex.data.Equals(toVer.data))
                {
                    throw new ArgumentException("添加了重复的边！");
                }
                tmp = node;
                node = node.next;
            } while (node != null);
            tmp.next = new Node(toVer);
        }
    }
 
    public class Node
    {
        public Vertex<T> adjvex; 
        public Node next;
        public Node(Vertex<T> value)
        {
            adjvex = value;
        }
    }

    public class Vertex<TValue>
    {
        public TValue data; 
        public Node firstEdge;
        public Boolean visited;
        public Vertex(TValue value) 
        {
            data = value;
        }
    }
    #region
    public void BFSTraverse(int index, CheckBuildPlace checkBuildPlace)
    {
        this.m_CheckBuildPlace = checkBuildPlace;
        InitVisited();
        BFS(items[index]);
    }
    public void BFSTraverse2()
    {
        InitVisited();
        foreach (Vertex<T> v in items)
        {
            if (!v.visited)
            {
                BFS(v);
            }
        }
    }
    private void BFS(Vertex<T> v) 
    {
        Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
        Debug.Log("Start search: Row="+(v.data as TilePosition).Row +"  Col=" +(v.data as TilePosition).Column + " ----->");
        v.visited = true; 
        queue.Enqueue(v);
        while (queue.Count > 0)
        {
            Vertex<T> w = queue.Dequeue();
            Node node = w.firstEdge;
            while (node != null)
            {
                if (!node.adjvex.visited)
                {
                    //Debug.Log("Row =" + (node.adjvex.data as TilePosition).Row + "-->> " + "Col = " + (node.adjvex.data as TilePosition).Column + "-->> ");

                    node.adjvex.visited = true;
                    queue.Enqueue(node.adjvex);
                    TilePosition TilePosition = node.adjvex.data as TilePosition;
                    if (this.m_CheckBuildPlace(TilePosition))
                    {
                        return;
                    }
                }
                node = node.next; 
            }
        } 
    }
    private void InitVisited()
    {
        foreach (Vertex<T> v in items)
        {
            v.visited = false; //全部置为false
        }
    }

    #endregion
}