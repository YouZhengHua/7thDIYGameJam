﻿using System.Collections;
using System.Collections.Generic;
using Scripts.Game.Base;
using UnityEngine;
using Scripts.Base;
using Scripts.Game.Data;

namespace Scripts.Game
{
    public class EnemyPool : IEnemyPool
    {
        public IDictionary<int, IBasePool> enemyPools;
        public IList<GameObject> enemyies;
        public EnemyPool(IGameFiniteStateMachine gameFiniteStateMachine, IPlayerController player, IEndUIController endUI, LevelData[] levelDatas, IAttributeHandle attributeHandle, IExpPool expPool, IDropAmmoPool dropAmmoPool, IDamagePool damagePool, IDropHealthPool healthPool)
        {
            enemyPools = new Dictionary<int, IBasePool>();
            enemyies = new List<GameObject>();
            foreach (LevelData level in levelDatas)
            {
                foreach(LevelEnemyData enemy in level.EnemyDatas)
                {
                    if (!enemyPools.ContainsKey(enemy.GetInstanceID()))
                    {
                        enemyPools.Add(enemy.GetInstanceID(), CreateBasePool(gameFiniteStateMachine, player, endUI, enemy.Prefab, enemy.Data, attributeHandle, expPool, dropAmmoPool, damagePool, healthPool));
                    }
                }
            }
        }

        private IBasePool CreateBasePool(IGameFiniteStateMachine gameFiniteStateMachine, IPlayerController player, IEndUIController endUI, GameObject prefab, EnemyData enemyData, IAttributeHandle attributeHandle, IExpPool expPool, IDropAmmoPool dropAmmoPool, IDamagePool damagePool, IDropHealthPool healthPool)
        {
            IBasePool result = new BasePool(prefab, 100);
            result.Prefabs.ForEach(prefab =>
            {
                prefab.GetComponent<IEnemyController>().SetGameFinitStateMachine = gameFiniteStateMachine;
                prefab.GetComponent<IEnemyController>().SetPlayer = player;
                prefab.GetComponent<IEnemyController>().SetEndUI = endUI;
                prefab.GetComponent<IEnemyController>().SetEnemyData = enemyData;
                prefab.GetComponent<IEnemyController>().SetAttributeHandle = attributeHandle;
                prefab.GetComponent<IEnemyController>().SetExpPool = expPool;
                prefab.GetComponent<IEnemyController>().SetDropAmmoPool = dropAmmoPool;
                prefab.GetComponent<IEnemyController>().SetDamagePool = damagePool;
                prefab.GetComponent<IEnemyController>().SetDropHealthPool = healthPool;
            });
            return result;
        }

        public IList<GameObject> GetEnemies(LevelEnemyData levelEnemyData)
        {
            enemyies.Clear();
            if (enemyPools.TryGetValue(levelEnemyData.GetInstanceID(), out IBasePool pool))
            {
                for(int i = 0; i < levelEnemyData.Quantity; i++)
                {
                    enemyies.Add(pool.GetPrefab());
                }
            }
            return enemyies;
        }
    }
}