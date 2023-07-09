using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class BombingWeapon : Weapon
{
    public float createRadius = 2f; // 生成半徑
    public float bombRadius = 2f; // 轰炸半径
    public float bombingInterval = 5f; // 轰炸间隔时间
    // 轰炸次數
    public int bombingCount = 1;
    private float _timer = 0f;
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;

        if (_timer >= bombingInterval)
        {
            _timer = 0f;
            for (int times = 0; times < bombingCount; times++)
            {
                _throwingBomb();
            }

        }
    }

    private void _throwingBomb()
    {
        Debug.Log("轰炸");
        Vector3 randomPoint = getRandomPointAroundPlayer();

        // 播放轰炸特效
        GameObject effect = Instantiate(weaponData.AmmoPrefab, randomPoint, Quaternion.identity);

        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPoint, bombRadius);

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

        // 销毁特效
        Destroy(effect, 2f);
    }

    private Vector3 getRandomPointAroundPlayer()
    {
        // 在半径范围内生成随机点
        Vector2 randomCircle = Random.insideUnitCircle * createRadius;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }

    //For Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, createRadius);
    }
}
