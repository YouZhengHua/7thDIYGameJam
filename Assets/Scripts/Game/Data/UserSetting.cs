using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/UserSetting")]
	public class UserSetting : ScriptableObject
	{
		[Header("音效音量"), Range(0, 10)]
		public float soundVolume = 8;
		[Header("音樂音量"), Range(0, 10)]
		public float musicVolume = 7;
	}
}