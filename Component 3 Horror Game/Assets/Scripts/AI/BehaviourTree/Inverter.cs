using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    private Node m_node;

    public Node node {
        get { return m_node; }
    }

    public Inverter(Node m_node) {
        this.m_node = m_node;
    }

    public override NodeStates Evaluate() {
        if (this.m_node.Evaluate() == NodeStates.FAILURE)
        {
            //UnityEngine.Debug.Log("INVERTER FAILURE -> SUCCESS");
            m_nodeState = NodeStates.SUCCESS;
            return m_nodeState;
        }
        else if (this.m_node.Evaluate() == NodeStates.SUCCESS)
        {
            //UnityEngine.Debug.Log("INVERTER SUCCESS -> FAILURE");
            m_nodeState = NodeStates.FAILURE;
            return m_nodeState;
        }
        else if (this.m_node.Evaluate() == NodeStates.RUNNING)
        {
            //UnityEngine.Debug.Log("INVERTER RUNNING -> RUNNING");
            m_nodeState = NodeStates.RUNNING;
            return m_nodeState;
        }
        //UnityEngine.Debug.Log("INVERTER SUCCESS");
        m_nodeState = NodeStates.SUCCESS;
        return m_nodeState;
    }

}
