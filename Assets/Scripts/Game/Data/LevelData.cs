using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Level")]
    public class LevelData : ScriptableObject
    {
        [Header("關卡開始時間"), Range(0, 1200)]
        public float LevelStartTime = 0f;
        [Header("關卡結束時間"), Range(0, 1200)]
        public float LevelEndTime = 1200f;
        [Header("關卡怪物資訊")]
        public LevelEnemyData[] EnemyDatas;
    }
}