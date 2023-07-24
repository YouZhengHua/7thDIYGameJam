namespace Scripts.Game
{
	/// <summary>
	/// 屬性分類
	/// </summary>
	public enum AttributeType
	{
        #region 武器數值

        #region 通用
        /// <summary>
        /// 調整傷害(固定值)
        /// </summary>
        Damage,
		/// <summary>
		/// 調整武器傷害(倍率)
		/// </summary>
		DamageMultiple,
        #endregion

        #region 投射物
        /// <summary>
        /// 調整投射物射擊速度
        /// </summary>
        ShootCountPreSecond,
		/// <summary>
		/// 調整投射物穿透數量
		/// </summary>
		PenetrationCount,
		/// <summary>
		/// 調整投射物有效射程
		/// </summary>
		EffectiveRange,
		/// <summary>
		/// 調整投射物射出數目
		/// </summary>
		ShootCount,
        #endregion

        #endregion

        #region 玩家數值
        /// <summary>
        /// 增加玩家移動速度(倍率)
        /// </summary>
        PlayerSpeed,
		/// <summary>
		/// 治癒玩家(百分比)
		/// </summary>
		PlayerHeal,
		/// <summary>
		/// 增加玩家最大血量(固定值)
		/// </summary>
		PlayerMaxHealth,
		/// <summary>
		/// 增加玩家防禦力
		/// </summary>
		PlayerDef,
		/// <summary>
		/// 調整經驗值倍率
		/// </summary>
		ExtendExp,
		/// <summary>
		/// 調整拾取掉落物的範圍
		/// </summary>
		GetDropItemRadius,
		/// <summary>
		/// 增加遊戲分數
		/// </summary>
		Score,
		#endregion

		/// <summary>
		/// 傷害範圍
		/// </summary>
		DamageRadius,
		/// <summary>
		/// 投射物飛行速度
		/// </summary>
		AmmoFlySpeed,
		/// <summary>
		/// 投射物體積
		/// </summary>
		AmmoScale,
		/// <summary>
		/// 特殊升級項目
		/// </summary>
		SpecialOption,
		/// <summary>
		/// 恢復護盾值
		/// </summary>
		RecoverShield,
		/// <summary>
		/// 補品掉落率
		/// </summary>
		HealItemRate,
		/// <summary>
		/// 擊退力量
		/// </summary>
		PushForce,
		/// <summary>
		/// Buff 持續時間
		/// </summary>
		BuffTime,
		/// <summary>
		/// 盾牌迴旋持續時間
		/// </summary>
		AmmoFlyTime
    }
}