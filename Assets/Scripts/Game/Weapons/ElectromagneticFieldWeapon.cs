using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class ElectromagneticFieldWeapon : Weapon
{
    public float electromagneticRadius = 2f; // 電磁半徑
    //自轉速度
    public float rotateSpeed = 100f;

    private GameObject _ammoObj = null;
    public override void Start()
    {
        base.Start();
        _ammoObj = Instantiate(weaponData.AmmoPrefab, this.transform.position, Quaternion.identity);
        if (_ammoObj == null)
        {
            Debug.LogError("電磁武器的特效為空");
            return;
        }
        _ammoObj.transform.SetParent(this.transform);
        //TODO 電磁範圍要影響特效大小
    }

    public override void Update()
    {
        base.Update();
        if (_ammoObj == null) return;
        _timer += Time.deltaTime;

        if (_timer >= weaponData.SkillTriggerInterval)
        {
            _timer = 0f;
            _triggerElectricShock();
        }

        //自動旋轉 _ammoObj
        _ammoObj.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }

    private void _triggerElectricShock()
    {
        Debug.Log("觸發電磁");
        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, electromagneticRadius);

        foreach (Collider2D collider in colliders)
        {
            // 如果碰撞体属于敌人单位
            EnemyController enemyUnit = collider.GetComponent<EnemyController>();
            if (enemyUnit != null)
            {
                // 对敌人单位执行受伤的动作
                Debug.Log("enemyUnit = " + enemyUnit.name);
                enemyUnit.TakeDamage(weaponData.Damage, weaponData.DamageFrom, weaponData.Force, weaponData.DelayTime);
            }
        }
    }
}
