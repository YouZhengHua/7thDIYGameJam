using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class FireAmmoController : MonoBehaviour
{
    public float damage = 10f;
    public float damageRadius = 1f;
    public float buffLifeTime = 5f;
    public float buffCoolDownTime = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(float damage, float damageRadius, float buffLifeTime, float buffCoolDownTime)
    {
        this.damage = damage;
        this.damageRadius = damageRadius;
        this.buffLifeTime = buffLifeTime;
        this.buffCoolDownTime = buffCoolDownTime;
        //轟炸範圍影響特效大小裡面所有Particle System 的scale
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.startSize = damageRadius;
        }
        //每隔一段時間，對範圍內的敵人造成傷害，維持一段時間後銷毀自己
        StartCoroutine(_delayDamage());
        StartCoroutine(_delayDestroy());
    }

    private IEnumerator _delayDamage()
    {
        yield return new WaitForSeconds(buffCoolDownTime);
        // 在半径范围内查找敌人单位
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (Collider2D collider in colliders)
        {
            // 如果碰撞体属于敌人单位
            BaseEnemyController enemyUnit = collider.GetComponent<BaseEnemyController>();
            if (enemyUnit != null)
            {
                // 对敌人单位造成伤害
                enemyUnit.TakeDamage(damage);
            }
        }
        StartCoroutine(_delayDamage());
    }

    private IEnumerator _delayDestroy()
    {
        yield return new WaitForSeconds(buffLifeTime);
        Destroy(this.gameObject);
    }
}
