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
		/// <summary>
		/// 被選取次數
		/// </summary>
		public int selectedCount { get; set; }
		[Header("最大被選取次數")]
		public int maxSelectedCount = 1;
		/// <summary>
		/// 是否達到最大被選取次數
		/// </summary>
		public bool IsSelectedMax { get => selectedCount >= maxSelectedCount && maxSelectedCount != -1; }
		[Header("值")]
		public float _value;
		[Header("是否為負值")]
		public float _isNegative = 1;
		public float Value { get => _value * _isNegative; }
		[Header("是否為最終選項")]
		public bool IsEndOption = false;
	}
}