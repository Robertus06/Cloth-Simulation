using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sample code for accessing MeshFilter data.
/// </summary>
public class MassSpring : MonoBehaviour
{
    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public MassSpring()
    {
        this.Paused = true;
        this.TimeStep = 0.01f;
        this.Gravity = new Vector3(0.0f, -9.81f, 0.0f);
        this.IntegrationMethod = Integration.Symplectic;
        this.mass = 1.0f;
        this.stiffness = 100.0f;
        this.damp = 15.0f;

        this.wind = false;
        this.windSpeed = new Vector3(0.0f, 0.0f, -50.0f);
        this.windFriction = 0.5f;

        this.nodes = new List<Node> { };
        this.springs = new List<Spring> { };
        this.edges = new List<Edge> { };
        this.windTriangles = new List<Triangle> { };
    }

    /// <summary>
	/// Integration method.
	/// </summary>
    public enum Integration
    {
        Explicit = 0,
        Symplectic = 1,
    };

    #region InEditorVariables

    public bool Paused;
    public float TimeStep;
    public Vector3 Gravity;
    public Integration IntegrationMethod;
    public float mass;
    public float stiffness;
    public float damp;

    public bool wind;
    public Vector3 windSpeed;
    public float windFriction;


    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private List<Node> nodes;
    private List<Spring> springs;
    private List<Edge> edges;
    private List<Triangle> windTriangles;

    #endregion

    #region OtherVariables

    #endregion

    #region MonoBehaviour

    public void Awake()
    {
        this.mesh = this.GetComponent<MeshFilter>().mesh;
        this.vertices = mesh.vertices;
        this.triangles = mesh.triangles;

        //For simulation purposes, transform the points to global coordinates
        createNodes();
        createEdges();
        createSprings();
    }

    public List<Node> getNodes()
    {
        return this.nodes;
    }

    public void createNodes()
    {
        for (int i = 0; i <= this.vertices.Length-1; i++)
        {
            Vector3 pos = transform.TransformPoint(this.vertices[i]);

            Node newNode = new Node(pos, this.Gravity, this.mass);
            /**
            if (i < 10)
            {
                newNode.setFixed(true);
            }
            /**/
            nodes.Add(newNode);
        }
    }

    public void createEdges()
    {
        float numTriangles = this.triangles.Length / 3;
        for (int i = 0; i <= numTriangles - 1; i++)
        {
            int j = i * 3;
            int vertex1 = this.triangles[j];
            int vertex2 = this.triangles[j + 1];
            int vertex3 = this.triangles[j + 2];

            Edge newEdge1 = new Edge(vertex1, vertex2, vertex3);
            edges.Add(newEdge1);
            Edge newEdge2 = new Edge(vertex2, vertex3, vertex1);
            edges.Add(newEdge2);
            Edge newEdge3 = new Edge(vertex3, vertex1, vertex2);
            edges.Add(newEdge3);

            Triangle newTriangle = new Triangle(nodes[vertex1], nodes[vertex2], nodes[vertex3], windFriction, windSpeed);
            windTriangles.Add(newTriangle);
        }

        edges.Sort();
    }

    public void createSprings()
    {
        Spring newSpring1 = new Spring(nodes[edges[0].VertexA], nodes[edges[0].VertexB], this.stiffness, this.damp);
        springs.Add(newSpring1);

        for (int i = 1; i < edges.Count; i++)
        {
            if (edges[i].compare(edges[i - 1]))
            {
                Spring newSpring = new Spring(nodes[edges[i].VertexOther], nodes[edges[i - 1].VertexOther], this.stiffness, this.damp);
                newSpring.stiffness = stiffness * 3 / 4;
                springs.Add(newSpring);
            }
            else
            {
                Spring newSpring = new Spring(nodes[edges[i].VertexA], nodes[edges[i].VertexB], this.stiffness, this.damp);
                springs.Add(newSpring);
            }
        }
    }

    public void Update()
    {
        this.mesh = this.GetComponent<MeshFilter>().mesh;
        this.vertices = new Vector3[mesh.vertexCount];

        for (int i = 0; i <= this.vertices.Length - 1; i++)
        {
            Vector3 pos = nodes[i].pos;
            this.vertices[i] = transform.InverseTransformPoint(pos);
        }
        mesh.vertices = this.vertices;
    }

    public void FixedUpdate()
    {
        if (this.Paused)
            return; // Not simulating

        // Select integration method
        switch (this.IntegrationMethod)
        {
            case Integration.Explicit: this.stepExplicit(); break;
            case Integration.Symplectic: this.stepSymplectic(); break;
            default:
                throw new System.Exception("[ERROR] Should never happen!");
        }

    }
    #endregion

    /// <summary>
    /// Performs a simulation step in 1D using Explicit integration.
    /// </summary>
    private void stepExplicit()
    {
        foreach (Node node in nodes)
        {
            node.force = Vector3.zero;
            node.ComputeForces();
        }
        if (wind)
        {
            foreach (Triangle windTriangle in windTriangles)
            {
                windTriangle.ComputeForces();
            }
        }
        foreach (Spring spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (Node node in nodes)
        {
            if (!node.isFixed)
            {
                node.pos += TimeStep * node.vel;
                node.vel += TimeStep / this.mass * node.force;
            }
        }

        foreach (Spring spring in springs)
        {
            spring.UpdateLength();
        }
    }

    /// <summary>
	/// Performs a simulation step in 1D using Symplectic integration.
	/// </summary>
	private void stepSymplectic()
    {
        foreach (Node node in nodes)
        {
            node.force = Vector3.zero;
            node.ComputeForces();
        }
        if (wind)
        {
            foreach (Triangle windTriangle in windTriangles)
            {
                windTriangle.ComputeForces();
            }
        }
        foreach (Spring spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (Node node in nodes)
        {
            if (!node.isFixed)
            {
                node.vel += TimeStep / this.mass * node.force;
                node.pos += TimeStep * node.vel;
            }
        }

        foreach (Spring spring in springs)
        {
            spring.UpdateLength();
        }
    }
}
