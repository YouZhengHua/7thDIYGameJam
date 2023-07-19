using System;
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
		[SerializeField, Header("武器升級屬性陣列")]
		private WeaponAttribute[] _weaponUpdateAttributes;
		/// <summary>
		/// 武器升級屬性陣列
		/// </summary>
		public WeaponAttribute[] WeaponUpdateAttributes { get => _weaponUpdateAttributes; }
	}

	[Serializable]
	public struct WeaponAttribute
	{
		[SerializeField, Header("屬性類別")]
		private AttributeType _attributeType;
		/// <summary>
		/// 屬性類別
		/// </summary>
		public AttributeType AttributeType { get => _attributeType; }

		[SerializeField, Header("數值"), Min(0)]
		private float _value;
		[SerializeField, Header("是否為負值")]
		private bool _isNegative;
		/// <summary>
		/// 選項數值
		/// </summary>
		public float Value { get => _isNegative ? _value * -1 : _value; }

		[SerializeField, Header("升級敘述")]
		private string _attributeDepiction;
		/// <summary>
		/// 升級敘述
		/// </summary>
		public string AttributeDepiction { get => _attributeDepiction; }
	}
}

