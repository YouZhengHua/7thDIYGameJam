using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/LevelEnemy")]
    public class LevelEnemyData : ScriptableObject
    {
        private float? _nextTime = null;
        public float NextTime
        {
            get
            {
                if (!_nextTime.HasValue)
                {
                    _nextTime = 0f;
                }
                return _nextTime.Value;
            }
            set
            {
                _nextTime = value;
            }
        }
        [Header("生成距離"), Range(5f, 20f)]
        public float Distance = 15f;
        [Header("生成距離區間"), Min(0f)]
        public float DistanceRange = 0f;
        /// <summary>
        /// 怪物生成間隔
        /// </summary>
        [Header("生成間隔"), Min(0.1f)]
        public float Intervals = 1f;
        [Header("生成數量"), Min(1)]
        public int Quantity = 1;
        [Header("怪物預置物")]
        public GameObject Prefab;
        [Header("是否團聚生成")]
        public bool IsGroup = false;
        [Header("是否環形生成")]
        public bool IsRound = false;
        [Header("關卡怪物登場音效")]
        public AudioClip WarmingAudio;
        [Header("音效額外音量"), Range(0f, 3f)]
        public float ExtendVolume = 0f;
    }
}