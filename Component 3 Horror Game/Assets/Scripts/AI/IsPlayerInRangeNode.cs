using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerInRangeNode : Node
{
    private Transform target;
    private Transform enemyUnit;
    public IsPlayerInRangeNode(Transform target, Transform enemyUnit) {
        this.Target = target;
        this.enemyUnit = enemyUnit;
    }

    public Transform Target { get => target; set => target = value; }

    public override NodeStates Evaluate() {
        float distance = Vector3.Distance(enemyUnit.position, target.position);
        if (distance < .2f)
        {
            return NodeStates.SUCCESS;
        }
        else {
            return NodeStates.FAILURE;
        }
    }
}
