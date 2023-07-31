using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;

public class HandheldMachineGunWeapon : Weapon
{
    public string FirePointName;
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
    [SerializeField, Header("多發子彈時的擴散角度"), Range(0f, 180f)]
    private float _angleRange = 30f;
    private Quaternion _offset;
    public override void Start()
    {
        base.Start();
        _currentMainShootCooldownTime = weaponData.CoolDownTime.Value;

        _firePoint = this.GetComponentInChildren<Transform>();
        _gunEffect = _firePoint.gameObject.GetComponentInChildren<Animator>();

        _playerRotation = GameObject.Find("ZRotation").GetComponent<Transform>();
    }
    public override bool Update()
    {
        if (!base.Update()) return false;
        _firePoint.rotation = _playerRotation.rotation;
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
        for (int i = 0; i < weaponData.OneShootAmmoCount.Value; i++)
        {
            GameObject effect = Instantiate(weaponData.AmmoPrefab);
            _shotAmmo.Add(effect);
        }

        for (int i = 0; i < _shotAmmo.Count; i++)
        {
            _shotAmmo[i].transform.position = _firePoint.position;
            _offset = Quaternion.Euler(0f, 0f, weaponData.OneShootAmmoCount.Value == 1 ? 0f : (_angleRange / 2f) - (_angleRange / (weaponData.OneShootAmmoCount.Value - 1) * i));
            _shotAmmo[i].transform.localRotation = Quaternion.Euler(_playerRotation.localRotation.eulerAngles + _shotAmmo[i].transform.rotation.eulerAngles);
            _shotAmmo[i].GetComponent<BulletController>().Init(Vector3.zero, _offset * _playerRotation.up, weaponData.AmmoFlySpeed.Value, weaponData.Damage.Value, weaponData.HavaPenetrationLimit ? weaponData.AmmoPenetrationCount.Value : -1);
            _shotAmmo[i].GetComponent<BuffSpawner>().SetSpawnActive(weaponData.BuffSpawnActive.Value);
        }

        _gunEffect.SetTrigger(playerShootFire);

        AudioController.Instance.PlayEffect(weaponData.ShootAudio, weaponData.ExtendVolume);

        _shotAmmo.Clear();

        _currentMainShootCooldownTime = weaponData.CoolDownTime.Value;
    }
}