using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPoint : Node
{
    private float radius;
    private EnemyAI enemyAI;

    public SetPoint(float radius, EnemyAI enemyAI) {
        this.radius = radius;
        this.enemyAI = enemyAI;
    }

    public override NodeStates Evaluate() {
        enemyAI.FindPoint(Random.insideUnitSphere * radius);
        return NodeStates.SUCCESS;
    }
}
