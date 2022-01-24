using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Node vertexA;
    public Node vertexB;
    public Node vertexC;

    public float area;
    public float friction;
    public Vector3 vel;
    public Vector3 wind;

    public Triangle(Node nodeA, Node nodeB, Node nodeC, float friction, Vector3 wind)
    {
        this.vertexA = nodeA;
        this.vertexB = nodeB;
        this.vertexC = nodeC;
        this.area = calculateArea();
        this.friction = friction;
        this.wind = wind;
    }

    public float calculateArea()
    {
        Vector3 edgeAB = vertexB.pos - vertexA.pos;
        Vector3 edgeAC = vertexC.pos - vertexA.pos;
        return Vector3.Cross(edgeAB, edgeAC).magnitude / 2; 
    }

    public void ComputeForces()
    {
        Vector3 edgeAB = vertexB.pos - vertexA.pos;
        Vector3 edgeAC = vertexC.pos - vertexA.pos;
        Vector3 u = Vector3.Cross(edgeAB, edgeAC);
        u.Normalize();
        vel = (vertexA.vel + vertexB.vel + vertexC.vel) / 3;
        Vector3 force = friction * area * Vector3.Project((wind - vel), u) / 3;
        vertexA.force += force;
        vertexB.force += force;
        vertexC.force += force;
    }
}
