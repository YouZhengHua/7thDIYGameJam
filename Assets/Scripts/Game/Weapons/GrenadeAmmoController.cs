using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class GrenadeAmmoController : MonoBehaviour
{
    public GameObject ExplosionEffect;
    //持續燃燒的子彈 GameObject
    public GameObject FireBullet;
    public float rotateSpeed = 10f;
    Rigidbody2D _rigidbody2D;
    private float buffCoolDownTime = 1f;
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //自轉
        transform.Rotate(new Vector3(0, 0, 1), rotateSpeed);
    }

    public void Init(Vector3 targetPoint, float force, float damage, float damageRadius, float ammoFlyTime, float buffCoolDownTime, float buffLifeTime)
    {
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        this.buffCoolDownTime = buffCoolDownTime;
        //讓炸彈往目標用物理的力量，被投擲出去到目標點
        _rigidbody2D.AddForce((targetPoint - transform.position).normalized * force, ForceMode2D.Impulse);
        StartCoroutine(_delayDamage(damage, damageRadius, ammoFlyTime, buffLifeTime));
        StartCoroutine(_delayDestroy(ammoFlyTime));
    }

    private IEnumerator _delayDestroy(float delayTime)
    {
        yield return new WaitForSeconds(delayTime + 0.01f);
        Destroy(this.gameObject);
    }

    private IEnumerator _delayDamage(float damage, float damageRadius, float ammoFlyTime, float buffLifeTime)
    {
        yield return new WaitForSeconds(ammoFlyTime);
        //在自身位置生成一個火焰子彈
        GameObject fireBullet = Instantiate(FireBullet, transform.position, Quaternion.identity);
        FireAmmoController fireAmmoController = fireBullet.GetComponent<FireAmmoController>();
        fireAmmoController.Init(damage / 4, damageRadius / 4, buffLifeTime, buffCoolDownTime);
        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (Collider2D collider in colliders)
        {
            // 如果碰撞体属于敌人单位
            BaseEnemyController enemyUnit = collider.GetComponent<BaseEnemyController>();
            if (enemyUnit != null)
            {
                // 对敌人单位执行受伤的动作
                Debug.Log("enemyUnit = " + enemyUnit.name);
                enemyUnit.TakeDamage(damage);
                enemyUnit.AddForce(damage, 0.1f);
            }
        }

        // 播放爆炸特效
        GameObject effect = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        //轟炸範圍影響特效大小裡面所有Particle System 的scale
        ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.startSize = damageRadius;
        }


        // 销毁特效
        Destroy(effect, 2f);
    }

}
