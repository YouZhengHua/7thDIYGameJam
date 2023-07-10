using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAWeapon : Weapon
{
    public Vector3 offset;
    private GameObject _ammoObj;

    public override void Start()
    {
        base.Start();
        //Testing code
        LoadWeapon();
    }
    public override void LoadWeapon(bool active = true)
    {
        base.LoadWeapon(active);
        _ammoObj = Instantiate(weaponData.AmmoPrefab, this.transform.position, Quaternion.identity);
    }

    public override bool Update()
    {
        if (!base.Update()) return false;
        // 获取玩家的移动方向
        Vector3 moveDirection = transform.forward;

        // 更新_ammoObj物体的位置 用leap去逐漸靠近玩家
        _ammoObj.transform.position = Vector3.Lerp(_ammoObj.transform.position, transform.position + offset + moveDirection, 0.1f);
        return true;
    }
}
