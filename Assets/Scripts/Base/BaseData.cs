using UnityEngine;

namespace Scripts.Game.Base
{
    [CreateAssetMenu(menuName = "Game/Base")]
	public class BaseData : ScriptableObject
	{
        [Header("血量")]
        [Range(0, 10000)]
        [Tooltip("有機體血量")]
        public float HealthPoint = 1;

        [Header("基礎移動速度")]
        [Range(0, 5000)]
        public float BaseMoveSpeed;

        [Header("移動速度倍率")]
        [Range(0.0f, 100.0f)]
        public float MoveSpeedRate = 1.0f;
    }
}