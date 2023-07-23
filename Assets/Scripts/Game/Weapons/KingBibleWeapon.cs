using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class KingBibleWeapon : Weapon
{
    private List<GameObject> _ammoObjList = new List<GameObject>();
    private float currentAngle = 0f;
    private float angleOffset = 360f / 3f;
    public override void Start()
    {
        base.Start();
        //Testing code
        // LoadWeapon();
    }
    public override void LoadWeapon(bool active = true)
    {
        base.LoadWeapon(active);
        angleOffset = 360f / (float)weaponData.OneShootAmmoCount.Value;
        for (int i = 0; i < weaponData.OneShootAmmoCount.Value; i++)
        {
            GameObject _ammoObj = Instantiate(weaponData.AmmoPrefab, this.transform.position, Quaternion.identity);
            _ammoObj.GetComponent<IBulletEvent>().OnHitEvent.AddListener(_hitEnemy);
            _ammoObj.GetComponent<BulletController>().Init(Vector3.zero, Vector3.zero, 0f, -1, weaponData.Damage.Value);
            _ammoObj.transform.SetParent(this.transform);
            _ammoObjList.Add(_ammoObj);
            float angle = i * angleOffset;
            Vector3 position = GetPositionOnCircle(angle);
            _ammoObj.transform.localPosition = position;
        }
    }

    private void _hitEnemy(Collider2D collision)
    {
        Debug.Log("KingBibleWeapon _hitEnemy");
        BaseEnemyController enemyController;
        if (TryGetComponent<BaseEnemyController>(out enemyController))
        {
            enemyController.TakeDamage(weaponData.Damage.Value);
            enemyController.AddForce(weaponData.Force.Value, weaponData.DelayTime.Value);
        }
    }

    public override bool Update()
    {
        if (!base.Update()) return false;
        // 使物件繞著玩家旋轉
        for (int i = 0; i < _ammoObjList.Count; i++)
        {
            float angle = currentAngle + i * angleOffset;
            Vector3 position = GetPositionOnCircle(angle);
            _ammoObjList[i].transform.localPosition = position;
        }

        // 更新角度
        currentAngle += weaponData.AmmoFlySpeed.Value * Time.deltaTime;
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
        }
        return true;
    }

    public override void ReloadWeapon()
    {
        base.ReloadWeapon();
        _ammoObjList.ForEach(col => Destroy(col));
        _ammoObjList.Clear();
        LoadWeapon();
    }

    private Vector3 GetPositionOnCircle(float angle)
    {
        Vector3 center = this.transform.localPosition;
        float x = center.x + weaponData.DamageRadius.Value * Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = center.y + weaponData.DamageRadius.Value * Mathf.Sin(Mathf.Deg2Rad * angle);
        return new Vector3(x, y, center.z);
    }
}
