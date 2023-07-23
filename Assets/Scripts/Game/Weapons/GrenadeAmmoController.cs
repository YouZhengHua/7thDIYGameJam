using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class GrenadeAmmoController : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public float rotateSpeed = 10f;
    Rigidbody2D _rigidbody2D;
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

    public void Init(Vector3 targetPoint, float force, float delayDestroyTime, float damage, float damageRadius, float lifeTime)
    {
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        //讓炸彈往目標用物理的力量，被投擲出去到目標點
        _rigidbody2D.AddForce((targetPoint - transform.position).normalized * force, ForceMode2D.Impulse);
        StartCoroutine(_delayDestroy(delayDestroyTime));
        StartCoroutine(_delayDamage(damage, damageRadius, lifeTime));
    }

    private IEnumerator _delayDestroy(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
    }

    private IEnumerator _delayDamage(float damage, float damageRadius, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
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
        //TODO 爆炸範圍要影響特效大小
        // 销毁特效
        Destroy(effect, 2f);
    }

}
