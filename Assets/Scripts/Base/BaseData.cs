using UnityEngine;

namespace Scripts.Game.Base
{
    [CreateAssetMenu(menuName = "Game/Base")]
	public class BaseData : ScriptableObject
	{
        [Header("血量"), Min(0f)]
        public float HealthPoint;

        [Header("移動速度")]
        public FloatAttributeHandle MoveSpeed;
    }
}