using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;

public class GrenadeWeapon : Weapon
{
    public float LifeTime = 1f;
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
        Debug.Log("投擲手榴彈");
        Vector3 randomPoint = getRandomPointAroundPlayer();
        AudioController.Instance.PlayEffect(weaponData.ShootAudio);
        // 播放投擲手榴彈特效
        //TODO 用架構的物件池 AmmoPool
        GameObject effect = Instantiate(weaponData.AmmoPrefab, transform.position, Quaternion.identity);
        //TODO 投擲手榴彈範圍要影響特效大小

        //從effect上拿到 GrenadeAmmoController並進行Init
        GrenadeAmmoController grenadeAmmoController = effect.GetComponent<GrenadeAmmoController>();
        grenadeAmmoController.Init(
            randomPoint,
            Force,
            LifeTime,
            weaponData.Damage.Value,
            weaponData.DamageRadius.Value,
            LifeTime
        );

    }

    private Vector3 getRandomPointAroundPlayer()
    {
        // 在半径范围内生成随机点
        Vector2 randomCircle = Random.insideUnitCircle * weaponData.CreateRadius.Value;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }
}
