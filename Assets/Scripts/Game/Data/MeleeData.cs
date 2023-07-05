using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Melee")]
	public class MeleeData : ScriptableObject
	{
        /// <summary>
        /// 每秒攻擊次數
        /// </summary>
        [Header("每秒可以揮舞近戰武器次數")]
        [Min(0.01f)]
        public float AttackPerSecond = 2f;

        /// <summary>
        /// 基礎每秒攻擊次數
        /// </summary>
        public float BaseAttackPerSecond { get => 2f; }

        /// <summary>
        /// 武器基礎大小
        /// </summary>
        public Vector3 BaseScale { get => Vector3.one * 1.2f; }

        /// <summary>
        /// 傷害值
        /// </summary>
        [Header("傷害值")]
        public float Damage = 5;

        /// <summary>
        /// 擊退力量
        /// </summary>
        [Header("擊退力量")]
        public float Force = 100;

        [Header("怪物僵直時間"), Min(0)]
        public float EnemyDelayTime = 0.5f;

        /// <summary>
        /// 攻擊音效
        /// </summary>
        [Header("攻擊音效")]
        public AudioClip AttackAudio;

        /// <summary>
        /// 近戰攻擊時的移動速度比率
        /// </summary>
        [Header("近戰攻擊時的移動速度比率")]
        public float AttackMoveSpeedRate = 0.8f;

        [Header("適用的升級選項")]
        public OptionData[] Options;
    }
}