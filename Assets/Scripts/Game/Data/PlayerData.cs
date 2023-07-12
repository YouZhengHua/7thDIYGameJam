using UnityEngine;
using Scripts.Game.Base;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Player")]
	public class PlayerData : BaseData
	{
        private float? _MaxHealthPoint;

        /// <summary>
        /// 血量最大值
        /// </summary>
        public float MaxHealthPoint
        {
            get
            {
                if (!_MaxHealthPoint.HasValue)
                {
                    _MaxHealthPoint = Mathf.RoundToInt(HealthPoint);
                }
                return _MaxHealthPoint.Value;
            }
            set
            {
                _MaxHealthPoint = value;
            }
        }

        [Header("擊退半徑")]
        public float Radius = 5;

        [Header("擊退力道")]
        public float Force = 100;

        [Header("擊退持續時間")]
        public float EnemyDelayTime = 1f;

        [Header("玩家等級"), Min(1)]
        public int Level = 1;

        [Header("玩家經驗值")]
        public float NowExp = 0f;

        [Header("吸收掉落物半徑")]
        public float DropItemRadius = 1.5f;

        [Header("經驗值倍率")]
        public float ExpRate = 1f;

        [SerializeField, Header("經驗值上限")]
        private float _levelExpMax = 100;
        [SerializeField, Header("每級所需的基礎經驗值")]
        private float _levelExp = 3f;
        [SerializeField, Header("經驗值指數")]
        private float _levelPow = 1.2f;

        /// <summary>
        /// 取得下一等級的升級經驗值
        /// 計算公式
        /// Level N = Min((N * 基礎經驗值) * (經驗值指數 ^ (N - 1)), 經驗值上限)
        /// </summary>
        public float NextLevelExp 
        { 
            get
            {
                return Mathf.Min(((float)Level * _levelExp) * Mathf.Pow(_levelPow, (Level - 1)), _levelExpMax);
            }
        }


        [Header("適用的升級選項")]
        public OptionData[] Options;

        [Header("升級音效")]
        public AudioClip LevelUpAudio;
        [Header("受擊音效")]
        public AudioClip GetHitAudio;
        [Header("補品掉落機率")]
        public float DropHealthRate = 0.01f;
        [Header("無敵時間")]
        public float InvincibleTime = 1f;
        [Header("防禦力")]
        public float DEF = 0f;
    }
}