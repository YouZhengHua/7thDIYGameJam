using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _buffPrefab;
    protected bool _spawnActive = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpawnActive(bool active)
    {
        _spawnActive = active;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_spawnActive) return;
        BaseEnemyController enemy;
        if (other.TryGetComponent<BaseEnemyController>(out enemy))
        {
            //檢查 other 物件包含子物件中找到掛有 Buff 這個 component 的GameObject 的話
            if (other.transform.Find(_buffPrefab.name + "(Clone)") != null) return;

            //TODO 要使用物件池
            //用 _buffPrefab 產生一個 buff 物件，放到 enemy 物件上
            GameObject buff = Instantiate(_buffPrefab, enemy.transform);
            //取得 buff 的 Buff 元件
            Buff buffComponent = buff.GetComponent<Buff>();
            //呼叫 Buff 元件的 Init 方法，並傳入 Buff 元件、enemy 物件、this.gameObject
            buffComponent.Init(buffComponent, enemy, this.gameObject);
        }
    }
}
