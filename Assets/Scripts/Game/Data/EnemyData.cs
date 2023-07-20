using UnityEngine;
using Scripts.Game.Base;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Enemy")]
	public class EnemyData : BaseData
	{
        [Header("經驗值"), Min(1)]
        public int exp = 1;

        [Header("傷害值")]
        public FloatAttributeHandle Damage;

        [Header("是否為Boss")]
        public bool IsBoss = false;

        [Header("攻擊範圍")]
        public FloatAttributeHandle AttackRange;

        [Header("是否可以被擊退")]
        public bool CanAddedForce = true;
    }
}