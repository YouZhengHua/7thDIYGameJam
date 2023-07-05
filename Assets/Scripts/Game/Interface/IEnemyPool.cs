using Scripts.Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public interface IEnemyPool
    {
        public IList<GameObject> GetEnemies(LevelEnemyData levelEnemyData);
    }
}