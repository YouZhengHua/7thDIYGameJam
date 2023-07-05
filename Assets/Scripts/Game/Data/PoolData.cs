using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Pool")]
	public class PoolData : ScriptableObject
	{
		[Header("預置物")]
		public GameObject prefab;
		[Header("池子大小")]
		public int poolSize;
	}
}