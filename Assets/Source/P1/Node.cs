using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Vector3 pos;
    public Vector3 vel;
    public Vector3 force;
    private Vector3 grav;
    public float mass;
    public bool isFixed;

    // Use this for initialization
    public Node(Vector3 pos, Vector3 grav, float mass)
    {
        this.pos = pos;
        this.vel = Vector3.zero;
        this.force = Vector3.zero;
        this.grav = grav;
        this.mass = mass;
        this.isFixed = false;
    }

    public void ComputeForces()
    {
        force += this.mass * this.grav;
    }

    public void setFixed(bool f)
    {
        this.isFixed = f;
    }
}
