using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/AttributeOptionData")]
	public class AttributeOptionData : OptionData
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
		private bool _isNegative = false;
		/// <summary>
		/// 選項數值
		/// </summary>
		public float Value { get => _isNegative ? _value * -1 : _value; }
	}
}