using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/UserSetting")]
	public class UserSetting : ScriptableObject
	{
		[Header("主武器攻擊")]
		public KeyCode Shoot = KeyCode.Mouse0;
		[Header("近戰武器攻擊")]
		public KeyCode MeleeAttack = KeyCode.F;
		[Header("主動換彈")]
		public KeyCode Reload = KeyCode.R;
		[Header("更換射擊模式")]
		public KeyCode ChangeShootType = KeyCode.V;
		[Header("音效音量"), Range(0, 10)]
		public float soundVolume = 8;
		[Header("音樂音量"), Range(0, 10)]
		public float musicVolume = 7;
	}
}