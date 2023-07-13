using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField, Header("武器資料")]
    private WeaponData _weaponData;
    private WeaponData _cloneWeaponData;
    //TODO 接 AttributeHandle? 串Option

    //TODO 接 IAudioContoller
    protected float _timer = 0f;
    protected bool _weaponActive = true;

    public virtual void Awake()
    {
        _cloneWeaponData = Object.Instantiate(_weaponData);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual bool Update()
    {
        return _weaponActive && GameStateMachine.Instance.CurrectState == GameState.InGame;
    }

    public virtual void SetWeaponActive(bool active)
    {
        _weaponActive = active;
    }

    public virtual void LoadWeapon(bool active = true)
    {
        _weaponActive = active;
    }

    public virtual void ReloadWeapon()
    {
    }

    public virtual void UnloadWeapon()
    {
        Destroy(this);
    }

    public WeaponIndex GetWeaponIndex { get => weaponData.WeaponIndex; }
    public WeaponData weaponData { get => _cloneWeaponData; }
}

public interface IAmmoEvent
{
    public UnityEvent<Collider2D> OnHitEnemy { get; set; }
}