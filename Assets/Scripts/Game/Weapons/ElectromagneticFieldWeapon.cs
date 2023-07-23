using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;

public class ElectromagneticFieldWeapon : Weapon
{
    //自轉速度
    public float rotateSpeed = 100f;
    private float _soundDealy = 10f;
    private float _nextSoundTime = 0f;

    private GameObject _ammoObj = null;
    public override void Start()
    {
        base.Start();
        //Testing code
        // LoadWeapon();
    }

    public override void LoadWeapon(bool active = true)
    {
        base.LoadWeapon(active);
        _ammoObj = Instantiate(weaponData.AmmoPrefab, this.transform.position, Quaternion.identity);
        if (_ammoObj == null)
        {
            Debug.LogError("電磁武器的特效為空");
            return;
        }
        _ammoObj.transform.SetParent(this.transform);
        //電磁傷害範圍要影響_ammoObj scale大小
        _ammoObj.transform.localScale = new Vector3(weaponData.DamageRadius.Value, weaponData.DamageRadius.Value, weaponData.DamageRadius.Value);

        weaponData.DamageRadius.OnMultipleChangedEvent.AddListener((float multiple) =>
                {
                    _ammoObj.transform.localScale = new Vector3(weaponData.DamageRadius.Value, weaponData.DamageRadius.Value, weaponData.DamageRadius.Value);
                });
    }

    public override void UnloadWeapon()
    {
        base.UnloadWeapon();
        Destroy(_ammoObj);
    }

    public override bool Update()
    {
        if (!base.Update()) return false;
        if (_ammoObj == null) return false;
        _timer += Time.deltaTime;

        if (_timer >= weaponData.SkillTriggerInterval.Value)
        {
            _timer = 0f;
            _triggerElectricShock();
        }

        //自動旋轉 _ammoObj
        _ammoObj.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);

        if(_nextSoundTime >= _soundDealy || _nextSoundTime == 0f)
        {
            PlaySound();
            _nextSoundTime = 0f;
        }
        _nextSoundTime += Time.deltaTime;
        return true;
    }

    private void _triggerElectricShock()
    {
        Debug.Log("觸發電磁");
        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, weaponData.DamageRadius.Value);
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
    }

    private void PlaySound()
    {
        AudioController.Instance.PlayEffect(weaponData.ShootAudio);
    }
}
