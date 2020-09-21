using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Nodes : MonoBehaviour
{
    //Using a delegate to return whether the particular node has succeeded, failed or is still running
    public delegate NodeStates NodeReturn();
    //Represents the current state of this particular node
    private NodeStates a_nodeState;

    public NodeStates nodeState {
        get { return a_nodeState; }
    }

    public Nodes() { 
    }

    /*Determines the state of the node based on a set of conditions 
    Will use polymorphism in different scripts to override this function. */
    public abstract NodeStates Evaluate();
}
