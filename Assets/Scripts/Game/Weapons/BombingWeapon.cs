using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;

public class BombingWeapon : Weapon
{
    public override void Start()
    {
        base.Start();
    }

    public override bool Update()
    {
        if (!base.Update()) return false;
        _timer += Time.deltaTime;

        if (_timer >= weaponData.SkillTriggerInterval.Value)
        {
            _timer = 0f;
            for (int times = 0; times < weaponData.OneShootAmmoCount.Value; times++)
            {
                _throwingBomb();
            }

        }
        return true;
    }

    private void _throwingBomb()
    {
        Debug.Log("轰炸");
        Vector3 randomPoint = getRandomPointAroundPlayer();
        AudioController.Instance.PlayEffect(weaponData.ShootAudio);
        // 播放轰炸特效
        //TODO 用架構的物件池 AmmoPool
        GameObject effect = Instantiate(weaponData.AmmoPrefab, randomPoint, Quaternion.identity);
        //轟炸範圍影響特效大小裡面所有Particle System 的scale
        ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.startSize = weaponData.DamageRadius.Value;
        }



        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPoint, weaponData.DamageRadius.Value);

        foreach (Collider2D collider in colliders)
        {
            // 如果碰撞体属于敌人单位
            BaseEnemyController enemyUnit = collider.GetComponent<BaseEnemyController>();
            if (enemyUnit != null)
            {
                // 对敌人单位执行受伤的动作
                Debug.Log("enemyUnit = " + enemyUnit.name);
                enemyUnit.TakeDamage(weaponData.Damage.Value);
                enemyUnit.AddForce(weaponData.Force.Value, weaponData.DelayTime.Value);
            }
        }

        // 销毁特效
        Destroy(effect, 2f);
    }

    private Vector3 getRandomPointAroundPlayer()
    {
        // 在半径范围内生成随机点
        Vector2 randomCircle = Random.insideUnitCircle * weaponData.CreateRadius.Value;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }

    //For Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, weaponData.CreateRadius.Value);
    }
}
