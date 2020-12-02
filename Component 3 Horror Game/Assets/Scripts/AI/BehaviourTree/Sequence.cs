using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    private List<Node> nodes = new List<Node>();
    public Sequence(List<Node> nodes) {
        this.nodes = nodes;
    }

    public override NodeStates Evaluate() {
        bool areChildNodesRunning = false;
        foreach (Node node in this.nodes) {
            switch (node.Evaluate()){
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    areChildNodesRunning = true;
                    continue;
                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }
        }
        m_nodeState = areChildNodesRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return m_nodeState; 
    }
}
