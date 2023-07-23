using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public float lifeTime = 2f;
    public bool isAutoDestroy = false;

    protected GameObject _spawner;
    protected BaseEnemyController _enemyTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyTarget != null)
        {
            OnBuffStay();
        }

    }

    public virtual void Init(Buff buff, BaseEnemyController enemyTarget, GameObject spawner)
    {
        Debug.Log("Buff Init");
        _enemyTarget = enemyTarget;
        _spawner = spawner;

        if (isAutoDestroy)
        {
            StartCoroutine(AutoDestroy());
        }
        OnBuffEnable();
    }

    //Buff啟用時
    public virtual void OnBuffEnable()
    {

    }

    //Buff常駐時
    public virtual void OnBuffStay()
    {

    }

    //Buff停用時
    public virtual void OnBuffDisable()
    {

    }

    public virtual void OnDestroy()
    {
        //印出 buff 類別的名字
        Debug.Log("Buff Destroy" + this.GetType().Name);
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroySelf();
    }

    public virtual void DestroySelf()
    {
        OnBuffDisable();
        Destroy(this.gameObject);
    }
}
