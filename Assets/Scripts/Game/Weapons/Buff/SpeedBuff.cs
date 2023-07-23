using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    //影響速度的倍率
    public float speed = 15f;

    public override void OnBuffEnable()
    {
        base.OnBuffEnable();
        _enemyTarget.SetMoveSpeedMultiple(speed / 100f);
        _enemyTarget.OnDeadEvent.AddListener(DestroySelf);
    }

    public override void OnBuffDisable()
    {
        base.OnBuffDisable();
        _enemyTarget.SetMoveSpeedMultiple(-(speed / 100f));
    }
}
