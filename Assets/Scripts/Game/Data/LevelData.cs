using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Level")]
    public class LevelData : ScriptableObject
    {
        [Header("關卡時間"), Range(0, 1200)]
        public float GameTime = 1200f;
        [Header("關卡回合")]
        public LevelRound[] LevelRounds;
        [Header("背景音樂")]
        public BGMData[] BGMs;
    }

    [System.Serializable]
    public struct LevelRound
    {
        [Header("關卡開始時間"), Range(0, 1200)]
        public float LevelStartTime;
        [Header("關卡結束時間"), Range(0, 1200)]
        public float LevelEndTime;
        [Header("關卡怪物資訊")]
        public LevelEnemyData[] EnemyDatas;
    }

    [System.Serializable]
    public struct BGMData
    {
        [Header("背景音樂")]
        public AudioClip Audio ;
        [Header("是否需要調整音量")]
        public bool IsNeedVolumn;
        [Header("調整音量"), Range(0f, 1f)]
        public float Volumn;
    }
}