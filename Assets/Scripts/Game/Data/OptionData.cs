using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Option")]
	public class OptionData : ScriptableObject
	{
		[SerializeField, Header("選項類別")]
		private OptionType _optionType;
		/// <summary>
		/// 選項類別
		/// </summary>
		public OptionType OptionType { get => _optionType; }

		[SerializeField, Header("屬性類別")]
		private AttributeType _attributeType;
		/// <summary>
		/// 屬性類別
		/// </summary>
		public AttributeType AttributeType { get => _attributeType; }

		[SerializeField, Header("選項敘述")]
		private string _depiction;
		/// <summary>
		/// 選項敘述
		/// </summary>
		public string Depiction { get => _depiction; }

		[SerializeField, Header("選項圖片")]
		private Sprite _image;
		/// <summary>
		/// 選項圖片
		/// </summary>
		public Sprite Image { get => _image; }

		[SerializeField, Header("最大被選取次數"), Range(0, 10)]
		private int _maxSelectedCount = 1;

		[SerializeField, Header("數值"), Min(0)]
		private float _value;
		[SerializeField, Header("是否為負值")]
		private bool _isNegative = false;
		/// <summary>
		/// 選項數值
		/// </summary>
		public float Value { get => _isNegative ? _value * -1 : _value; }

		[SerializeField, Header("是否為最終選項"), Tooltip("只有在所有可升級選項都已經選擇完畢才會出現的內容")]
		private bool _isEndOption = false;
		public bool IsEndOption { get => _isEndOption; }

		[SerializeField, Header("是否為循環選項"), Tooltip("忽視最大被選取次數的限制")]
		private bool _isLoopOption = false;
		/// <summary>
		/// 是否為循環選項
		/// </summary>
		public bool IsLoopOption { get => _isLoopOption; }

		[SerializeField, Header("武器編號"), Tooltip("如果選項類別是 Weapon 才會有值")]
		private WeaponIndex _weaponIndex;
		/// <summary>
		/// 武器編號
		/// </summary>
		public WeaponIndex WeaponIndex { get => _weaponIndex; }

		/// <summary>
		/// 被選取次數
		/// </summary>
		public int SelectedCount { get; set; }

		/// <summary>
		/// 是否達到最大被選取次數
		/// </summary>
		public bool IsSelectedMax { get => SelectedCount >= _maxSelectedCount && !_isLoopOption; }
		[SerializeField, Header("是否為有效選項"), Tooltip("取消勾選後此選項不會在出現在升級選項中")]
		private bool _isActive = true;
		public bool IsAcitve { get => _isActive; }
	}
}