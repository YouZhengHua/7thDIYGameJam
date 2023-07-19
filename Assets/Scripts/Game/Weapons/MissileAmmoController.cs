using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

public class MissileAmmoController : Projectile, IAmmoEvent
{
    public UnityEvent<Collider2D> OnHitEnemy { get => _onHitEnemyEvent; set => _onHitEnemyEvent = value; }

    private UnityEvent<Collider2D> _onHitEnemyEvent = new UnityEvent<Collider2D>();

    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    [Header("==== SPEED CHANGE ====")]
    [SerializeField] float lowSpeed = 8f;
    [SerializeField] float highSpeed = 25f;
    [SerializeField] float variableSpeedDelay = 0.5f;

    [Header("==== EXPLOSION ====")]
    [SerializeField] GameObject explosionVFX = null;

    WaitForSeconds waitVariableSpeedDelay;

    protected void Awake()
    {
        waitVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {

        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    public void StartHoming()
    {
        StartCoroutine(guidanceSystem.HomingCoroutine(targetPos));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Spawn a explosion VFX
        // PoolManager.Release(explosionVFX, transform.position);
        // Play explosion SFX
        // AudioManager.Instance.PlayRandomSFX(explosionSFX);
    }

    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitVariableSpeedDelay;

        moveSpeed = highSpeed;

        if (targetPos != null)
        {
            // AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onHitEnemyEvent?.Invoke(collision);
    }

    // * AOE Damage Implementation 2
    // * 范围伤害实现方法2
    // !Disadvantages: To detect all enemies in the scene, slightly lower efficiency 
    // !缺点：检测场景中所有的敌人，效率稍低
    // void DistanceDetection()
    // {
    //     // Loop detection all enemies in current scene
    //     // 遍历当前场景中所有的敌人
    //     foreach (var enemyInRange in EnemyManager.Instance.Enemies)
    //     {
    //         // If the distance between the enemy and the missile is within the explosion radius (3f)
    //         // 如果敌人和导弹的距离在爆炸半径(3f)内
    //         if (Vector2.Distance(transform.position, enemyInRange.transform.position) <= 3f)
    //         {
    //             if (enemyInRange.TryGetComponent<Enemy>(out Enemy enemy))
    //             {
    //                 // enemy take 100 damage
    //                 // 则敌人受到100点伤害
    //                 enemy.TakeDamage(100f);
    //             }
    //         }
    //     }
    // }

    // * AOE Damage Implementation 3
    // * 范围伤害实现方法3
    // [SerializeField] LayerMask enemyLayerMask = default;
    // [SerializeField] float explosionRadius = 3f;
    // [SerializeField] float explosionDamage = 100f;

    // void PhysicsOverlapDetection()
    // {
    //     // Enemies within explosion radius take AOE damage
    //     var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayerMask);

    //     foreach (var collider in colliders)
    //     {
    //         if (collider.TryGetComponent<Enemy>(out Enemy enemy))
    //         {
    //             enemy.TakeDamage(explosionDamage);
    //         }
    //     }
    // }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, explosionRadius);
    // }
}
