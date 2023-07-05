using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/DropItem")]
    public class DropItemData : ScriptableObject
    {
        [Header("掉落物回收速度")]
        public float speed = 100;

        [Header("預置物池資料")]
        public PoolData poolData;
    }
}