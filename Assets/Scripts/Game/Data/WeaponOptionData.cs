using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/WeaponOption")]
	public class WeaponOptionData : OptionData
	{
		[SerializeField, Header("武器編號")]
		private WeaponIndex _weaponIndex;
		/// <summary>
		/// 武器編號
		/// </summary>
		public WeaponIndex WeaponIndex { get => _weaponIndex; }
	}
}