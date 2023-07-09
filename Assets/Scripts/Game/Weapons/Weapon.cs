using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
    //TODO 接 AttributeHandle? 串Option

    //TODO 接 IAudioContoller
    public IAudioContoller SetAudio { set => _audio = value; }
    protected float _timer = 0f;
    protected bool _weaponActive = true;
    protected IAudioContoller _audio;
    protected IGameFiniteStateMachine _gameFiniteStateMachine;
    // Start is called before the first frame update
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual bool Update()
    {
        return _weaponActive && _gameFiniteStateMachine.CurrectState == GameState.InGame;
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
}

public interface IAmmoEvent
{
    public UnityEvent<Collider2D> OnHitEnemy { get; set; }
}