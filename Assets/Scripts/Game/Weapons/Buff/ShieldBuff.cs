using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : Buff
{
    public int shield = 1;

    public override void OnBuffEnable()
    {
        base.OnBuffEnable();
        //每x秒增加x點護盾

    }

}
