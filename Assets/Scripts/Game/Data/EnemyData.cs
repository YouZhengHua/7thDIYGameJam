using UnityEngine;
using Scripts.Game.Base;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Enemy")]
	public class EnemyData : BaseData
	{
        [Header("經驗值"), Min(1)]
        public int exp = 1;

        [Header("傷害值"), Min(1)]
        public float Damage = 1f;

        [Header("是否為Boss")]
        public bool IsBoss = false;

        [Header("攻擊範圍"), Min(0)]
        public float AttackRange = 0;
    }
}