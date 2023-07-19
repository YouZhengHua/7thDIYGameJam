using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class DroneAWeapon : Weapon
{
    public GameObject missileAmmoPrefab;
    public Vector3 offset;
    public float shootRadius = 5f;
    public float targetRadius = 5f;
    public float shootInterval = 0.1f;
    public int shootCount = 5;
    private GameObject _ammoObj;

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
        // _ammoObj.transform.SetParent(this.transform);
    }

    public override bool Update()
    {
        if (!base.Update()) return false;
        // 获取玩家的2d移动方向
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), -5f).normalized;

        // 更新_ammoObj物体的位置 用leap去逐漸靠近玩家
        _ammoObj.transform.position = Vector3.Lerp(_ammoObj.transform.position, transform.position + offset + moveDirection, 0.1f);

        //每隔一段時間就觸發一次隨機對半徑圓周上某一點進行子彈射擊
        _timer += Time.deltaTime;
        if (_timer >= weaponData.SkillTriggerInterval)
        {
            _timer = 0f;
            //執行射擊 shootCount
            _triggerShoot();
        }

        return true;
    }

    private void _triggerShoot()
    {
        // _ammoObj為圓心，半徑為 shootRadius，取出圓周上隨機一點的世界座標
        Vector3 randomPoint = getRandomPointAroundAmmoObj();
        Debug.Log("_triggerShoot randomPoint = " + randomPoint);
        //執行 launch shootCount 次,傳入 Vector3 參數，每次間隔 shootInterval 秒, 使用 StartCoroutine
        StartCoroutine(_launch(randomPoint));
    }

    IEnumerator _launch(Vector3 center)
    {
        for (int i = 0; i < shootCount; i++)
        {
            launch(center);
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void launch(Vector3 center)
    {
        //產生 missileAmmoPrefab
        Vector3 pos = new Vector3(_ammoObj.transform.position.x, _ammoObj.transform.position.y, 0);
        GameObject missileAmmo = Instantiate(missileAmmoPrefab, _ammoObj.transform.position, Quaternion.identity);
        missileAmmo.GetComponent<IAmmoEvent>().OnHitEnemy.AddListener(_hitEnemy);
        missileAmmo.GetComponent<MissileAmmoController>().SetTarget(getRandomPointInCircle(center, targetRadius));
        missileAmmo.GetComponent<MissileAmmoController>().StartHoming();
    }

    private Vector3 getRandomPointAroundAmmoObj()
    {
        // 在半径的圓周上讓取得一個随机点
        Vector2 randomCircle = Random.insideUnitCircle.normalized * shootRadius;
        Vector3 randomPoint = _ammoObj.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }

    //以一點為圓心，半徑為 shootRadius，取出圓內隨機一點的世界座標
    private Vector3 getRandomPointInCircle(Vector3 center, float radius)
    {
        // 在半径范围内生成随机点
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPoint = center + new Vector3(randomCircle.x, randomCircle.y, 0);

        return randomPoint;
    }

    private void _hitEnemy(Collider2D collision)
    {
        EnemyController enemyController;
        if (collision.TryGetComponent<EnemyController>(out enemyController))
        {
            Debug.Log("missileAmmo _hitEnemy");
            enemyController.TakeDamage(weaponData.Damage.Value, weaponData.DamageFrom, weaponData.Force, weaponData.DelayTime);
        }
    }

}