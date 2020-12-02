using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectors : Node
{
    private List<Node> nodes = new List<Node>();
    public Selectors(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    /*Evaluates the state of each individual child node of the
     parent selector */
    public override NodeStates Evaluate()
    {
        foreach (Node node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        return m_nodeState;
    }
}
