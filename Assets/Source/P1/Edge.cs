using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : IComparable<Edge>
{
    public int VertexA;
    public int VertexB;
    public int VertexOther;

    public Edge (int a, int b, int c)
    {
        if (a < b)
        {
            this.VertexA = a;
            this.VertexB = b;
            this.VertexOther = c;
        }
        else
        {
            this.VertexA = b;
            this.VertexB = a;
            this.VertexOther = c;
        }
    }
    
    public bool compare(Edge e)
    {
        if (this.VertexA == e.VertexA && this.VertexB == e.VertexB)
        {
            return true;
        }
        return false;
    }

    public int CompareTo(Edge e)
    {
        if ( e == null)
        {
            return 1;
        }
        else if(this.VertexA == e.VertexA)
        {
            return this.VertexB.CompareTo(e.VertexB);
        }
        else
        {
            return this.VertexA.CompareTo(e.VertexA);
        }
        
    }
}
