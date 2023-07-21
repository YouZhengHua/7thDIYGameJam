using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/DropItemData")]
    public class DropItemData : ScriptableObject
    {
        [Header("飛行速度")]
        public float FlySpeed;
    }
}