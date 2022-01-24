using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixer : MonoBehaviour
{
    public GameObject tela;
    private Vector3 posFixerAnterior;
    List<Node> nodesFixed = new List<Node>();

	// Possibilities of the Fixer
	void Start ()
    {
        this.posFixerAnterior = this.transform.position;

        List<Node> nodes = tela.GetComponent<MassSpring>().getNodes();

        Bounds bounds = GetComponent<Collider>().bounds;

        foreach (Node node in nodes)
        {
            if (bounds.Contains(node.pos))
            {
                node.setFixed(true);
                nodesFixed.Add(node);
            }

        }
    }

    // Mueve la tela entera
    /**
    private void Update()
    {
        Vector3 posFixer = this.transform.position;

        if (posFixer - this.posFixerAnterior != Vector3.zero)
        {
            Vector3 move = posFixer - this.posFixerAnterior;
            List<Node> nodes = tela.GetComponent<MassSpring>().getNodes();
            foreach (Node node in nodes)
            {
                node.pos += move;
            }
            this.posFixerAnterior = posFixer;
        }
    }
    /**/

    // Mueve la tela solo de los nodos fijos
    /**/
    private void Update()
    {
        Vector3 posFixer = this.transform.position;

        if (posFixer - this.posFixerAnterior != Vector3.zero)
        {
            Vector3 move = posFixer - this.posFixerAnterior;
            foreach (Node node in this.nodesFixed)
            {
                node.pos += move;
            }
            this.posFixerAnterior = posFixer;
        }
    }
    /**/
}