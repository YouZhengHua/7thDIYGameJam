using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Option")]
	public class OptionData : ScriptableObject
	{
		[Header("選項類別")]
		public OptionType optionType;
		[Header("選項類別")]
		public OptionAttribute optionAttribute;
		[Header("選項敘述")]
		public string depiction;
		[Header("最大被選取次數"), Range(1, 10)]
		public int maxSelectedCount = 1;
		[SerializeField, Header("數值"), Min(0)]
		private float _value;
		[SerializeField, Header("是否為負值")]
		private bool _isNegative = false;
		/// <summary>
		/// 選項計算值
		/// </summary>
		public float Value { get => _isNegative ? _value * -1 : _value; }
		[Header("是否為最終選項"), Tooltip("如果是最終選項，即在所有可選選項選完前不會出現")]
		public bool IsEndOption = false;
		[Header("武器編號"), Tooltip("如果選項類別是 Weapon 才會有值")]
		public WeaponIndex WeaponIndex;
		/// <summary>
		/// 被選取次數
		/// </summary>
		public int selectedCount { get; set; }
		/// <summary>
		/// 是否達到最大被選取次數
		/// </summary>
		public bool IsSelectedMax { get => selectedCount >= maxSelectedCount && maxSelectedCount != -1; }
	}
}