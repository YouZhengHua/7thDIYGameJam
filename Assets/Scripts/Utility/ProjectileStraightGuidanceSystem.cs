using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightGuidanceSystem : ProjectileGuidanceSystem
{
    public override void MoveAction(Vector3 targetPos)
    {
        //直線往targetPos的方向飛去
        targetDirection = targetPos - transform.position;
        projectile.SetMoveDirection(targetDirection.normalized);
        projectile.Move();
    }
}
