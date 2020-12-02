using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInRangeNode : Node
{
    private LineOfSight los;
    public IsInRangeNode(LineOfSight los) {
        this.Los = los;
    }

    public LineOfSight Los { get => los; set => los = value; }
    public override NodeStates Evaluate() {
        return Los.visibleTargets != null ? NodeStates.SUCCESS : NodeStates.FAILURE; //Checks if the player is within the line of sight, if so, 
        //then the node returns its state as SUCCESS, else, then its FAILURE
    }
}
