using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node : MonoBehaviour
{
    //Using a delegate to return whether the particular node has succeeded, failed or is still running
    public delegate NodeStates NodeReturn();
    //Represents the current state of this particular node
    protected NodeStates m_nodeState;

    public NodeStates nodeState {
        get { return m_nodeState; }
    }

    public Node() { 
    }

    /*Determines the state of the node based on a set of conditions 
    Will use polymorphism in different scripts to override this function. */
    public abstract NodeStates Evaluate();
}
