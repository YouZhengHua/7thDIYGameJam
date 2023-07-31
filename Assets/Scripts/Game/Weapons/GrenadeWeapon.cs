using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;

public class GrenadeWeapon : Weapon
{
    public float Force = 1f;
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
            AudioController.Instance.PlayEffect(weaponData.ShootAudio, weaponData.ExtendVolume);
            _timer = 0f;
            for (int times = 0; times < weaponData.OneShootAmmoCount.Value; times++)
            {
                _throwingGrenade();
            }

        }
        return true;
    }

    private void _throwingGrenade()
    {
        Vector3 randomPoint = getRandomPointAroundPlayer();
        // 播放投擲手榴彈特效
        GameObject effect = Instantiate(weaponData.AmmoPrefab, transform.position, Quaternion.identity);

        //從effect上拿到 GrenadeAmmoController並進行Init
        GrenadeAmmoController grenadeAmmoController = effect.GetComponent<GrenadeAmmoController>();
        grenadeAmmoController.Init(
            randomPoint,
            Force,
            weaponData.Damage.Value,
            weaponData.DamageRadius.Value,
            weaponData.AmmoFlyTime.Value,
            weaponData.BuffCoolDownTime.Value,
            weaponData.BuffLifeTime.Value
        );
        grenadeAmmoController.HitEnemy.AddListener(GrenadeHitEnemy);
    }

    private Vector3 getRandomPointAroundPlayer()
    {
        // 在半径范围内生成随机点
        Vector2 randomCircle = Random.insideUnitCircle * weaponData.CreateRadius.Value;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }

    private void GrenadeHitEnemy(BaseEnemyController baseEnemyController)
    {
        baseEnemyController.TakeDamage(weaponData.Damage.Value);
        baseEnemyController.AddForce(weaponData.Force.Value, weaponData.DelayTime.Value);
    }
}
