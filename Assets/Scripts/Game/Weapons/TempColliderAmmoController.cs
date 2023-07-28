using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

public class TempColliderAmmoController : MonoBehaviour, IAmmoEvent
{
    public UnityEvent<Collider2D> OnHitEnemy { get => _onHitEnemyEvent; set => _onHitEnemyEvent = value; }

    private UnityEvent<Collider2D> _onHitEnemyEvent = new UnityEvent<Collider2D>();

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
        _onHitEnemyEvent?.Invoke(collision);
    }
}
