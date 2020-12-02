using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : Node
{
    private float damage;
    private GameObject target;

    public AttackNode(float damage, GameObject target) {
        this.damage = damage;
        this.target = target;
    }

    public override NodeStates Evaluate() {
        Debug.Log("Attack the player");
        return NodeStates.RUNNING;
    }
}
