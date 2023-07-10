using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

public class TempColliderAmmoController : MonoBehaviour, IAmmoController, IAmmoEvent
{
    public IGameFiniteStateMachine SetGameFiniteStateMachine { set => throw new System.NotImplementedException(); }
    public IAttributeHandle SetAttributeHandle { set => throw new System.NotImplementedException(); }
    public Transform SetPlayerTransform { set => throw new System.NotImplementedException(); }
    public IEndUIController SetEndUI { set => throw new System.NotImplementedException(); }

    public bool IsActive => true;

    public int AmmoGroup { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public UnityEvent<Collider2D> OnHitEnemy { get => _onHitEnemyEvent; set => _onHitEnemyEvent = value; }

    private UnityEvent<Collider2D> _onHitEnemyEvent = new UnityEvent<Collider2D>();

    public void HitEmeny()
    {
        //nothing to do
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TempColliderAmmoController OnTriggerEnter2D");
        _onHitEnemyEvent?.Invoke(collision);
    }
}
