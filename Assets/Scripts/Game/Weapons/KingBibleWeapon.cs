using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class KingBibleWeapon : Weapon
{
    private List<GameObject> _ammoObjList = new List<GameObject>();
    private float currentAngle = 0f;
    private float angleOffset = 360f / 3f;
    private float _ammoFlyTime = 0f;
    //旋轉半徑
    private float radius = 1f;
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
        _ammoFlyTime = weaponData.AmmoFlyTime.Value;
        radius = weaponData.DamageRadius.Value;
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

        weaponData.AmmoScale.OnMultipleChangedEvent.AddListener((float multiple) =>
        {
            for (int i = 0; i < _ammoObjList.Count; i++)
            {

                _ammoObjList[i].transform.localScale = new Vector3(weaponData.AmmoScale.Value, weaponData.AmmoScale.Value, weaponData.AmmoScale.Value);
            }
        });
        weaponData.AmmoFlyTime.OnMultipleChangedEvent.AddListener((float multiple) =>
        {
            _ammoFlyTime = weaponData.AmmoFlyTime.Value;
        });
        weaponData.DamageRadius.OnMultipleChangedEvent.AddListener((float multiple) => { radius = weaponData.DamageRadius.Value; });
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

        //持續一段時間後，逐漸快速縮小旋轉半徑與 _ammoObjList 裡面所有子彈的大小到 0
        _ammoFlyTime -= Time.deltaTime;
        if (_ammoFlyTime <= 0f)
        {
            _ammoFlyTime = 0f;
            StartCoroutine(Shrink());
        }

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
        float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float y = center.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
        return new Vector3(x, y, center.z);
    }


    IEnumerator Shrink()
    {
        _ammoFlyTime = -1f;
        //逐漸快速縮小物件繞著玩家旋轉的半徑與 _ammoObjList 裡面所有子彈的大小到 0
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            for (int i = 0; i < _ammoObjList.Count; i++)
            {
                float angle = currentAngle + i * angleOffset;
                Vector3 position = GetPositionOnCircle(angle);
                _ammoObjList[i].transform.localPosition = position;
                _ammoObjList[i].transform.localScale = Vector3.Lerp(new Vector3(weaponData.AmmoScale.Value, weaponData.AmmoScale.Value, weaponData.AmmoScale.Value), Vector3.zero, time);
                radius = Mathf.Lerp(weaponData.DamageRadius.Value, 0f, time);
            }
            yield return null;
        }
        //等待一段時間後
        yield return new WaitForSeconds(weaponData.CoolDownTime.Value);

        //逐漸快速放大物件繞著玩家旋轉的半徑與 _ammoObjList 裡面所有子彈的大小從 0 到 weaponData.AmmoScale.Value
        time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            for (int i = 0; i < _ammoObjList.Count; i++)
            {
                float angle = currentAngle + i * angleOffset;
                Vector3 position = GetPositionOnCircle(angle);
                _ammoObjList[i].transform.localPosition = position;
                _ammoObjList[i].transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(weaponData.AmmoScale.Value, weaponData.AmmoScale.Value, weaponData.AmmoScale.Value), time);
                radius = Mathf.Lerp(0f, weaponData.DamageRadius.Value, time);
            }
            yield return null;
        }

        _ammoFlyTime = weaponData.AmmoFlyTime.Value;
    }
}
