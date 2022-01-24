using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring {

    public Node nodeA, nodeB;

    public float Length0;
    public float Length;

    public float stiffness;
    public float damp;

    // Use this for initialization
    public Spring(Node nodeA, Node nodeB, float stiffness, float damp)
    {
        this.nodeA = nodeA;
        this.nodeB = nodeB;
        this.stiffness = stiffness;
        this.damp = damp;

        UpdateLength();
        Length0 = Length;
    }
	
    public void UpdateLength ()
    {
        Length = (nodeA.pos - nodeB.pos).magnitude;
    }

    public void ComputeForces()
    {
        Vector3 u = nodeA.pos - nodeB.pos;
        u.Normalize();
        Vector3 force = - stiffness * (Length - Length0) * u;
        force += -damp * Vector3.Project((nodeA.vel - nodeB.vel), u);
        nodeA.force += force;
        nodeB.force -= force;
    }
}
