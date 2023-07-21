using UnityEngine;

namespace Scripts.Game.Data
{
	public abstract class OptionData : ScriptableObject
	{
		[SerializeField, Header("選項類別")]
		protected OptionType _optionType;
		/// <summary>
		/// 選項類別
		/// </summary>
		public OptionType OptionType { get => _optionType; }

		[SerializeField, Header("選項標題")]
		protected string _title;
		/// <summary>
		/// 選項敘述
		/// </summary>
		public string Title { get => _title; }
		[SerializeField, Header("選項敘述")]
		protected string _depiction;
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
		protected int _maxSelectedCount = 1;

		[SerializeField, Header("是否為最終選項"), Tooltip("只有在所有可升級選項都已經選擇完畢才會出現的內容")]
		protected bool _isEndOption = false;
		/// <summary>
		/// 是否為最終選項
		/// </summary>
		public bool IsEndOption { get => _isEndOption; }

		[SerializeField, Header("是否為循環選項"), Tooltip("忽視最大被選取次數的限制")]
		protected bool _isLoopOption = false;
		/// <summary>
		/// 是否為循環選項
		/// </summary>
		public bool IsLoopOption { get => _isLoopOption; }

		/// <summary>
		/// 被選取次數
		/// </summary>
		public int SelectedCount { get; set; }

		/// <summary>
		/// 被選取次數
		/// </summary>
		public int MaxSelectedCount { get => _maxSelectedCount; }

		/// <summary>
		/// 是否達到最大被選取次數
		/// </summary>
		public bool IsSelectedMax { get => SelectedCount >= _maxSelectedCount && !_isLoopOption; }

		[SerializeField, Header("是否為有效選項"), Tooltip("取消勾選後此選項不會在出現在升級選項中")]
		protected bool _isActive = true;
		public bool IsAcitve { get => _isActive; }
	}
}