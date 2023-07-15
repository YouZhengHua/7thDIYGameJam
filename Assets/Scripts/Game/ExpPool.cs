using System.Collections.Generic;
using UnityEngine;
using Scripts.Base;
using Scripts.Game.Data;

namespace Scripts.Game
{
    public class ExpPool : IExpPool
    {
        private IBasePool expPool1;
        private IBasePool expPool2;
        private IBasePool expPool3;
        private List<GameObject> expPrefabs;
        public ExpPool(ExpData exp1, ExpData exp2, ExpData exp3, IGameUIController gameUI, Transform playerTransform)
        {
            expPool1 = new BasePool(exp1.poolData.prefab, exp1.poolData.poolSize);
            expPool2 = new BasePool(exp2.poolData.prefab, exp2.poolData.poolSize);
            expPool3 = new BasePool(exp3.poolData.prefab, exp3.poolData.poolSize);
            expPrefabs = new List<GameObject>();
            expPool1.Prefabs.ForEach(prefab =>
            {
                prefab.GetComponent<IDropExpController>().SetExpData = exp1;
                prefab.GetComponent<IDropExpController>().SetGameUI = gameUI;
                prefab.GetComponent<IDropExpController>().SetPlayerTransform = playerTransform;
            });
            expPool2.Prefabs.ForEach(prefab =>
            {
                prefab.GetComponent<IDropExpController>().SetExpData = exp2;
                prefab.GetComponent<IDropExpController>().SetGameUI = gameUI;
                prefab.GetComponent<IDropExpController>().SetPlayerTransform = playerTransform;
            });
            expPool3.Prefabs.ForEach(prefab =>
            {
                prefab.GetComponent<IDropExpController>().SetExpData = exp3;
                prefab.GetComponent<IDropExpController>().SetGameUI = gameUI;
                prefab.GetComponent<IDropExpController>().SetPlayerTransform = playerTransform;
            });
        }

        public GameObject[] GetExpPrefabs(int exp)
        {
            expPrefabs.Clear();
            while(exp >= (int)ExpNumber.exp3)
            {
                expPrefabs.Add(expPool3.GetPrefab());
                exp -= (int)ExpNumber.exp3;
            }
            while (exp >= (int)ExpNumber.exp2)
            {
                expPrefabs.Add(expPool2.GetPrefab());
                exp -= (int)ExpNumber.exp2;
            }
            while (exp >= (int)ExpNumber.exp1)
            {
                expPrefabs.Add(expPool1.GetPrefab());
                exp -= (int)ExpNumber.exp1;
            }
            return expPrefabs.ToArray();
        }
    }
}