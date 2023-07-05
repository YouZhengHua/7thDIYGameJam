using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Exp")]
    public class ExpData : DropItemData
    {
        [Header("經驗值數值")]
        public ExpNumber expNumber;
    }
}