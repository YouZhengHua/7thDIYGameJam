using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class PlasmaMachineGunWeapon : Weapon
{
    public string FirePointName;
    public int OneShootAmmoCount;
    public Quaternion ShootOffset;
    public float AmmoFlySpeed;
    public string playerShootFire = "Fire";
    /// <summary>
    /// 玩家面對方向
    /// </summary>
    private Transform _playerRotation;
    /// <summary>
    /// 子彈發射位置
    /// </summary>
    private Transform _firePoint;

    /// <summary>
    /// 槍械動畫
    /// </summary>
    private Animator _gunEffect;
    private List<GameObject> _shotAmmo = new List<GameObject>();
    private float _currentMainShootCooldownTime = 0.0f;
    public override void Start()
    {
        base.Start();
        _currentMainShootCooldownTime = weaponData.CoolDownTime;

        _firePoint = this.GetComponentInChildren<Transform>();
        _gunEffect = _firePoint.gameObject.GetComponentInChildren<Animator>();

        _playerRotation = GameObject.Find("ZRotation").GetComponent<Transform>();
    }
    public override bool Update()
    {
        if (!base.Update()) return false;
        MainShootHandel();
        return true;
    }

    /// <summary>
    /// 槍械射擊處理
    /// 判斷是否處於射擊冷卻時間
    /// </summary>
    private void MainShootHandel()
    {
        _currentMainShootCooldownTime -= Time.deltaTime;
        if (_currentMainShootCooldownTime <= 0.0f)
        {
            _currentMainShootCooldownTime = 0f;
        }

        if (_currentMainShootCooldownTime == 0.0f)
        {
            MainShoot();
        }
    }

    /// <summary>
    /// 槍械射擊
    /// </summary>
    public void MainShoot()
    {

        for (int i = 0; i < OneShootAmmoCount; i++)
        {
            GameObject effect = Instantiate(weaponData.AmmoPrefab, this.transform.position, Quaternion.identity);
            _shotAmmo.Add(effect);
        }

        foreach (GameObject bullet in _shotAmmo)
        {
            bullet.transform.position = _firePoint.position;
            Vector3 currentRotation = _playerRotation.rotation.eulerAngles;
            // 將 Y 旋轉值用公式設為正值
            if (currentRotation.y < 0)
            {
                currentRotation.y = 360 + currentRotation.y;
            }
            Quaternion rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
            bullet.transform.rotation = rotation * ShootOffset;
            // 取得目前物體的旋轉值
            // Vector3 currentRotation = bullet.transform.rotation.eulerAngles;


            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = Vector3.zero.normalized;
            bulletRigidbody.AddForce(ShootOffset * _firePoint.up * AmmoFlySpeed);
            // bullet.GetComponent<IAmmoController>().AmmoGroup = Time.time.GetHashCode();
        }

        _gunEffect.SetTrigger(playerShootFire);

        //TODO 計算偏移
        // _attributeHandle.AddOffset();
        //TODO 播放音效
        // _audio.PlayEffect(_attributeHandle.ShootAudio);

        _shotAmmo.Clear();

        _currentMainShootCooldownTime = weaponData.CoolDownTime;
    }
}
